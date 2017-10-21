RecoverableObject = { }

function RecoverableObject:New(poolName)
    local obj = { }
    setmetatable(obj, self)
    self.__index = self
    obj.isActive = false
    obj.poolName = poolName
    return obj
end

function RecoverableObject:SetActivate()

end

function RecoverableObject:SetUnactivate()

end

function RecoverableObject:Update()

end

function RecoverableObject:Release()

end

return RecoverableObject