UIExtremeSkill = BaseUI.New()

UIExtremeSkill.Instance = nil
local this = nil

function UIExtremeSkill.New(image_name, skill_name)
    local obj = BaseUI.New("UIExtremeSkill", UIType.SingleUI, false)
    setmetatable(obj, UIExtremeSkill)
    UIExtremeSkill.__index = UIExtremeSkill
    UIExtremeSkill.Instance = obj
    obj.Instance = obj
    obj.image_name = image_name
    obj.skill_name = skill_name
    logBlue("image:" .. tostring(image_name))
    logBlue("skill:" .. tostring(skill_name))
    this = obj
    return obj
end

function UIExtremeSkill:Start()
    self.spFace = self.transform:FindChild("Image/face"):GetComponent("Image")
    self.txtSkillName = self.transform:FindChild("name"):GetComponent("Text")
    self.spFace.sprite = CommonLib.Texture2DToSprite(_load_resource("textures/hero_face", self.image_name))
    self.txtSkillName.text = self.skill_name
    StartCoroutine(function() 
        WaitForSeconds(2.5)
        UIExtremeSkill:Close()
    end)
end

function UIExtremeSkill:OnClose()
    UIExtremeSkill.Instance = nil
end

return UIExtremeSkill