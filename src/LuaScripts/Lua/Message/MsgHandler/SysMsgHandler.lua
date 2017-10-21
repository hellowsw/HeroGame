----------------------------------------------------------------------
-- 描述
----------------------------------------------------------------------
-- 包含
require "Message/MsgHandler/CppRpcHandler"
----------------------------------------------------------------------
-- 上值定义
local SPMPS = Network.DataDefs.NetMsgDef.SPMPS
local SCMPS = Network.DataDefs.NetMsgDef.SCMPS
local GameWorld = GameWorld
local CppRpcHandlers = CppRpcHandlers
local CRpcPop = CRpcPop
local Logger = Logger

----------------------------------------------------------------------
-- 实现
local Handlers = AddHandlerToMsgHandler(SPMPS.EPRO_SYSTEM_MESSAGE)
----------------------------------------------------------------------

----------------------------------------------------------------------
local SAChDataMsg = NetMsg.SAChDataMsg
Handlers[SCMPS.EPRO_CHARACTER_DATA_INFO] = function(stream)
    local msg = SAChDataMsg:Read(stream)
	local data = msg.data_
    MasterPlayer:CreateInstance(data)
end

----------------------------------------------------------------------
local SAClientRPCOPMsg = NetMsg.SAClientRPCOPMsg


Handlers[SCMPS.EPRO_CLIENT_REQUEST_RPCOP] = function(stream)
    local msg = SAClientRPCOPMsg:Read(stream)
    
    local rpc_stream = msg.rpc_stream_
    local rpc_msg = {}
    look2("收到消息:",msg)
    logRed("收到消息SAClientRPCOPMsg = NetMsg.SAClientRPCOPMsg:"..msg.type_)
	-- opType  1 成功 0 调用存储过程错 -1 非法的客户端 -5 系统繁忙，请稍后再试（前台提示）-2 非法的功能id -3 序列化字节不足 -4 参数解析过程中遇到错误
	-- 返回0 或 -4时，具体错误信息 解析streamData获得 （功能id）（错误编号）（当前序列化到的序号）（错误信息描述）
    if msg.type_==-1 then 
        UIPrompt.Show(getPrompt(70))
        elseif msg.type_ == -5 then 
        
      --  UIPrompt.Show("非法的客户端.")
        elseif msg.type_ ==  -2 then 
      --  UIPrompt.Show("非法的客户端.")
        elseif msg.type_ == -3 then 
      --  UIPrompt.Show("序列化字节不足.")
        elseif msg.type_ == -4 then 
       -- UIPrompt.Show("参数解析过程中遇到错误.")
              elseif msg.type_ == -5 then 
      --  UIPrompt.Show("系统繁忙.")
    end


    if msg.type_ == 0 or msg.type_ == -4 then
        rpc_msg.typeid = CRpcPop(rpc_stream)  -- 功能id
		rpc_msg.errorid = CRpcPop(rpc_stream) -- 错误编号
		rpc_msg.indexid = CRpcPop(rpc_stream) -- 序列化编号 - 13  为前端
		rpc_msg.str = CRpcPop(rpc_stream)
        Logger.Log("SAClientRPCOPMsg", msg.type_, table.tostring(rpc_msg))

    elseif msg.type_ > 0 then

        CRpcPop(rpc_stream)
        CRpcPop(rpc_stream)
	    local sid = CRpcPop(rpc_stream)
	    local opType = CRpcPop(rpc_stream)
        local handler = CppRpcHandlers[opType]
        if handler == nil then
            Logger.FmtWarning("Cpp Rpc handler not exist, code:%d", opType)
            return
        end
        handler(rpc_stream)
    else
        Logger.Log("SAClientRPCOPMsg", msg.type_)
    end
end