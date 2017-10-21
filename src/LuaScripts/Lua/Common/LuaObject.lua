LuaObject = { }
LuaObject.__index = LuaObject
local identity = 1

function LuaObject.New()
    local o = { }
    setmetatable(o, LuaObject)
    o.instanceId = identity
    identity = identity + 1
    return o
end

function LuaObject:OnStart()

end

function LuaObject:OnUpdate()

end

function LuaObject:OnEnable()

end

function LuaObject:OnDisable()

end

function LuaObject:OnDestroy()

end

function LuaObject:StartCoroutine(cor)

end

return LuaObject