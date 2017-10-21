BaseUI = { }

function BaseUI.New(prefabPath, uiType, isHideLastUI, showShadow, playAnim, dontPlaySound)
    local obj = { }
    setmetatable(obj, BaseUI)
    BaseUI.__index = BaseUI
    obj.prefabPath = prefabPath
    obj.uiType = uiType
    obj.isHideLastUI = isHideLastUI
    obj.showShadow = showShadow
    obj.playAnim = playAnim
    obj.transform = nil
    obj.gameObject = nil
    obj.Instance = nil
    obj.cantChangeSeq = false
    obj.Id = 50
    if dontPlaySound == nil then
        dontPlaySound = false
    end
    obj.dontPlaySound = dontPlaySound
    obj.coroutines = { }
    return obj
end

function BaseUI:Start() end

function BaseUI:OnClose() end

function BaseUI:OnEnable() end

function BaseUI:OnDisable() end

function BaseUI:Set(...) end

function BaseUI:Show(...)
    if not self.Instance then
        UIManager.Instance:OpenUI(self.New(...))
    else
        self.Instance:Set(...)
    end
end

function BaseUI:Close()
    if self.Instance ~= nil then
        UIManager.Instance:CloseUI(self.Instance)
    end
end

function BaseUI:SetToUp(targetUI)
    SiblingUP(self.transform, targetUI.transform)
end

function BaseUI:SetToDown(targetUI)
    SiblingDOWN(self.transform, targetUI.transform)
end

function BaseUI:StartCoroutine(func)
    if not func then
        return
    end
    local id = BaseUI.coroutineId
    local cor = StartCoroutine(function() 
        func() 
        self.coroutines[id] = nil
    end)
    self.coroutines[BaseUI.coroutineId] = { [1] = BaseUI.coroutineId, [2] = cor }
    BaseUI.coroutineId = BaseUI.coroutineId + 1
end

function BaseUI:StopAllCoroutine()
    for k, v in pairs(self.coroutines) do
        StopCoroutine(v[2])
    end
    self.coroutines = { }
end

return BaseUI

--[[ **************≈……˙¿‡ƒ£∞Â*****************
ClassName = BaseUI.New()

ClassName.Instance = nil
local this = nil

function ClassName.New()
    local obj = BaseUI.New("prefabName", UIType.GroupUI, true)
    setmetatable(obj, ClassName)
    ClassName.__index = ClassName
    ClassName.Instance = obj
    obj.Instance = obj
    this = obj
    return obj
end

function ClassName:Start()

end

function ClassName:OnClose()
    ClassName.Instance = nil
end

return ClassName
]]--