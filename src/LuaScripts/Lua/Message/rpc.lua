-- region *.lua
-- Date
-- 此文件由[BabeLua]插件自动生成

-- rpc service table, implements all rpc call function from server
RpcService = { }
RpcServiceMsg = { }
local Logger = Logger
local GameWorld = _G["GameWorld"]
local ID_NamedFuncRpc = 10  -- 上行使用
local IDsNamedFuncRpc =     -- 下行解析用, 后续均为命名调用: 8, 9, 10, 11, 13, 14, 15
{
    [10] = 1,
}
local pcall = pcall

function OnLuaRpcCall(unused1, unused2, rpc_info_table)
    -- Do some checks.
    --table.print(rpc_info_table, "OnLuaRpcCall")

    local ids = rpc_info_table.ids

    if IDsNamedFuncRpc[ids[1]] ~= nil then
        -- 函数式RPC, 格式为{ids={}, arg={}, f=""}
        local rpc_func = RpcService[rpc_info_table.f]
        if rpc_func == nil then
            Logger.FmtWarning("Named Rpc =====: [ %s ] is not exist! %s", rpc_info_table.f, table.tostring(rpc_info_table))
            return
        end
        -- rpc_func(unpack(rpc_info_table.arg))
        rpc_func(rpc_info_table.arg)
    else
        -- 协议式RPC, 格式为{ids={}, ...}, 其中...为参数
        local secondTbl = RpcServiceMsg[ids[1]]
        if secondTbl == nil then
            Logger.FmtWarning("Rpc =====: {%d,%d} is not exist! %s", ids[1], ids[2], table.tostring(rpc_info_table))
            return
        end
        local rpc_func = secondTbl[ids[2]]
        if rpc_func == nil then
            Logger.FmtWarning("Rpc =====: {%d,%d} is not exist! %s", ids[1], ids[2], table.tostring(rpc_info_table))
            return
        end
        rpc_info_table.ids = nil
        -- rpc_func(unpack(rpc_info_table))
        rpc_func(rpc_info_table)
    end
end

--------------------------------------------------------------------------------
local RpcMeta = { }
--------------------------------------------------------------------------------
local LybTmpIds = { 0, 0 }
local NamedRpcTable = { }
function NamcRpcCSProxy(func_name, params)
    -- 函数式RPC, 格式为{ids={}, arg={}, f=""}
    LybTmpIds[1] = ID_NamedFuncRpc
    LybTmpIds[2] = 0
    NamedRpcTable.ids = LybTmpIds
    NamedRpcTable.f = func_name
    NamedRpcTable.arg = params

    RpcProxy.Call(ID_NamedFuncRpc, 0, NamedRpcTable, "_")
end

RpcMeta.__index = function(proxy_table, func_name)
    return function(...)
        NamcRpcCSProxy(func_name, { ...})
    end
end

ServerRpc = { }

--------------------------------------------------------------------------------
function RpcCSProxy(first_id, second_id, params)
    -- 协议式RPC, 格式为{ids={}, ...}, 其中...为参数
    local rpc_table = params
    LybTmpIds[1] = first_id
    LybTmpIds[2] = second_id
    rpc_table.ids = LybTmpIds

    RpcProxy.Call(first_id, second_id, rpc_table, "_")
end

--[[
ServerRpc.Call = function(first_id, second_id)
	return function (...)
		RpcCSProxy(first_id, second_id, {...})
	end
end
--]]

-- LYB RPC 只有一个msg参数，可以直接掉RpcCSProxy
ServerRpc.Call = function(first_id, second_id)
    return function(msg)
        RpcCSProxy(first_id, second_id, msg)
    end
end

--------------------------------------------------------------------------------
local GmMsgIds = { 3, 255 }
local GmRpcTable = { }
function GMRpcCSProxy(cmd_name, params)
    -- GM RPC, 格式为{ids={}, gCMD = "", args = {}}
    GmRpcTable.ids = GmMsgIds
    GmRpcTable.gCMD = cmd_name
    GmRpcTable.args = params

    RpcProxy.Call(3, 255, GmRpcTable, "_")
end

ServerRpc.GM = function(cmd_name)
    return function(...)
        GMRpcCSProxy(cmd_name, { ...})
    end
end

--------------------------------------------------------------------------------
setmetatable(ServerRpc, RpcMeta)
-- rpc test code
-- ServerRpc.ServerFuncName(12,123,14,345,546)
-- ServerRpc.Call(12,32)(a,b,b,c,d)
-- ServerRpc.GM("ko")(a,b,b,c,d)

function AddHandlerToRpcServiceMsg(rpc_type)
    local Handlers = RpcServiceMsg[rpc_type]
    if Handlers == nil then
        Handlers = { }
        RpcServiceMsg[rpc_type] = Handlers
    end
    return Handlers
end


-- endregion

