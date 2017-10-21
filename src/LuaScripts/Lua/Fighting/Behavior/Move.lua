Move = { }

function Move.New(fightRole, from, to, speed)
    local o = Behavior.New(fightRole)
    setmetatable(o, Move)
    Move.__index = Move
    o.from = from
    o.to = to
    o.dir = to - from
    o.time = Vector3.Distance(from, to) / speed
    o.timer = 0
    return o
end

function Move:OnEnter()
    self.fightRole:SetPosition(self.from)
end

function Move:OnExit()
    self.fightRole:SetPosition(self.to)
    self.fightRole:AnimMove(0)
end

function Move:Execute()
    self.timer = self.timer + Time.deltaTime
    if self.timer <= self.time then
        self.fightRole:AnimMove(1)
        self.fightRole:SetPosition(self.from + self.dir * (self.timer / self.time))
    else
        return true
    end
    return false
end

return Move