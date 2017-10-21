
local GameServer = _G.GameServer
local AccountServer = _G.AccountServer
local Logger = _G.Logger
local GameWorld = nil

NetInterface = { }

NetInterface.kicked = false
NetInterface.logout = false

-- 连接成功
function NetInterface.OnConnectSuccess(server_name)
    Logger.Log("NetInterface.OnConnectSuccess", server_name)
    if server_name == "AS" then

    elseif server_name == "GS" then

    end
end
-- 连接超时
function NetInterface.OnConnectTimeout(server_name)
    Logger.Log("NetInterface.OnConnectTimeout", server_name)
    if server_name == "AS" then
        AccountServer.Instance:Reconnect(-1)
    elseif server_name == "GS" then

    end
end
-- 连接失败
function NetInterface.OnConnectFailed(server_name)
    Logger.Log("NetInterface.OnConnectFailed", server_name)
    if server_name == "AS" then
        -- AccountServer.Instance:Reconnect(-1)
    elseif server_name == "GS" then

    end
end
-- 掉线
function NetInterface.OnDisconnectedNormal(server_name)
    Logger.Log("NetInterface.OnDisconnectedNormal", server_name)
    if server_name == "AS" then
        AccountServer.Instance:Reconnect(-1)

    elseif server_name == "GS" then
        GameServer.Instance:Reconnect(-1)

    end
end
-- 服务器未响应
function NetInterface.OnDisconnectedTimeout(server_name)
    Logger.Log("NetInterface.OnDisconnectedTimeout", server_name)
    if server_name == "AS" then
        AccountServer.Instance:Reconnect(-1)

    elseif server_name == "GS" then

    end
end

function NetInterface.OnBeKicked(server_name)
    if server_name == "AS" then
        Logger.LogError("连接逻辑错误, 不应在帐号服连接中收到OnBeKick事件");
        return
    end

end



function NetInterface.OnSARebindMsg()
    logBlue("NetInterface.OnSARebindMsg")
    CommonMsg.SendZeroZero()
end

function NetInterface.OnLoginSuccess()

end

function NetInterface.OnLoginFaild(result)

end

function NetInterface.OnGoldChange(_type, gold)
    logBlue("NetInterface.OnGoldChange type:" .. _type .. "\tgold:" .. gold)

end

