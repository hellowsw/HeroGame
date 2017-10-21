GridMap = { }

function GridMap:New(cell_count_x, cell_count_y, min_x, max_x, min_y, max_y)
    local obj = { }
    setmetatable(obj, self)
    self.__index = self
    obj.cell_count_x = cell_count_x
    obj.cell_count_y = cell_count_y
    obj.min_x = min_x
    obj.max_x = max_x
    obj.min_y = min_y
    obj.max_y = max_y
    obj.width = max_x - min_x
    obj.height = max_y - min_y
    obj.per_cell_width_reciprocal = 1 /(obj.width / cell_count_x)
    obj.per_cell_height_reciprocal = 1 /(obj.height / cell_count_y)
    obj.tab = { }
    obj.cacheTab = { }
    obj.cacheLen = 0
    obj.cacheIndex = 0

    for x = 1, cell_count_x do
        obj.tab[x] = { }
        for y = 1, cell_count_y do
            obj.tab[x][y] = { }
        end
    end
    return obj
end

function GridMap:GetRange(x, y)
    --    self.cacheTab[1] = nil
    --    self.cacheTab[2] = nil
    --    self.cacheTab[3] = nil
    --    self.cacheTab[4] = nil
    --    self.cacheTab[5] = nil

    --    table.insert(self.cacheTab, self:Get(x, y))

    --    if x > 1 then
    --        table.insert(self.cacheTab, self:Get(x - 1, y))
    --    end
    --    if x < self.cell_count_x then
    --        table.insert(self.cacheTab, self:Get(x + 1, y))
    --    end
    --    if y > 1 then
    --        table.insert(self.cacheTab, self:Get(x, y - 1))
    --    end
    --    if y < self.cell_count_y then
    --        table.insert(self.cacheTab, self:Get(x, y + 1))
    --    end
    --    return self.cacheTab

    self.cacheLen = 0
    self.cacheIndex = 1
    for k, v in pairs(self:Get(x, y)) do
        self.cacheTab[self.cacheIndex] = v
        self.cacheIndex = self.cacheIndex + 1
        self.cacheLen = self.cacheLen + 1
    end
    if x > 1 then
        for k, v in pairs(self:Get(x - 1, y)) do
            self.cacheTab[self.cacheIndex] = v
            self.cacheIndex = self.cacheIndex + 1
            self.cacheLen = self.cacheLen + 1
        end
    end
    if x < self.cell_count_x then
        for k, v in pairs(self:Get(x + 1, y)) do
            self.cacheTab[self.cacheIndex] = v
            self.cacheIndex = self.cacheIndex + 1
            self.cacheLen = self.cacheLen + 1
        end
    end
    if y > 1 then
        for k, v in pairs(self:Get(x, y - 1)) do
            self.cacheTab[self.cacheIndex] = v
            self.cacheIndex = self.cacheIndex + 1
            self.cacheLen = self.cacheLen + 1
        end
    end
    if y < self.cell_count_y then
        for k, v in pairs(self:Get(x, y + 1)) do
            self.cacheTab[self.cacheIndex] = v
            self.cacheIndex = self.cacheIndex + 1
            self.cacheLen = self.cacheLen + 1
        end
    end

    return self.cacheTab, self.cacheLen
end

function GridMap:GetElementXY(world_x, world_y)
    return math.floor((world_x + self.max_x) * self.per_cell_width_reciprocal) + 1, math.floor((world_y + self.max_y) * self.per_cell_height_reciprocal) + 1
end

function GridMap:Add(x, y, key, obj)
    if self.tab[x][y] == nil then
        self.tab[x][y] = { }
    end
    self.tab[x][y][key] = obj
end

function GridMap:Get(x, y)
    return self.tab[x][y]
end

function GridMap:AddByWorldPos(world_x, world_y, key, obj)
    local x, y = self:GetElementXY(world_x, world_y)
    return self:Add(x, y, key, obj)
end

function GridMap:Remove(x, y, key)
    self.tab[x][y][key] = nil
end

function GridMap:Clear()
    for x = 1, self.cell_count_x do
        for y = 1, self.cell_count_y do
            self.tab[x][y] = nil
        end
    end
    self.cacheTab = nil
end

return GridMap
