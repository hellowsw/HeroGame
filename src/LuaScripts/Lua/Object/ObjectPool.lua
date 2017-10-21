ObjectPool = { }

function ObjectPool:New(initSize, onCreate, parentName, root)
    local obj = { }
    setmetatable(obj, self)
    self.__index = self
    obj.size = initSize
    obj.onCreate = onCreate
    obj.list = { }
    if parentName then
        obj.transform = HTransform:CreateById(ObjectManager.NewGameObject(parentName))
        obj.transform:SetParent(root)
    end
    for i = 0, initSize - 1 do
        local poolObj = onCreate()
        if obj.transform and poolObj.transform then
            poolObj.transform:SetParent(obj.transform)
        end
        poolObj:SetUnactivate()
        table.insert(obj.list, poolObj)
    end
    return obj
end

function ObjectPool:Get()
    local obj = nil
    if table.getn(self.list) > 0 then
        obj = self.list[1]
        table.remove(self.list, 1)
    else
        obj = self.onCreate()
        if self.transform then
            obj.transform:SetParent(self.transform)
        end
    end
    return obj
end

function ObjectPool:Store(obj)
    if obj ~= nil then
        obj:SetUnactivate()
        table.insert(self.list, obj)
    end
end

function ObjectPool:Release()
    for k, v in pairs(self.list) do
        v:Release()
    end
end

return ObjectPool