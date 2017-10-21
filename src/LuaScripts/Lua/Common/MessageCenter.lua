MessageCenter = { }

MessageCenter.instance = nil

function MessageCenter.GetInstance()
    if instance == nil then
        local o = { }
        setmetatable(o, MessageCenter)
        o.ListenerDic = { }
        instance = o
    end
    return instance
end

function MessageCenter.AddListener(msgName, callback, instance)
    local ins = MessageCenter.GetInstance()
    if ins.ListenerDic[msgName] == nil then
        ins.ListenerDic[msgName] = { }
    end
    table.insert(ins.ListenerDic[msgName], { callback, instance })
end

function MessageCenter.RemoveListener(msgName, callback)
    local ins = MessageCenter.GetInstance()
    if ins.ListenerDic[msgName] == nil then
        return
    end

    local index = 0
    for k, v in pairs(ins.ListenerDic[msgName]) do
        if v[1] == callback then
            index = k
            break
        end
    end
    if index > 0 then
        table.remove(ins.ListenerDic[msgName], index)
    end
end

function MessageCenter.SendMessage(msgName, msg)
    local ins = MessageCenter.GetInstance()
    if ins.ListenerDic[msgName] == nil then
        return
    end

     for k, v in pairs(ins.ListenerDic[msgName]) do
        v[1](v[2], msg)
     end
end

return MessageCenter