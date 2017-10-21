-- region AfterLoaded
----------------------------------------------
-- 加载后事件
AfterLoadedMeta = 
{
    __newindex = function(t,k,v)
        table.insert(t._todo_[k],v)
    end
}

-- UpValues: 上值定义初始化
-- InitEvents: 初始化事件
-- 其它: 待扩展
AfterLoaded =
{
    _todo_ =
    {
        UpValues = {},
        InitEvents = {}
    },
}

function AfterLoaded.Renew()
    -- UpValues
    local todo = AfterLoaded._todo_.UpValues
    for k,v in ipairs(todo) do
        v()
    end

    -- InitEvents
    local todo = AfterLoaded._todo_.InitEvents
    for k,v in ipairs(todo) do
        v()
    end

    -- 其它
end
setmetatable(AfterLoaded, AfterLoadedMeta)
-- endregion
----------------------------------------------------
local DebugLog = UnityEngine.Debug.Log
local DebugLogError = UnityEngine.Debug.LogError
local DebugLogWarning = UnityEngine.Debug.LogWarning
Logger = {}

-- 关闭输出时可对此函数作注释
function Logger._Concat(prefix, split_char, ...)
    local result = prefix
    local params = {...}
    for _,v in pairs(params) do
        if _ == 1 or split_char == nil then
            result = result .. tostring(v)
        else
            result = result .. split_char .. tostring(v)
        end
    end
    return result
end

-- 关闭输出时可对此函数作注释
function Logger._Format(prefix, fmt, ...)
    local ret,ret_str = pcall(string.format, prefix .. fmt, ...)
    if ret == false then
        DebugLogError(ret_str)
        DebugLogError(debug.traceback())
    end
    return ret_str
end

local Logger_Concat = Logger._Concat
local Logger_Format = Logger._Format


function Logger.Fmt(fmt, ...)
    DebugLog(Logger_Format("[Info]: ", fmt, ...))
end

function Logger.FmtError(fmt, ...)
    DebugLogError(Logger_Format("[Error]: ", fmt, ...))
end

function Logger.FmtWarning(fmt, ...)
    DebugLogWarning(Logger_Format("[Warning]: ", fmt, ...))
end

function Logger.Log(...)
    DebugLog(Logger_Concat("[Info]: ", ",", ...))
end

function Logger.LogError(...)
    DebugLogError(Logger_Concat("[Error]: ", ",", ...))
end

function Logger.LogWarning(...)
    DebugLogWarning(Logger_Concat("[Warning]: ", ",", ...))
end

local Logger_Fmt = Logger.Fmt
local Logger_FmtError = Logger.FmtError
local Logger_FmtWarning = Logger.FmtWarning
local Logger_Log = Logger.Log
local Logger_LogError = Logger.LogError
local Logger_LogWarning = Logger.LogWarning

string.Fmt = function (self, ...)
    Logger_Fmt(self, ...)
end
string.FmtError = function (self, ...)
    Logger_FmtError(self, ...)
end
string.FmtWarning = function (self, ...)
    Logger_FmtWarning(self, ...)
end

----------------------------------------------------
-- region WeakTblMeta
local WeakTblMeta = { __mode = "k", }
WeakTbl = { }
function WeakTbl.New()
    local temp = { }
    setmetatable(temp, WeakTblMeta)
    return temp
end
local WeakValTblMeta = { __mode = "v", }
WeakValTbl = { }
function WeakValTbl.New()
    local temp = { }
    setmetatable(temp, WeakValTblMeta)
    return temp
end
-- endregion

-- region TableExtension
-- 复制表(深拷贝)
table.copy = function(src,target, deep_count, ignore_prefix)
    if deep_count == nil then
        deep_count = 1000
    elseif deep_count == 0 then
        return
    end

	if src._proxy ~= nil and type(src._proxy)=="table" then
		src = src._proxy
	end
	for k,v in pairs(src) do
		if ignore_prefix == nil or string.sub(tostring(k),1,1) ~= ignore_prefix then
			if type(v)=="table" then
				local sub_t = target[k]
				if sub_t == nil then
					sub_t = {}
					target[k] = sub_t
				end
				table.copy(v, sub_t, deep_count - 1, ignore_prefix)
			else
				target[k] = v
			end
		end
	end
end

local has_node = next
table.is_empty = function(t)
    return has_node(t) == nil
end

table.iclear = function(t)
    for i = #t, 1, -1 do
        t[i] = nil
    end
end

table.clear = function(t)
    for k, v in pairs(t) do
        t[k] = nil
    end
end

-- 打印表
local table_show_mode = nil
local ser_mark_tbl=WeakTbl.New()
local function ser_table(t, name, ignore_prefix, deep_count, indent)
    if deep_count == nil then
        deep_count = 10
    elseif deep_count == 0 then
        return
    end

    if name == nil then
        name = "[noname]"
    end
    if table_show_mode == nil then
        table_show_mode = print
    end
    if t == nil or type(t) ~= "table" then
        table_show_mode(name .. ": table is nil or is not table")
        return
    end

    if ser_mark_tbl[t] ~= nil then return end
    ser_mark_tbl[t] = 1

    local brackets_indent
    if indent == nil then
        table_show_mode(name .. " = ")
        brackets_indent = ""
        indent = "    "
    else
        brackets_indent = indent
        indent = indent .. "    "
    end

    table_show_mode(brackets_indent .. "{")
    local key = nil
    for k, v in pairs(t) do
        if ignore_prefix == nil or string.sub(tostring(k), 1, 1) ~= ignore_prefix then
            key = type(k) == "number" and "[" .. k .. "]" or k
            if (type(v) == "table") then
                table_show_mode(indent .. tostring(key) .. " = ")
                ser_table(v, name, ignore_prefix, deep_count - 1, indent)
            else
                table_show_mode(indent .. tostring(key) .. " = " .. tostring(v) .. ",")
            end
        end
    end
    table_show_mode(brackets_indent .. "},")
end

local tostring_result = ""
local show_tostring = function(str)
    tostring_result = tostring_result .. str .. "\n"
end

table.tostring = function(t, name, ignore_prefix)
	ser_mark_tbl = WeakTbl.New()
    tostring_result = ""
    table_show_mode = show_tostring
	ser_table(t, name, ignore_prefix)
    return tostring_result
end

table.print = function(t, name, ignore_prefix)
	ser_mark_tbl = WeakTbl.New()
    tostring_result = ""
    table_show_mode = show_tostring
    ser_table(t, name, ignore_prefix)
    Logger_Log("[" .. os.date() .. "] " .. tostring_result, "\n", tostring(t))
end
------------------------------
-- 不可重复设置的表
local UnRepeatableMetatable =
{
    __index = function(t, k) return rawget(t.__org_tbl, k) end,
    __newindex = function(t, k, v)
            local exist = t.__org_tbl[k]
            if exist ~= nil then
                Logger.LogError("Table element is already exist on key:", k, debug.traceback())
            end
            t.__org_tbl[k] = v
        end,
}

local UnRepeatableMetatableWithDebugTrace =
{
    __index = function(t, k)
            local exist = t.__org_tbl[k]
            if exist == nil then return nil end
            return exist.value
        end,
    __newindex = function(t, k, v)
            local exist = t.__org_tbl[k]
            if exist ~= nil then
                Logger.LogError("Table element is already exist on key:", k, debug.traceback(), exist.trace)
            end
            t.__org_tbl[k] = { value = v, trace = debug.traceback() }
        end,
}
table.unrepeatable = function(t)
	if  t == nil or type(t) ~= "table" then
		Logger.Log("table is nil or not table")
		return
	end
    for k,v in pairs(t) do
        if type(v)=="table" then
            t[k] = table.unrepeatable(v)
        end
	end
    
    local proxy =
    {
        __org_tbl = t,
    }
    setmetatable(proxy, UnRepeatableMetatableWithDebugTrace)
    return proxy
end
-- endregion

-- 弧度->角度 (180/π)
local radian_per_angle = (180 / Mathf.PI)
-- 角度转弧度 (π/180)
local angle_per_radian = (Mathf.PI / 180)

math.angle2radian = function(angle)
    return angle * angle_per_radian
end
math.radian2angle = function(radian)
    return radian * radian_per_angle
end

--------------------------------------------------------------------------

string.split = function(text, sep, plain)
     local res={}
     local searchPos=1
     while true do
		 local matchStart, matchEnd=string.find(text, sep, searchPos, plain)
		 if matchStart and matchEnd >= matchStart then
			-- insert string up to separator into result
			table.insert(res, string.sub(text, searchPos, matchStart-1))
			-- continue search after separator
			searchPos=matchEnd+1
		 else
			-- insert whole reminder as result
			table.insert(res, string.sub(text, searchPos))
			break
		 end
     end
     return res
end

string.trim = function(text)
	return string.gsub(text, "^%s*(.-)%s*$", "%1")
end

-- 二进制字符串转换成二进制数据 "110" => 6
string.bin2val = function(bin_str)
    local length = string.len(bin_str)
    local char = "0"
    local val = 0
    for k = 1, length do
        char = string.sub(bin_str, k, k)
        if char ~= "0" then val = val +(2 ^(length - k)) end
    end
    return val
end


-----------------------------------------------------------
local TileHelper = TileHelper
function CmFromTilePos(x, y)
    local pos = {}
    pos.x_ = TileHelper.CmFromTileX(x)
    pos.y_ = TileHelper.CmFromTileY(y)
    return pos
end

-----------------------------------------------------------
math.int_max = 0xffffffff