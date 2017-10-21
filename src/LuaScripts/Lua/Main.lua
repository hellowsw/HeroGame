require "Import"
require "ByInit"

-------------------------------------------------------------------------------------
-- 加载后事件,可定义UpValues函数解决上值引用为空的问题, 避免为此dofile手工调序
AfterLoaded.Renew()

function Main()
    UnityEngine.Debug.Log("main")
    UpdateBeat:Add(Update)
    GameManager.Initialize()
end

function Update()
    GameManager.OnUpdate()
end
