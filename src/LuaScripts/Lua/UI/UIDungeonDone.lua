UIDungeonDone = BaseUI.New()

UIDungeonDone.Instance = nil
local this = nil

function UIDungeonDone.New()
    local obj = BaseUI.New("UIDungeonDone", UIType.GroupUI, false)
    setmetatable(obj, UIDungeonDone)
    UIDungeonDone.__index = UIDungeonDone
    UIDungeonDone.Instance = obj
    obj.Instance = obj
    this = obj
    return obj
end

function UIDungeonDone:Start()
    UIEvent.AddButtonClick(self.transform:FindChild("btnReturn"):GetComponent("Button"), function() 
        GameManager.LoadWorld(MainTown.New())
    end)
end

function UIDungeonDone:OnClose()
    UIDungeonDone.Instance = nil
end

return UIDungeonDone