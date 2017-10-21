FightRole = LuaObject.New()
FightRole.__index = FightRole

FightRoleState = {
    Idle = 1,
    DoAction = 2,
    Death = 3,
    WaitFree = 4,
    Freed = 5,
}

FightRoleType = {
    Hero = 1,
    Monster = 2,
}

PropertyType = {
    HP = 1,
    CD = 2,
}

PromptType = {
    Attack = 1,
    Recover = 2,
    CastSkill = 3,
}

function FightRole.New(pos, _type, id, bornPoint, maxHP, cd)
    local o = LuaObject.New()
    setmetatable(o, FightRole)
    o.behaviors = Queue.New()
    o.pos = pos
    o._type = _type
    o.state = FightRoleState.Idle
    o.id = id
    o.maxHP = maxHP
    o.curHP = maxHP
    o.cd = cd
    o.curT = 0
    o.bornPoint = bornPoint
    o.cfg = nil
    if _type == FightRoleType.Hero then
        o.cfg = hero_config[id]
    elseif _type == FightRoleType.Monster then
        o.cfg = monster_config[id]
    end
    o.gameObject = _instantiate("prefabs/roles", o.cfg.image)
    o.transform = o.gameObject.transform
    o.animator = CommonLib.GetComponentInChildren(o.transform, "Animator")
    return o
end

function FightRole:OnStart()
    if self._type == FightRoleType.Hero then
        self:AddBehavior(Move.New(self, self.bornPoint + Vector3.New(-1.2, 0, 0), self.bornPoint, 1.6))
    elseif self._type == FightRoleType.Monster then
        self:AddBehavior(Move.New(self, self.bornPoint + Vector3.New(1.2, 0, 0), self.bornPoint, 1.3))
        self.curT = - math.random(1, 3)
    end
end

function FightRole:OnUpdate()
    if self:IsState(FightRoleState.Death) then
        return
    end

    if self.behaviors:Count() > 0 then
        local beh = self.behaviors:Peek()
        if beh:Execute() then
            beh:OnExit()
            self.behaviors:Dequeue()
        end
    else
        if self:IsState(FightRoleState.WaitFree) then
            Dungeon.Instance:DestroyObject(self)
            return
        end
    end

    self:SetCD(self.curT + Time.deltaTime)
end

function FightRole:AddBehavior(behavior)
    self.behaviors:Enqueue(behavior)
    behavior:OnEnter()
end

function FightRole:OnDestroy()
    GameObject.Destroy(self.gameObject)
end

function FightRole:SetPosition(pos)
    self.transform.position = pos
end

function FightRole:GetPosition()
    if self:IsDead() then
        return self.bornPoint
    end
    return self.transform.position
end

function FightRole:IsDead()
    return self:IsState(FightRoleState.Death) or self:IsState(FightRoleState.WaitFree) or self:IsState(FightRoleState.Freed)
end

function FightRole:GetAnimDelayTime(name)
    return self.cfg.anim_delay_time[name]
end

function FightRole:AnimMove(speed)
    self.animator:SetFloat("Move", speed)
end

function FightRole:AnimSkill(name)
    self.animator:SetTrigger(name)
end

function FightRole:AnimHurt()
    self.animator:SetTrigger("Hurt")
end

function FightRole:AnimDeath()
    self.animator:SetBool("Death", true)
end

function FightRole:IsState(state)
    return self.state == state
end

function FightRole:Ready()
    return self.curT >= self.cd
end

function FightRole:SetCD(t)
    if t ~= self.curT then
        self.curT = math.min(t, self.cd)
        if self._type == FightRoleType.Hero then
            MessageCenter.SendMessage("on_property_changed", { self.pos, PropertyType.CD, t, self.cd })
        end
    end
end

function FightRole:GetASkillId()
    local skillId = self.cfg.skills[math.random(1, #self.cfg.skills)]
    return skillId
end

function FightRole:ShowPrompt(_type, str)
    local fontSize = 25
    local color = nil
    local pos = nil
    if _type == PromptType.Attack then
        color = Color.New(1, 0, 0, 1)
    elseif _type == PromptType.Recover then
        color = Color.New(0, 1, 0, 1)
    elseif _type == PromptType.CastSkill then
        color = Color.New(0, 1, 1, 1)
    end
    if self._type == FightRoleType.Hero then
        pos = self.bornPoint + Vector3.New(1, 0, 0)
    elseif self._type == FightRoleType.Monster then
        pos = self.bornPoint + Vector3.New(-1, 0, 0)
    end
    PromptManager.Show(pos, str, fontSize, color)
end

-- ±»ÖÎÁÆ
function FightRole:BeTreated(power)
    if self:IsDead() then
        return
    end

    self.curHP = math.min(self.maxHP, self.curHP + power)
    self:ShowPrompt(PromptType.Recover, "+" .. power)
    if self._type == FightRoleType.Hero then
        MessageCenter.SendMessage("on_property_changed", { self.pos, PropertyType.HP, self.curHP, self.maxHP })
    end
end

-- ±»¹¥»÷
function FightRole:BeAttacked(power)
    if self:IsDead() then
        return
    end

    self.curHP = math.max(0, self.curHP - power)
    self:ShowPrompt(PromptType.Attack, - power)
    if self._type == FightRoleType.Hero then
        MessageCenter.SendMessage("on_property_changed", { self.pos, PropertyType.HP, self.curHP, self.maxHP })
    else
        self:AnimHurt()
    end
    if self.curHP <= 0 then
        self:AnimDeath()
        self.state = FightRoleState.Death
        GameManager.SendGMessage({ GMsgType.RoleDead, self })
    end
end

return FightRole