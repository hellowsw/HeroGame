UIDungeon = BaseUI.New()

UIDungeon.Instance = nil
local this = nil

function UIDungeon.New()
    local obj = BaseUI.New("UIDungeon", UIType.SingleUI, false)
    setmetatable(obj, UIDungeon)
    UIDungeon.__index = UIDungeon
    UIDungeon.Instance = obj
    obj.Instance = obj
    this = obj
    return obj
end

function UIDungeon:Start()
    self.items = { }
    for i = 1, 4 do
        self.items[i] = UIDungeonItem.New(self.transform:FindChild("heros/hero" .. i))
        if not Dungeon.Instance.heros[i] then
            self.items[i].transform.gameObject:SetActive(false)
        end
    end

    MessageCenter.AddListener("on_property_changed", self.OnPropertyChanged, self)
end

--[[ msg
    [1] 角色Pos
    [2] PropertyType
    [3] PropertyType = HP时, 当前hp, PropertyType = CD时, 当前累计时间
    [4] PropertyType = HP时, 最大hp, PropertyType = CD时, 最大时间
]]--
function UIDungeon:OnPropertyChanged(msg)
    if msg[2] == PropertyType.HP then
        self.items[msg[1]]:SetHP(msg[3], msg[4])
    elseif msg[2] == PropertyType.CD then
        self.items[msg[1]]:SetCD(msg[3], msg[4])
    end
end

function UIDungeon:OnClose()
    UIDungeon.Instance = nil
    MessageCenter.RemoveListener("on_property_changed", self.OnPropertyChanged)
end

UIDungeonItem = { }
UIDungeonItem.__index = UIDungeonItem

function UIDungeonItem.New(trans)
    local o = { }
    setmetatable(o, UIDungeonItem)
    o.transform = trans
    o.spHP = trans:FindChild("hp/fg"):GetComponent("Image")
    o.spCD = trans:FindChild("cd/fg"):GetComponent("Image")
    return o
end

function UIDungeonItem:SetHP(hp, maxhp)
    self.spHP.fillAmount = hp / maxhp
end

function UIDungeonItem:SetCD(t, needstime)
    self.spCD.fillAmount = t / needstime
end

return UIDungeon