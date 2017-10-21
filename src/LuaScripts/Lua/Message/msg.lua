--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

-- rpc service table, implements all rpc call function from server
NetMsgHandlers = {}
local pcall = pcall
local Logger = Logger

function OnNetMessage(firstID, secondID, msgData)

   -- Logger.Log("msgasd:"..tostring(firstID).."::"..tostring(secondID)..":"..tostring(msgData))
	local secondTbl = NetMsgHandlers[firstID]
	if secondTbl == nil then
		Logger.FmtWarning("Msg Handler ==========:  [%d][%d] is not exist!", firstID, secondID)
		return
	end
	local handler = secondTbl[secondID]
	if handler == nil then
		Logger.FmtWarning("Msg Handler ==========:  [%d][%d] is not exist!", firstID, secondID)
		return
	end

    handler(msgData)
    --[[
	local ret,errmsg = pcall(handler, msgData)
    if ret == false then
        Debug.LogError(debug.traceback())
        Debug.LogError(errmsg)
		Logger.FmtWarning("OnNetMessage call error :  [%d][%d] !", firstID, secondID)
    end
    --]]
end

function AddHandlerToMsgHandler(msg_type)
    local Handlers = NetMsgHandlers[msg_type]
    if Handlers == nil then
        Handlers = table.unrepeatable({})
        NetMsgHandlers[msg_type] = Handlers
    end
    return Handlers
end