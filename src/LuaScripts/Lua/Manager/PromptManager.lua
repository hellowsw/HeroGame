PromptManager = { }
PromptManager.canvasTrans = nil
PromptManager.items = { }

function PromptManager.Init()
    PromptManager.canvasTrans = GameObject.Find("Launcher/WorldCanvas").transform
end

function PromptManager.Show(pos, str, fontSize, color)
    local trans = _instantiate("prefabs", "prompt").transform
    trans:SetParent(PromptManager.canvasTrans)
    trans.position = pos
    trans.localScale = Vector3.New(1, 1, 1)
    local text = trans:GetComponent("Text")
    text.fontSize = fontSize
    text.color = color
    text.text = str
    DOTweenUtil.DOMoveWithTimeScale(trans, pos + Vector3.New(0, 1, 0), 1, 0.3)
    GameObject.Destroy(trans.gameObject, 1.3)
end

return PromptManager