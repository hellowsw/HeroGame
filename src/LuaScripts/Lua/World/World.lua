World = { }
World.__index = World

function World.New()
    local o = { }
    setmetatable(o, World)
    o.objects = { }
    return o
end

function World:OnEnter()

end

function World:OnUpdate()

end

function World:OnRecGameMessage(msg)

end

function World:OnExit()

end

function World:CreateObject(obj)
    self.objects[obj.instanceId] = obj
    obj:OnStart()
    return obj
end

function World:DestroyObject(obj)
    self.objects[obj.instanceId] = nil
    obj:OnDestroy()
end

function World:UpdateObjects()
    for k, v in pairs(self.objects) do
        v:OnUpdate()
    end
end

return World