UIEvent = { }

function UIEvent.AddButtonClick(button, callback, useAnim, sleepTime, highLight)
    button.onClick:AddListener(callback)
    CommonLib.AttachButtonSleep(button.gameObject, sleepTime or 0.5)
    if not useAnim or config.Mobile then
        return CommonLib.AttachCursorEventHandler(button.gameObject, 0, highLight and not config.Mobile)
    end
    local eventHandler = CommonLib.AttachCursorEventHandler(button.gameObject, 0, highLight)
    return UIEvent.AddScaleAnim(eventHandler, button)
end

function UIEvent.SetButtonClick(button, callback, useAnim, sleepTime, highLight)
    CommonLib.ButtonRemoveAllEvent(button)
    button.onClick:AddListener(callback)
    CommonLib.AttachButtonSleep(button.gameObject, sleepTime or 0.5)
    if not useAnim or config.Mobile then
        return CommonLib.AttachCursorEventHandler(button.gameObject, 0, highLight and not config.Mobile)
    end
    local eventHandler = CommonLib.AttachCursorEventHandler(button.gameObject, 0, highLight)
    return UIEvent.AddScaleAnim(eventHandler, button)
end

function UIEvent.AddToggle(toggle, callback)
    toggle.onValueChanged:AddListener(callback)
    return CommonLib.AttachCursorEventHandler(toggle.gameObject, 0, false)
end

function UIEvent.SetToggle(toggle,callback)
    CommonLib.ToggleRemoveAllEvent(toggle)
        toggle.onValueChanged:AddListener(callback)
    CommonLib.AttachCursorEventHandler(toggle.gameObject, 0, false)
end

function UIEvent.SetToggle2(toggle,callback)
    UIEvent.SetToggle(toggle,function (  )
        if toggle.isOn then 
            toggle.transform:FindChild("n").gameObject:SetActive(false)
            toggle.transform:FindChild("h").gameObject:SetActive(true)
        else
            toggle.transform:FindChild("h").gameObject:SetActive(false)
            toggle.transform:FindChild("n").gameObject:SetActive(true)
        end
        callback(toggle.isOn)
    end)
end

function UIEvent.RemoveButtonClick(button)
    CommonLib.ButtonRemoveAllEvent(button)
end

function UIEvent.AddScaleAnim(eventHandler, button)
    eventHandler:SetEnterEvent( function()
        DOTweenUtil.DOScale(eventHandler.transform, Vector3.New(1.1, 1.1, 1), 0.13, 0)
        local com = button:GetComponent("TransformGroup")
        if com then
            com:Scale(1.1)
        end
    end ):SetExitEvent( function()
        DOTweenUtil.DOScale(eventHandler.transform, Vector3.One, 0.13, 0)
        local com = button:GetComponent("TransformGroup")
        if com then
            com:ResetScale()
        end
    end )
    return eventHandler
end

function UIEvent.InputValueChanged(inputField, callback)
    inputField.onValueChanged:AddListener(callback)
end
function UIEvent.SetInputValueChanged(inputField, callback)
    inputField.onValueChanged:RemoveAllListeners()
    inputField.onValueChanged:AddListener(callback)
end
return UIEvent

