Behavior = { }

function Behavior.New(fightRole)
    local o = { }
    setmetatable(o, Behavior)
    o.params = params
    o.running = false
    o.fightRole = fightRole
    return o
end

function Behavior:OnEnter()

end

function Behavior:OnExit()

end

function Behavior:Execute()
    return false
end

return Behavior