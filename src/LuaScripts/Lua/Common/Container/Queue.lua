Queue = { }

function Queue.New()
    local o = { }
    setmetatable(o, Queue)
    Queue.__index = Queue
    o.tab = { }
    return o
end

function Queue:Enqueue(obj)
    self.tab[#self.tab + 1] = obj
    return
end

function Queue:Dequeue()
    if #self.tab > 0 then
        return table.remove(self.tab, 1)
    end
    return nil
end

function Queue:Peek()
    if #self.tab > 0 then
        return self.tab[1]
    end
    return nil
end

function Queue:Contains(obj)
    if #self.tab > 0 then
        for i = 1, #self.tab do
            if obj == self.tab[i] then
                return true
            end
        end
    end
    return false
end

function Queue:Count()
    return #self.tab
end

function Queue:Clear()
    self.tab = { }
end

return Queue