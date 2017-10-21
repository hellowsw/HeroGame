UIManager = {

}

UIManager.Instance = nil

-- 创建UIManager实例
function UIManager:CreateInstance()
    if self.Instance == nil then
        local obj = { }
        setmetatable(obj, self)
        self.__index = self
        obj.gameObject = GameObject.Find("Launcher/UIManager")
        obj.transform = obj.gameObject.transform
        obj.uisList = { }
        obj.camera = GameObject.FindWithTag("UICamera").transform:GetComponent("Camera")
        obj.canvas = obj.gameObject:GetComponent("RectTransform")
        obj.event = obj.transform:FindChild("EventSystem"):GetComponent("EventSystem")
        self.z = 0
        self.Instance = obj
        GameObject.DontDestroyOnLoad(obj.gameObject)
    end
end

-- 打开UI
function UIManager:OpenUI(ui)
    if ui==nil then
        return
    end

    -- 实例化Prefab
    self:NewUI(ui, ResourceManager.GetResourcesFromLocal("UI", ui.prefabPath))
end

function UIManager:NewUI(ui, obj)
    ui.gameObject = GameObject.Instantiate(obj)
    ui.transform = ui.gameObject.transform
    ui.transform.position = Vector3.New(0, 0, self.z)
    ui.rectTransform = CommonLib.GetComponentInChildren(ui.transform, "RectTransform")
    -- 如果是SingleUI
    if ui.uiType == UIType.SingleUI then
        table.insert(self.uisList, ui)
        -- 如果是GroupUI
    elseif ui.uiType == UIType.GroupUI then
        local lastUI = self:GetTopGroupUI()
        table.insert(self.uisList, ui)
        if ui.isHideLastUI then
            if lastUI ~= nil then
                lastUI.gameObject:SetActive(false)
                lastUI:OnDisable()
            end
        end
    end

    ui.transform:SetParent(self.transform)
    ui.transform.localScale = Vector3.New(1, 1, 1)
    ui.rectTransform.offsetMax = Vector2.New(0, 0, 0)
    ui.rectTransform.offsetMin = Vector2.New(0, 0, 0)
    ui:Start()
    ui:OnEnable()
end

function UIManager:GetTopGroupUI()
    for i = #self.uisList, 1, -1 do
        if self.uisList[i].uiType == UIType.GroupUI then
            return self.uisList[i]
        end
    end
    return nil
end

function UIManager:PopTopGroupUI()
    for i = #self.uisList, 1, -1 do
        if self.uisList[i].uiType == UIType.GroupUI then
            local obj = self.uisList[i]
            table.remove(self.uisList, i)
            return obj
        end
    end
    return nil
end

function UIManager:GetTopShadowUI()
    for i = #self.uisList, 1, -1 do
        if self.uisList[i].showShadow then
            local obj = self.uisList[i]
            return obj
        end
    end
    return nil
end

-- 关闭UI
function UIManager:CloseUI(ui)
    -- 如果是SingleUI
    if ui ~= nil and ui.uiType == UIType.SingleUI then
        local index = 0
        for k, v in pairs(self.uisList) do
            if v == ui then
                index = k
                break
            end
        end
        table.remove(self.uisList, index)

        -- 如果是GroupUI
    else
        
        ui = self:PopTopGroupUI()
        local lastUI = self:GetTopGroupUI()
        if ui and ui.isHideLastUI then
            
            if lastUI ~= nil then
                lastUI.gameObject:SetActive(true)
                lastUI:OnEnable()
            end
        end
    end

    -- 删除GameObject

    if ui then
        if ui.on_close then 
            ui.on_close()
        end
        ui:StopAllCoroutine()
        ui:OnClose()
        GameObject.Destroy(ui.gameObject)
    end
end

-- 关闭全部GroupUI
function UIManager:CloseAllGroupUI()
    for i = #self.uisList, 1, -1 do
        if self.uisList[i].uiType == UIType.GroupUI then
            self:CloseUI(self.uisList[i])
        end
    end
end

return UIManager