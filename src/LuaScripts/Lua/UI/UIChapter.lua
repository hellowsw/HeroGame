UIChapter = BaseUI.New()

UIChapter.Instance = nil
local this = nil

function UIChapter.New()
    local obj = BaseUI.New("UIChapter", UIType.GroupUI, true)
    setmetatable(obj, UIChapter)
    UIChapter.__index = UIChapter
    UIChapter.Instance = obj
    obj.Instance = obj
    this = obj
    return obj
end

function UIChapter:Start()
    UIEvent.AddButtonClick(self.transform:FindChild("btnClose"):GetComponent("Button"), function() 
        UIManager.Instance:CloseUI()
    end)
    for i = 1, 4 do
        local button = self.transform:FindChild("page/levels/level0" .. i):GetComponent("Button")
        UIEvent.AddButtonClick(button, function()
            local dungeon = Dungeon.New()
            table.insert(dungeon.heros, { id = 1001, hp = 200, cd = 1.8 })
            table.insert(dungeon.heros, { id = 1003, hp = 160, cd = 1.9 })
            dungeon.mapId = 1000 + i
            GameManager.LoadWorld(dungeon)
        end )
    end
end

function UIChapter:OnClose()
    UIChapter.Instance = nil
end

return UIChapter