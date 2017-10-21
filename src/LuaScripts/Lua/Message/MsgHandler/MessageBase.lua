NetMsg = {}

--------------------------------------------------------------------
-- 与服务器间的名字交互
local NameSplitChar = string.char(31)
function GetPlayerNames(full_server_name)
        local temp_names    = string.split(full_server_name, NameSplitChar)
		return temp_names[1],temp_names[2]
end

function MakeFullServerName(server_name, name)
		return server_name .. NameSplitChar .. name
end

--------------------------------------------------------------------
-- 读取数组
function MsgReadArray(stream, read_func, length)
	if length == nil then
		length = stream:ReadUShort()
	end
	local ret = {}
	for i = 0, length - 1 do
		ret[i] = read_func(stream)
	end
	return ret
end

function MsgWriteArray(array_tbl,stream, write_func, length)
    
	if length == nil then
		length = #array_tbl
        stream:WriteShort(length)
	end
    
	for i = 0, length - 1 do
		write_func(stream, array_tbl[i])
	end
end

function MsgNewArray(length)
    local ret = {}
	for i = 0, length - 1 do
		ret[i] = 0
	end
    return ret
end

--------------------------------------------------------------------
-- 读取到缓存
local TBinaryData = TBinaryData
function MsgReadBuffer(stream, length)
	if length == nil then
		length = stream:ReadUShort()
	end
	local ret = TBinaryData(length)
    stream:Read(ret.Buff, 0, length)
    ret:Write(nil, 0, length)
	return ret
end

function MsgWriteBuffer(stream, cs_buff, length)
	if length == nil then
		length = cs_buff.Size
        stream:WriteShort(length)
	end
    stream:Write(cs_buff.Buff, 0, length)
end

-------------------------------------------------------------
-- C++ RPC流

LYBVariant =
{
    VT_EMPTY = 0,        -- 完全的空类型，变量未初始化
    VT_NULL = 1,        -- 空类型，表示没有数据

    VT_RPC_OP_LITE = 4,        -- 特殊控制字符，轻量级版本，但为了向下兼容，以前的VT_RPC_OP也保留

    VT_WORD = 8,        -- [EX]2字节无符号数据
    VT_SHORTINTEGER = 9,        -- [EX]16位带符号整数

    VT_U32_24 = 0x0a,     -- [EX] 原本是UINT32类型，数据压缩后变为24位数据
    VT_I32_24 = 0x0b,     -- [EX] 原本是 INT32类型，数据压缩后变为24位数据
    VT_U64_24 = 0x0c,     -- [EX] 原本是UINT64类型，数据压缩后变为24位数据
    VT_I64_24 = 0x0d,     -- [EX] 原本是 INT64类型，数据压缩后变为24位数据

    VT_DWORD = 0x10,     -- 4字节无符号数据
    VT_INTEGER = 0x11,     -- 32位带符号整数
    VT_FLOAT = 0x12,     -- 单精度浮点数

    VT_U64_56 = 0x1a,     -- [EX] 原本是UINT64类型，数据压缩后变为56位数据
    VT_I64_56 = 0x1b,     -- [EX] 原本是 INT64类型，数据压缩后变为56位数据

    VT_RPC_OP = 0x1f,     -- 特殊控制字符

    VT_QWORD = 0x20,     -- 8字节无符号数据
    VT_LARGINTEGER = 0x21,     -- 64位带符号大整数
    VT_DOUBLE = 0x22,     -- 双精度浮点数
    VT_DATE64 = 0x23,     -- 64位日期

    VT_POINTER = 0x40,     -- 指针类型数据
    VT_STRING = 0x41,     -- 标准字符串
    VT_BSTRING = 0x42,     -- BSTR字符串
    VT_UTF8 = 0x43,     -- UTF8字符串

    -- 压缩用的
    --_UBIT_M32   = 0xff000000,
    --_IBIT_M32   = 0xff800000,
    --_UBIT_M64:Number = 0xffffffffff000000, -- 这两个64位的暂时没用到，被转换为32位分开计算了
    --_IBIT_M64:Number = 0xffffffffff800000,
}

-- 自定义的类型名，方便与PC客户端一致，同时为了序列化统一指定类型
LYBVariant.INT8 = LYBVariant.VT_SHORTINTEGER
LYBVariant.UINT8 = LYBVariant.VT_WORD
LYBVariant.INT16 = LYBVariant.VT_SHORTINTEGER
LYBVariant.UINT16 = LYBVariant.VT_WORD
LYBVariant.INT32 = LYBVariant.VT_INTEGER
LYBVariant.UINT32 = LYBVariant.VT_DWORD
LYBVariant.INT64 = LYBVariant.VT_LARGINTEGER
LYBVariant.UINT64 = LYBVariant.VT_QWORD
LYBVariant.FLOAT = LYBVariant.VT_FLOAT
LYBVariant.DOUBLE = LYBVariant.VT_DOUBLE
LYBVariant.STRING = LYBVariant.VT_STRING
LYBVariant.RPCOP = LYBVariant.VT_RPC_OP
LYBVariant.NULL = LYBVariant.VT_NULL        -- 空类型，表示没有数据

local VarType = LYBVariant
local BitAnd = BitOp32.And
local BitOr = BitOp32.Or
local BitNot = BitOp32.Not
local BitLShift = BitOp32.LShift
local BitRShift = BitOp32.RShift

-----------------------------------------------------------
function CRpcPop(rpc_stream)
    -- CppRpc流头部管理
    if rpc_stream.HeaderSkiped ~= true then
        rpc_stream:SkipHeader()
    end

    local ret = nil
    local type = rpc_stream:ReadByte()

    if type == VarType.VT_EMPTY or
        type == VarType.VT_NULL then
        rpc_stream:Skip(3)

    elseif type == VarType.VT_RPC_OP then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadUInt()
        
    elseif type == VarType.VT_SHORTINTEGER then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadShort()

    elseif type == VarType.VT_WORD then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadUShort()

    elseif type == VarType.VT_INTEGER then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadInt()

    elseif type == VarType.VT_I32_24 then
        local low2 = rpc_stream:ReadUShort()
        local low1 = rpc_stream:ReadSByte() --符号继承
        ret = low1
        ret = BitOr(BitLShift(ret, 16), low2)

    elseif type == VarType.VT_DWORD then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadUInt()

    elseif type == VarType.VT_U32_24 then
        local low2 = rpc_stream:ReadUShort()
        local low1 = rpc_stream:ReadByte()
        ret = low1
        ret = BitOr(BitLShift(ret, 16), low2)

    elseif type == VarType.VT_QWORD then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadUInt64()

    elseif type == VarType.VT_U64_24 then
        local low2 = rpc_stream:ReadUShort()
        local low1 = rpc_stream:ReadByte()
        ret = low1
        ret = BitOr(BitLShift(ret, 16), low2)

    elseif type == VarType.VT_U64_56 then
        local low = rpc_stream:ReadUInt()
        local high2 = rpc_stream:ReadUShort()
        local high1 = rpc_stream:ReadByte()
        ret = high1
        ret = BitOr(BitLShift(ret, 16), high2)
        ret = BitOr(BitLShift(ret, 32), low)

    elseif type == VarType.VT_LARGINTEGER then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadInt64()

    elseif type == VarType.VT_I64_24 then
        local low2 = rpc_stream:ReadUShort()
        local low1 = rpc_stream:ReadSByte()    -- 符号继承
        ret = low1
        ret = BitOr(BitLShift(ret, 16), low2)

    elseif type == VarType.VT_I64_56 then
        local low = rpc_stream:ReadUInt()
        local high2 = rpc_stream:ReadUShort()
        local high1 = rpc_stream:ReadSByte() -- 符号继承
        ret = high1
        ret = BitOr(BitLShift(ret, 16), high2)
        ret = BitOr(BitLShift(ret, 32), low)
        
    elseif type == VarType.VT_FLOAT then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadFloat()
    elseif type == VarType.VT_DOUBLE then
        rpc_stream:Skip(3)
        ret = rpc_stream:ReadDouble()
    elseif type == VarType.VT_UTF8 then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadStringAlign4B(0)   -- EncodeHelper.UTF8
        
    elseif type == VarType.VT_STRING then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadStringAlign4B(1)   -- EncodeHelper.GBK

    elseif type == VarType.VT_BSTRING then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadStringAlign4B(2)   -- EncodeHelper.Unicode

    elseif type == VarType.VT_POINTER then
        rpc_stream:Skip(1)
        ret = rpc_stream:ReadBinaryAlign4B()
    end
    return ret
end

function CRpcPush(rpc_stream, val, type)
    if type == VarType.VT_EMPTY or
        type == VarType.VT_NULL then
        rpc_stream:WriteInt(VarType.VT_EMPTY)
        
    elseif type == VarType.VT_RPC_OP then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteShort(4)
        rpc_stream:WriteUInt(val)
        
    elseif type == VarType.VT_SHORTINTEGER then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteShort(val)

    elseif type == VarType.VT_WORD then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteUShort(val)

    elseif type == VarType.VT_INTEGER then
        rpc_stream:WriteInt(type)
        rpc_stream:WriteInt(val)

    elseif type == VarType.VT_DWORD then
        rpc_stream:WriteInt(type)
        rpc_stream:WriteUInt(val)

    elseif type == VarType.VT_QWORD then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteShort(8)
        rpc_stream:WriteUInt64(val)

    elseif type == VarType.VT_LARGINTEGER then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteShort(8)
        rpc_stream:WriteInt64(val)

    elseif type == VarType.VT_FLOAT then
        rpc_stream:WriteInt(type)
        rpc_stream:WriteFloat(val)

    elseif type == VarType.VT_DOUBLE then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteShort(8)
        rpc_stream:WriteDouble(val)
        
    elseif type == VarType.VT_UTF8 then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteStringAlign4B(val, 0)   -- EncodeHelper.UTF8
        
    elseif type == VarType.VT_STRING then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteStringAlign4B(val, 1)   -- EncodeHelper.GBK

    elseif type == VarType.VT_BSTRING then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteStringAlign4B(val, 2)   -- EncodeHelper.Unicode

    elseif type == VarType.VT_POINTER then
        rpc_stream:WriteShort(type)
        rpc_stream:WriteBinaryAlign4B(val)
    end
end