GameManager = { }
GameManager.world = nil
this = GameManager

function GameManager.LoadWorld(world)
    if this.world ~= nil then
        this.world:OnExit()
    end
    this.world = world
    this.world:OnEnter()
end

function GameManager.Initialize()
    logBlue("GameManager Initialize")
    UIManager:CreateInstance()
    PromptManager.Init()
    this.LoadWorld(MainTown.New())
end

function GameManager.Uninitialize()
    logBlue("GameManager Uninitialize")
    if this.world then this.world:OnExit() end
end

function GameManager.OnUpdate()
    if this.world then 
        this.world:OnUpdate() 
        this.world:UpdateObjects() 
    end
end

function GameManager.SendGMessage(msg)
    if this.world then this.world:OnRecGameMessage(msg) end
end

return GameManager