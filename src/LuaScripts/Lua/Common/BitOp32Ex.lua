
local type,pairs,ipairs = type,pairs,ipairs
local tostring = tostring

local MAX_BITS = 32

local BitOp32Ex = {}

function BitOp32Ex.Set( src, begs, ends, num )
	if src == nil or begs == nil or ends == nil or num == nil then return end	
	if begs <= 0 or ends > MAX_BITS then return end
	if begs > ends then return end
	if num > 2 ^(ends - begs +1) - 1 then
		print('BitOp32Ex.Set Value Error')
		return 
	end
	local pre_v = math.floor(src / (2 ^ ends)) * (2 ^ ends)
	local end_v = 0
	begs = begs - 1
	if begs > 0 then
		end_v = src % (2 ^ begs) 
	end
	local new_v = num * (2 ^ begs)	
	local dest = pre_v + new_v + end_v
	
	return dest
end

function BitOp32Ex.Get(src, begs, ends)
	if begs <= 0 or ends > MAX_BITS then return end
	if begs > ends then return end
	src = math.floor(src / (2 ^ (begs - 1)))
	local dest = src % (2 ^(ends - begs + 1))
	
	return dest
end

return BitOp32Ex
