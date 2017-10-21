Dungeon = World.New()
Dungeon.Instance = nil
Dungeon.isLoaded = false
Dungeon.MaxHeroNum = 4
Dungeon.MaxEnemyNum = 4
local this = nil

DungeonState = {
    Fighting = 1,
    Lose = 2,
    Victory = 3,
}

function Dungeon.New()
    local o = World.New()
    setmetatable(o, Dungeon)
    Dungeon.__index = Dungeon
    o.heros = { }
    o.mapId = 0
    o.curEnemyIndex = 1
    o.doneTime = 0
    this = o
    Dungeon.Instance = o
    return o
end

function Dungeon:OnEnter()
    logBlue("Dungeon:OnEnter")
    ResourceManager.LoadScene("Dungeon", self.OnSceneLoaded)
end

function Dungeon.OnSceneLoaded()
    logBlue("Dungeon.OnSceneLoaded")
    this:LoadDungeon()
end

function Dungeon:LoadDungeon()
    -- 加载背景图
    self.dungeonCfg = dungeon_config[self.mapId]
    _instantiate("prefabs/maps", self.dungeonCfg.bg)

    -- 加载角色
    self.fightHeros = { }
    for i = 1, #self.heros do
        self.fightHeros[i] = self:CreateObject(FightRole.New(i, FightRoleType.Hero, self.heros[i].id, hero_born_point[i], self.heros[i].hp, self.heros[i].cd))
    end

    self:CreateMonster()

    UIDungeon:Show()
    Dungeon.isLoaded = true
    self.state = DungeonState.Fighting
end

function Dungeon:CreateMonster()
    self.monsters = { }
    local enemies = self.dungeonCfg.enemies[self.curEnemyIndex]
    self.curEnemyIndex = self.curEnemyIndex + 1
    if enemies == nil then
        return false
    end

    for i = 1, #enemies do
        if enemies[i] and enemies[i] ~= 0 then
            local cfg = monster_config[enemies[i]]
            self.monsters[i] = self:CreateObject(FightRole.New(i, FightRoleType.Monster, enemies[i], monster_born_point[i], cfg.hp, cfg.cd))
        end
    end

    return true
end

function Dungeon:Victory()
    for i = 1, Dungeon.MaxHeroNum do
        if self.fightHeros[i] and not self.fightHeros[i]:IsState(FightRoleState.Death) then
            self.fightHeros[i]:AddBehavior(Move.New(self.fightHeros[i], self.fightHeros[i].bornPoint, self.fightHeros[i].bornPoint + Vector3.New(10, 0, 0), 3))
        end
    end
    self.state = DungeonState.Victory
    self.doneTime = Time.time
end

function Dungeon:Lose()
    self.state = DungeonState.Lose
    self.doneTime = Time.time
end

function Dungeon:OnExit()
    logBlue("Dungeon:OnExit")
    Dungeon.isLoaded = false
    UIDungeon:Close()
    UIManager.Instance:CloseAllGroupUI()
end

function Dungeon:OnUpdate()
    if not Dungeon.isLoaded then
        return
    end

    if self.state ~= DungeonState.Fighting then
        if Time.time - self.doneTime > 2 and not UIDungeonDone.Instance then
            UIManager.Instance:OpenUI(UIDungeonDone.New())
        end
        return
    end

    if InputState.GetTouchDown(0) then
        UIExtremeSkill:Show()
    end

    self:DoAction(self.fightHeros, FightRoleType.Hero, FightRoleType.Monster)
    self:DoAction(self.monsters, FightRoleType.Monster, FightRoleType.Hero)
end

function Dungeon:DoAction(roles, selfType, targetType)
    for i = 1, #roles do
        local role = roles[i]
        if role and role:Ready() then
            local skillId = role:GetASkillId()
            local skillCfg = skill_config[skillId]
            local targetAmountType = skillCfg._target_type or target_type.single
            local target = nil
            if targetAmountType == target_type.single then
                if skillCfg._effect_type == effect_type.recover then
                    target = self:GetARandomTarget(selfType, skillCfg._effect_type)
                else
                    target = self:GetARandomTarget(targetType, skillCfg._effect_type)
                end
            elseif targetAmountType == target_type.all then
                if skillCfg._effect_type == effect_type.recover then
                    target = self:GetAllTarget(selfType, skillCfg._effect_type)
                else
                    target = self:GetAllTarget(targetType, skillCfg._effect_type)
                end
            end
            if target then
                role:AddBehavior(Skill.New(role, skillId, target))
                role:SetCD(0)
            end
        end
    end
end

function Dungeon:GetARandomTarget(_type, effectType)
    local list = { }
    if _type == FightRoleType.Hero then
        for k, v in pairs(self.fightHeros) do
            if self:CheckTarget(v, effectType) then
                table.insert(list, v)
            end
        end
    elseif _type == FightRoleType.Monster then
        for k, v in pairs(self.monsters) do
            if self:CheckTarget(v, effectType) then
                table.insert(list, v)
            end
        end
    end
    if #list == 0 then return nil end
    return list[math.random(1, #list)]
end

function Dungeon:GetAllTarget(_type, effectType)
    local list = { }
    if _type == FightRoleType.Hero then
        for k, v in pairs(self.fightHeros) do
            if self:CheckTarget(v, effectType) then
                table.insert(list, v)
            end
        end
    elseif _type == FightRoleType.Monster then
        for k, v in pairs(self.monsters) do
            if self:CheckTarget(v, effectType) then
                table.insert(list, v)
            end
        end
    end
    if #list == 0 then return nil end
    return list
end

function Dungeon:CheckTarget(target, effectType)
    if not target or target:IsState(FightRoleState.Death) then
        return false
    end
    if effectType == effect_type.recover then
        return target.curHP < target.maxHP
    end
    return true
end

function Dungeon:OnMonsterDead(monster)
    self.monsters[monster.pos] = nil
    monster.state = FightRoleState.WaitFree
    local clear = true
    for i = 1, Dungeon.MaxEnemyNum do
        if self.monsters[i] then
            clear = false
            break
        end
    end
    if clear then
        if not self:CreateMonster() then
            PromptManager.Show(Vector3.New(), "战斗胜利", 66, Color.New(1, 1, 0, 1))
            self:Victory()
        end
    end
end

function Dungeon:OnHeroDead(hero)
    local clear = true
    for i = 1, Dungeon.MaxHeroNum do
        if self.fightHeros[i] and not self.fightHeros[i]:IsDead() then
            clear = false
            break
        end
    end
    if clear then
        PromptManager.Show(Vector3.New(), "战斗失败", 66, Color.New(1, 0, 0, 1))
        self:Lose()
    end
end

function Dungeon:OnRecGameMessage(msg)
    if msg[1] == GMsgType.RoleDead then
        if msg[2]._type == FightRoleType.Monster then
            self:OnMonsterDead(msg[2])
        elseif msg[2]._type == FightRoleType.Hero then
            self:OnHeroDead(msg[2])
        end
    end
end

return Dungeon