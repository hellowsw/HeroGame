UIMainTown = BaseUI.New()

UIMainTown.Instance = nil
local this = nil

function UIMainTown.New()
    local obj = BaseUI.New("UIMainTown", UIType.SingleUI, false)
    setmetatable(obj, UIMainTown)
    UIMainTown.__index = UIMainTown
    UIMainTown.Instance = obj
    obj.Instance = obj
    this = obj
    return obj
end

function UIMainTown:Start()
    self.btnBar = self.transform:FindChild("Building/Bar"):GetComponent("Button")
    self.btnDungeon = self.transform:FindChild("Building/Dungeon"):GetComponent("Button")

    UIEvent.AddButtonClick(self.btnDungeon, function() 
        UIManager.Instance:OpenUI(UIChapter.New())
    end)
end

function UIMainTown:OnClose()
    UIMainTown.Instance = nil
end

return UIMainTown