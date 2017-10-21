local BitAnd = BitOp32.And
local BitOr = BitOp32.Or
local BitNot = BitOp32.Not
local BitLShift = BitOp32.LShift
local BitRShift = BitOp32.RShift

local ReadArray = MsgReadArray
local WriteArray = MsgWriteArray
local ReadBuffer = MsgReadBuffer
local WriteBuffer = MsgWriteBuffer

local LYBGlobalConsts = LYBGlobalConsts
local log = UnityEngine.Debug.Log

local CRpcPop = nil
function AfterLoaded.UpValues()
    CRpcPop = _G["CRpcPop"]
end

local m_time2007_timestamp = 400000000
-- 对C#中SyncWay的封装

SyncWay = 
{
    New = function()
        return CSSyncWay()
    end,

	Read = function(stream)
        local csObj = CSSyncWay()
        csObj:Serialize(stream)
        return csObj
    end,
    
	Write = function(csObj, stream)
        csObj:Serialize(stream)
    end
}

-- 装备数据长度48字节 read2
EquipItem = 
{
	Read = function(stream)
        local ret = {}
        ret.index_		= stream:ReadUShort()	-- m_wIndex
        ret.other_flags_= stream:ReadByte()	-- m_byOther 这才应该是byFlags吧
        ret.flags_		= BitAnd(ret.other_flags_, 0x3)	-- m_byFlags 是否绑定(1绑定 0非绑定)
		--ret.head_		= ReadArray(stream, stream.ReadByte, LYBGlobalConsts.ITEM_HEAD_LENGTH)	-- Head
		--ret.data_		= ReadArray(stream, stream.ReadByte, LYBGlobalConsts.ITEM_DATA_LENGTH)	-- Data
		 ret.head_		= ReadBuffer(stream, LYBGlobalConsts.ITEM_HEAD_LENGTH)	-- Head
		 ret.data_		= ReadBuffer(stream, LYBGlobalConsts.ITEM_DATA_LENGTH)	-- Data
        return ret
    end,
}

-- 背包物品, 长度2字节位置, 加48字节装备长度 read
PackageItem = 
{
    --背包改成[200][1], 忽略掉row_,200列
	Read = function(stream)
		local ret = {}
		local pt_pos_ = {}
        local item_stream = ReadBuffer(stream, LYBGlobalConsts.ITEM_ALL_LENGTH)

		pt_pos_.col_		= item_stream:ReadByte()	-- 列
		pt_pos_.row_		= item_stream:ReadByte()	-- 行
		ret.item_		    = EquipItem.Read(item_stream)
        ret.item_.pt_pos_   = pt_pos_	

        --读取道具的唯一ID(server_id)和创建时间(create_time)
        local head_stream = ret.item_.head_
        if head_stream ~= nil then
		    local byte_val      = head_stream:ReadByte()
		    local uint_val      = head_stream:ReadUInt()
		    local ushort_val    = head_stream:ReadUShort()		
		    --创建时间
		    ret.item_.create_time_ = BitRShift(uint_val, 7) * 30 + m_time2007_timestamp / 1000
		    --服务器ID(唯一ID, GUID)
		    ret.item_.server_id_ = tostring(byte_val) .. tostring(uint_val) .. tostring(ushort_val)            
        end

        item_stream.ReadPos = 0
        ret.item_.all_ = item_stream

        return ret
    end,
}

-- 摊位物品 read3
SaleItem = 
{
	Read = function(stream)
		local ret = {}
		local temp		= stream:ReadByte()
		ret.x_			= BitAnd(temp, 0xF)
		ret.y_			= BitRShift(temp, 4)
		ret.item_id_	= stream:ReadUShort()

		temp			= stream:ReadUInt()
        ret.sale_type_	= BitAnd(temp, 1)
        ret.price_		= BitRShift(temp, 1)

		ret.item_		= EquipItem.Read(stream)
        return ret
    end
}

LearnedSkill = 
{
	Read = function(stream)
        local ret = {}
        ret.type_id_	= stream:ReadUShort()	-- wTypeID
        ret.level_		= stream:ReadUShort()	-- wLevel
        return ret
    end
}

LearnedCitta = 
{
	Read = function(stream)
        local ret = {}
        ret.type_id_	= stream:ReadUShort()	-- wTypeID
        ret.level_		= stream:ReadUShort()	-- wLevel
        return ret
    end
}

ShortCutItem = 
{
	Read = function(stream)
        local ret = {}
        ret.type_	= stream:ReadByte()	    -- iItem 道具类型  (快捷项指向类型)
        ret.id_		= stream:ReadUShort()	-- ID    物品ID    (快捷项指向ID)
        return ret
    end
}

AutoFightData = 
{
	Read = function(stream)
        local ret = {}
		stream:Skip(9)
		local temp_byte = stream:ReadByte()
		ret.is_saved_ = (0 ~= BitAnd(temp_byte, 128))

		-- 是否保存了挂机设置
		if is_saved_ == false then return end

		stream:Skip(-10)
		temp_byte				= stream:ReadByte()
		ret.auto_cure_			= (0 ~= BitAnd(temp_byte, 1))
		ret.on_hp_percent_		= BitRShift(temp_byte, 1)

		ret.qualifying_skills_	= ReadArray(stream, stream.ReadByte, 8)

		temp_byte				= stream:ReadByte()
		ret.medicine_sort_		= BitAnd(BitRShift(temp_byte, 6), 1)
		ret.auto_accept_team_	= BitAnd(temp_byte, 32)
		ret.auto_fight_small_	= BitAnd(temp_byte, 16)
		ret.auto_fight_middle_	= BitAnd(temp_byte, 8)
		ret.auto_fight_big_		= BitAnd(temp_byte, 4)
		ret.auto_pick_			= BitAnd(temp_byte, 2)
		ret.auto_buy_medicine_	= (0 ~= BitAnd(temp_byte, 1))

		temp_byte				= stream:ReadByte()
		ret.auto_cure2_			= (0 ~= BitAnd(temp_byte, 1))
		ret.on_hp_percent2_		= BitRShift(temp_byte, 1)

		temp_byte				= stream:ReadByte()
		ret.auto_cure3_			= (0 ~= BitAnd(temp_byte, 1))
		ret.on_hp_percent3_		= BitRShift(temp_byte, 1)

		temp_byte				= stream:ReadByte()
		ret.medicine_sort2_		= BitAnd(temp_byte, 1)
		ret.medicine_sort3_		= BitAnd(temp_byte, 2)

		ret.auto_respawn_		= BitAnd(temp_byte, 4)
		ret.auto_horse_skill_	= BitAnd(temp_byte, 8)
		ret.auto_join_team_		= BitAnd(temp_byte, 16)
		ret.auto_buy_medicine2_ = (0 ~= BitAnd(temp_byte, 32))
		ret.auto_pick_equip_	= BitAnd(temp_byte, 64)
		ret.auto_respawn_yb_	= BitAnd(temp_byte, 128)

		temp_byte				= stream:ReadByte()
		ret.on_equip_color_		= BitAnd(temp_byte, 7)

		ret.shortcuts_			= ReadArray(stream, ShortCutItem.Read, LYBGlobalConsts.SHORTCUT_MAX)
		ret.fight_skill_idxes_	= ReadArray(stream, stream.ReadByte, 5)

		temp_byte				= stream:ReadByte()
		ret.auto_buy_medicine2_type_ = BitAnd(temp_byte, 0xF)
		if ret.auto_buy_medicine2_type_ > 2 then
			ret.auto_buy_medicine2_type_ = 0
		end
        return ret
    end
}


------------------------------------------------------------------------
-- 邮件相关数据
local temp_stream = LYBStreamOut()

EmailData =
{
	ReadContent = function(mail, bin_data)
        local ret = {}
        if mail.mail_type_ == 0 then
            temp_stream.Reset(bin_data)
            ret.title = CRpcPop(temp_stream)
            ret.content = CRpcPop(temp_stream)
            if mail.sender_ ~= nil then
                mail.sender_ = "g_Language.getContent(DMailPanelMediator_2)"    -- ?
            end
        elseif mail.mail_type_ == 2 or mail.mail_type_ == 1 then
            ret = table.loadblob(bin_data, 1)   -- GBK {title, content,}
        elseif mail.mail_type_ == 3 then
            ret = table.loadblob(bin_data, 0)   -- UTF8 {title, content,}
        end
        return ret
    end,

	Read = function(rpc_stream)
        local mail = {}
        mail.mail_id_ = CRpcPop(rpc_stream)             -- m_iMailId 在数据库中的ID
        mail.sender_ = CRpcPop(rpc_stream)              -- m_szSender 发送者

        if mail.sender_ ==nil  then
        mail.sender_="系统"
        end
         
        mail.mail_type_ = CRpcPop(rpc_stream)           -- m_iMsgType 邮件类型
        mail.mail_content_ = EmailData.ReadContent(mail, CRpcPop(rpc_stream))   -- content 接收内容
        mail.item_id_ = CRpcPop(rpc_stream)             -- itemid

        if mail.item_id_ == 0 and (mail.mail_type_ < 10 or mail.mail_type_ > 16) then
			mail.item_id_ = -1
        end

        mail.item_count_ = CRpcPop(rpc_stream)          -- itemnum
        mail.is_accept_item_ = CRpcPop(rpc_stream)      -- m_bIsAcceptItem 是否接收了附件
        mail.is_readed_ = CRpcPop(rpc_stream)           -- m_bIsRead 是否已读
        mail.time_str_ = CRpcPop(rpc_stream)            -- m_szTime 发送时间
        mail.delete_time_str_ = CRpcPop(rpc_stream)     -- m_deleteTime 到期时间
        mail.sale_item_id_ = CRpcPop(rpc_stream)        -- itemID 拍卖
        mail.sale_item_count_ = CRpcPop(rpc_stream)     -- itemNum 拍卖
        return mail
    end
}

-- 用于解析sys_flags_ (dwExtraState)
SysFlags =
{
	Read = function(rpc_stream)
        local temp = rpc_stream:ReadUInt()
        local ret = {}
        ret.is_gm_ = temp == 0x10                                   -- m_isGM
        ret.vip_level_ = BitAnd(temp, 0xF)                          -- vipLevel
        ret.is_helper_ = (BitAnd(BitRShift(temp, 4), 1) ~= 0)       -- m_isHelp 是否是指导员
        ret.faction_title_id_ = BitAnd(BitRShift(temp, 7), 7)       -- factionTitle 工会
        ret.create_title_id_ = BitAnd(BitRShift(temp, 14), 0x7f)    -- m_createTitle
        --[[
        -- strClientSL
        ret.platform_vip_ = BitAnd(BitRShift(temp, 21), 0xf)        -- m_platVip / m_PlatLv
        ret.platform_vip_year_ = BitAnd(BitRShift(temp, 25), 1)     -- m_platVipYear
        
        -- strClientDW
        ret.platform_vip_ = BitAnd(BitRShift(temp, 21), 0x7)        -- m_YYVip / m_PlatLv
        ret.platform_vip_year_ = BitAnd(BitRShift(temp, 25), 1)     -- m_YYVipYear
        ret.platform_vip_month_ = BitAnd(BitRShift(temp, 24), 1)    -- m_YYVipMonth
        
        -- strClientSQ || strClientST
        ret.platform_vip_ = BitAnd(BitRShift(temp, 21), 0x1f)       -- m_37WanVipLv / m_PlatLv

        -- strClientXL
        ret.platform_vip_ = BitRShift(temp, 21)                     -- m_37WanVipLv / m_PlatLv
        --]]
        
		ret.suit_effect_id_ = BitAnd(BitRShift(temp, 26), 0xf)      -- txEff 套装特效
        return ret
    end
}