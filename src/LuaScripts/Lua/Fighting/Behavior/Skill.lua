Skill = { }

function Skill.New(fightRole, skillId, targetRole)
    local o = Behavior.New(fightRole)
    setmetatable(o, Skill)
    Skill.__index = Skill
    o.skillCfg = skill_config[skillId]
    o.targetRole = targetRole
    o.timer = 0
    o.state = SkillState.Prepare
    return o
end

function Skill:OnEnter()
    self.delay = self.fightRole:GetAnimDelayTime(self.skillCfg._anim)
    self.fightRole:AnimSkill(self.skillCfg._anim)
    self.fightRole:ShowPrompt(PromptType.CastSkill, self.skillCfg._name)
    if self.skillCfg.extreme then
        UIExtremeSkill:Show(self.fightRole.cfg.image, self.skillCfg._name)
    end
end

function Skill:OnExit()

end

function Skill:Execute()
    if self.targetRole == nil then
        return true
    end
    self.timer = self.timer + Time.deltaTime
    -- 技能准备动作
    if self.state == SkillState.Prepare then
        if self.timer > self.delay then
            self.ready = true
            self.timer = 0
            if self.skillCfg._cast_type == cast_type.far then
                self.state = SkillState.Cast
                if self.skillCfg._target_type ~= target_type.all then
                    self.becastTrans = _instantiate("prefabs/effect/becast", self.skillCfg.be_cast_effect).transform
                    self.becastTrans.position = self.fightRole:GetPosition()
                    self.dir = self.targetRole:GetPosition() - self.fightRole:GetPosition()
                    self.time = self.dir:Magnitude() / self.skillCfg._speed
                end
            else
                self.state = SkillState.Ready
            end
        end
        -- 技能发射
    elseif self.state == SkillState.Cast then
        if self.timer <= self.time then
            self.becastTrans.position = self.fightRole:GetPosition() + self.dir * (self.timer / self.time)
        else
            self.becastTrans.position = self.targetRole:GetPosition()
            GameObject.Destroy(self.becastTrans.gameObject)
            self.state = SkillState.Ready
        end
        -- 技能对目标生效
    elseif self.state == SkillState.Ready then
        if self.skillCfg._target_type == target_type.all then
            for k, v in pairs(self.targetRole) do
                self:EffectToAsSoon(v)
            end
        else
            self:EffectToAsSoon(self.targetRole)
        end
        self.state = SkillState.Done
        return true
    end
    return false
end

function Skill:EffectToAsSoon(target)
    if self.skillCfg.explode_effect then
        local go = _instantiate("prefabs/effect/explode", self.skillCfg.explode_effect)
        go.transform.position = target:GetPosition()
    end
    if self.skillCfg._effect_type == effect_type.recover then
        target:BeTreated(self.skillCfg._power)
    elseif self.skillCfg._effect_type == effect_type.damage then
        target:BeAttacked(self.skillCfg._power)
    end
end

SkillState = {
    Prepare = 1,
    Cast = 2,
    Ready = 3,
    Done = 4,
}

return