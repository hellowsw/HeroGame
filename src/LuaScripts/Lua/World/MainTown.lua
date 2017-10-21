MainTown = World.New()

function MainTown.New()
    local o = World.New()
    setmetatable(o, MainTown)
    MainTown.__index = MainTown
    return o
end

function MainTown:OnEnter()
    logBlue("MainTown:OnEnter")
    ResourceManager.LoadScene("Town", self.OnSceneLoaded)
end

function MainTown.OnSceneLoaded()
    logBlue("MainTown.OnSceneLoaded")
    UIManager.Instance:OpenUI(UIMainTown.New())
end

function MainTown:OnExit()
    logBlue("MainTown:OnExit")
    UIMainTown:Close()
    UIManager.Instance:CloseAllGroupUI()
end

function MainTown:OnUpdate()
    
end

function MainTown:OnRecGameMessage(msg)
    logBlue("MainTown:OnRecGameMessage")
end

return MainTown