-----------------------------------------------------------
-- 描述: 用于兼容生物对象, 作生物共有属性初始化

    --[[
        PI_SetPlayerIcon / CI_SetPlayerIcon (itype,idx,val,bSend,   object,id)
        PI_GetPlayerIcon / CI_GetPlayerIcon (itype,idx,             object,id)
        itype:
            0: 保存到sys_flags_(dwExtraState) 系统图标, 4字节, 查看SysFlags实现
            1: 保存到script_title_(dwScriptIcon) 脚本图标, 4字节, 表示4种称号
            2: 保存到temp_title_(dwScriptState) 临时图标, 4字节, 表示4种临时称号
            3: 保存到show_datas_(m_wShowID) 其它显示数据,short[16], 时装等
        idx:
            itype = 0, 功能索引, 可以尝试SysFlag实现中去分析, 或者直接到LYB对接讨论组里去咨询相关人员
            itype = 1, 忽略
            itype = 2, 忽略
            itype = 3, 在show_datas_中的下标索引, 也相当于功能索引
            
        object,id:
            LYB服务器中脚本与服务器交互时获取玩家对象的依据, lyb.cpp中的具体接口实现未包含这两个参数
    --]]
-----------------------------------------------------------
-- 上值定义 
local BitAnd = BitOp32.And
local BitOr = BitOp32.Or
local BitNot = BitOp32.Not
local BitLShift = BitOp32.LShift
local BitRShift = BitOp32.RShift

local ReadArray = MsgReadArray
local WriteArray = MsgWriteArray

local LYBGlobalConsts = LYBGlobalConsts
local SyncWay = SyncWay
local EquipItem = EquipItem
local LearnedSkill = LearnedSkill
local LearnedCitta = LearnedCitta
local AutoFightData = AutoFightData

-----------------------------------------------------------
-- 实现

local CreatureData = {}
function CreatureData.New()
    local ret = {}
    ret.hp_ = 100
    ret.mp_ = 100
    ret.max_hp_ = 100
    ret.max_mp_ = 100
    ret.move_speed_ = 5.0
    return ret
end

-----------------------------------------------------------
-- 网络下行数据类型
NpcData = 
{
    Read = function(stream)
        local ret = CreatureData.New()
        ret.type_id_    = stream:ReadUShort()
        ret.id_         = stream:ReadUInt()
        ret.control_id_ = stream:ReadUInt()
        
        local temp		= stream:ReadUShort()
        ret.head_id_	= BitAnd(temp, 0x7FFF)             -- 头像编号
        ret.is_collected_ = (BitRShift(temp, 15) ~= 0)  -- 是否被采集了

        ret.icon_id_	= stream:ReadByte()
        ret.dir_		= stream:ReadByte()
        ret.tile_x_		= stream:ReadUShort()
        ret.tile_y_		= stream:ReadUShort()

        
        -- 运行时数据
        return ret
    end
}

MonsterData = 
{
    Read = function(stream)
        local ret = CreatureData.New()
        ret.type_id_    = stream:ReadShort()	-- wTypeID
        ret.head_id_    = stream:ReadUShort()	-- iHeadImageNum
        ret.dir_		= stream:ReadByte()	-- byDir
        ret.level_		= stream:ReadByte()	-- iLevel
        ret.id_			= stream:ReadUInt()	-- dwGlobalID
        ret.control_id_ = stream:ReadUInt()	-- dwControlId
        ret.move_speed_ = stream:ReadFloat() * 10	-- m_fMoveSpeed
        ret.master_id_  = stream:ReadUInt()	-- dwMasterGID

        ret.halo_		= stream:ReadByte()	-- dwHalo
        ret.hp_percent_ = stream:ReadByte()	-- byHpPercent
        ret.change_type_= stream:ReadUShort()	-- byChangeType

        ret.buff_state_ = stream:ReadUInt()	-- dwBuffState
        ret.tile_area_  = stream:ReadByte()	-- wTileArea
        ret.side_		= stream:ReadUShort()	-- side
		stream:Skip(1)

        ret.script_extra_ = stream:ReadUInt()	-- dwScriptIcon
        ret.beat_flat_left_time_ = stream:ReadByte()	-- byBeatFlatLastTime
        ret.camp_		= stream:ReadByte()	-- camp
        ret.change_level_ = stream:ReadByte()	-- byChangeLevel
        ret.name_		= stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE)	-- sName
		ret.move_info_	= SyncWay.Read(stream)	-- ssp
		stream:Skip(2)
        ret.wHD			= stream:ReadInt()	-- wHD

        
        -- 运行时数据
        return ret
    end
}

OtherPlayerData = 
{
    -- EQUIP_TYPE_SYNC
	ETS_WEAPON = 0,
	ETS_CLOTH = 1,
	ETS_HAT = 2,
	ETS_WAIST = 3,
	ETS_SHOSE = 4,
	ETS_MANTLE = 5,
	ETS_WRIST = 6,
	ETS_FASHION_CLOTH = 7,
	ETS_MAX = 3,

    Read = function(stream)
        local ret = CreatureData.New()
		ret.check_id_		= stream:ReadUShort()	-- wCheckID
		ret.id_				= stream:ReadUInt()	-- dwGlobalID
		ret.sid_			= stream:ReadUInt()	-- dwSID
		ret.out_equip_id_	= stream:ReadUShort()	-- outEquipID

		-- ret.side_ = stream:ReadUShort()	-- side
		stream:Skip(2)

		ret.move_speed_		= stream:ReadFloat() * 10	-- fMoveSpeed
		local temp			= stream:ReadUShort()
        ret.sex_			= BitAnd(temp, 0x1)	-- bySex
        ret.dir_			= BitAnd(BitRShift(temp, 1), 7)	-- dir
        ret.level_			= BitAnd(BitRShift(temp, 4), 0xFF)	-- wLevel
        ret.job_			= BitAnd(BitRShift(temp, 12), 7)	-- bySchool
        ret.is_fly_			= BitRShift(temp, 15) ~= 0	-- isFly isfly

		ret.hp_percent_		= stream:ReadByte()	-- byHPPercent

		temp				= stream:ReadByte()
		ret.head_id_		= BitAnd(temp, 0x1F)	-- byHead
        
		ret.server_name_, ret.name_	= GetPlayerNames(stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE))	-- sName

		ret.spouse_name_	= stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE)	-- sSpouseName
		ret.tong_name_		= stream:ReadStringUtf8(LYBGlobalConsts.MAX_TONGNAMESIZE)	-- sTongName

		temp				= stream:ReadByte()
        ret.camp_			= BitAnd(BitRShift(temp, 4), 3)	-- m_camp
        ret.is_leader_		= BitAnd(BitRShift(temp, 6), 1)	-- isLeader 是否是队长
        ret.in_team_		= BitAnd(BitRShift(temp, 7), 1)	-- inTeam 是否在队伍中

        ret.move_info_		= SyncWay.Read(stream)	-- ssp

        ret.mount_id_		= stream:ReadShort() -- wMountID
        ret.buff_state_		= stream:ReadUInt() -- dwBuffState
        
        -- 是系统显示 vip 十大 这些 as3里面应该能看到具体的
        ret.sys_flags_      = SysFlags.Read(stream) -- dwExtraState  系统标志,查看SysFlags实现    PI_SetPlayerIcon(0, ...)

        -- 是脚本设置的称号，最多四个；后来好像在3里面又扩展了
        ret.script_title_ = stream:ReadUInt()  -- dwScriptIcon  脚本称号    PI_SetPlayerIcon(1, ...)

        -- 是脚本中一些活动过程临时用到的用于同步显示的
        ret.temp_title_ = stream:ReadUInt()    -- dwScriptState 临时称号    PI_SetPlayerIcon(2, ...)

        -- ret.item_ids_ = ReadArray(stream, stream.ReadUShort)	-- itemlist
		ret.equip_ids_		= ReadArray(stream, stream.ReadShort, OtherPlayerData.ETS_MAX)	-- wEquipIndex

        -- 其它显示用的数据
		ret.show_datas_		= ReadArray(stream, stream.ReadShort, 16)	-- m_wShowID PI_SetPlayerIcon(3, 数组索引, 值)

		stream:Skip(2)
        ret.server_group_id_= stream:ReadUInt() -- dwGroupID
        ret.wHD				= stream:ReadInt() -- wHD

        
        -- 运行时数据
        return ret
    end
}

LocalPlayerData = 
{
    Read = function(stream)
        local ret = CreatureData.New()
		ret.map_img_id_		= stream:ReadUShort()				-- wCurMapImgID 玩家当前所在地图图片编号
		ret.id_				= stream:ReadUInt()					-- dwPlayerGlobalID
		ret.region_id_		= stream:ReadUInt()					-- dwRegionGlobalID 场景gid
		ret.sid_			= stream:ReadUInt()					-- dwPlayerStaticID
		ret.server_id_		= stream:ReadUInt()					-- dwServerID

		stream:Skip(4)

		ret.max_exp_		= stream:ReadUInt64()				-- qwMaxExp 经验最大值
		ret.version_		= stream:ReadUInt()					-- dwVersion 版本号，用于二进制数据扩展！
		ret.leave_tick_		= stream:ReadUInt()					-- dwLeaveTime 玩家离线时的时间	(时刻)
		
		local temp			= stream:ReadByte()
		ret.head_id_		= BitAnd(temp, 0x1f)				-- byHead
		ret.job_			= BitAnd(BitRShift(temp, 5), 0xf)	-- bySchool

		-- bySex_OE_byMy_R4
        temp				= stream:ReadByte()
        ret.sex_			= BitAnd(temp, 0x1)--m_nSex
        ret.camp_			= BitAnd(BitRShift(temp, 1), 0x3)--camp
        ret.ai_type_		= BitAnd(BitRShift(temp, 3), 0x3)--aiType
        ret.out_equip_		= BitAnd(BitRShift(temp, 5), 0x7)--m_byShowOutEquip

		ret.level_			= stream:ReadUShort()					-- wlevel 玩家的等级65535上限
		ret.bag_lock_		= stream:ReadByte()					-- bagLock 背包锁
		ret.tag_open_count_	= stream:ReadByte()					-- openTags 背包开放的标签数
		ret.storage_tag_open_count_	= stream:ReadByte()					-- openStorageTags 仓库开放的标签数

		ret.pk_rule_		= stream:ReadByte()					-- pkRule PK规则
		ret.money_			= stream:ReadUInt()					-- dwMoney 游戏币
		ret.yuan_bao_		= stream:ReadUInt()					-- dwYuanBao 元宝
		ret.recharge_		= stream:ReadUInt()					-- recharge 累计充值
		ret.consumption_	= stream:ReadUInt()					-- consumption 累计消费
		
		ret.faction_id_		= stream:ReadUInt()					-- dwFactionId 帮派ID
		ret.hp_				= stream:ReadUInt()					-- dwCurHP 当前生命力
		ret.mp_				= stream:ReadUShort()				-- wCurMP当前怒气
		ret.map_id_			= stream:ReadUShort()				-- wCurMapID 玩家当前的地图ID
		ret.tile_x_			= stream:ReadUShort()				-- ptTile_x 玩家当前的区域坐标
		ret.tile_y_			= stream:ReadUShort()				-- ptTile_Y 玩家当前的区域坐标
		ret.dir_			= stream:ReadByte()					-- nDirection 当前方向
		
		stream:Skip(1)
		ret.vip_level_		= stream:ReadByte()					-- vipLevel vip等级
        
		ret.server_name_, ret.name_	= GetPlayerNames(stream:ReadStringGBK(LYBGlobalConsts.MAX_ROLENAMESIZE))	-- name

		ret.exp_			= stream:ReadUInt64()				-- qwCurExp
		ret.equip_items_	= ReadArray(stream, EquipItem.Read,LYBGlobalConsts.MAX_EQUIP_COUNT)					-- equip
		ret.skills_			= ReadArray(stream, LearnedSkill.Read,LYBGlobalConsts.MAX_SKILL_COUNT)					-- skills 目前已学习的武功  (0普通攻击12觉醒技13骑乘技)
		ret.cittas_			= ReadArray(stream, LearnedCitta.Read,LYBGlobalConsts.MAX_CITTA_COUNT)					-- citta 目前已学习的心法
		ret.talents_		= ReadArray(stream, stream.ReadByte,LYBGlobalConsts.MAX_TALNET_COUNT)					-- talnets 天赋

		ret.online_time_	= stream:ReadUShort()				-- onlineTimeCount
		ret.jewel_level_	= stream:ReadByte()					-- jewelLevel

		stream:Skip(17)
		ret.auto_fight_data_= AutoFightData.Read(stream)		-- autoFightConfig 64字节挂机设置

        -- 运行时数据
        --ret.move_info_		= SyncWay.New()	-- ssp
        return ret
    end
}

-- 查看玩家信息返回结构, 原名: PrivateChatObj
ShowPlayerData =
{
	ETS_MAX = 3,
    Read = function(stream)
		local ret = {}
		
        local temp			= stream:ReadByte()
        ret.level_			= BitAnd(temp, 0xff)	-- m_level
        ret.head_id_		= BitAnd(BitRShift(temp, 10), 0x1F)	-- m_head
        ret.job_			= BitAnd(BitRShift(temp, 8), 0x3)	-- m_school
        ret.sex_			= BitAnd(BitRShift(temp, 15), 0x1)	-- m_sex

		ret.equip_ids_		= ReadArray(stream, stream.ReadUShort, ShowPlayerData.ETS_MAX)	-- m_equipIds
		ret.vip_icon_id_	= stream:ReadByte()	-- byVIPIcon
        
		ret.server_name_, ret.name_	= GetPlayerNames(stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE))	-- m_serverName 名字

		ret.show_datas_		= ReadArray(stream, stream.ReadUShort, 16)	-- m_wShowID
		ret.camp_			= stream:ReadByte()	-- m_camp

		return ret
	end
}

-- 查看玩家装备信息返回结构, 使用消息 SAEquipmentListMsg
ShowPlayerEquipmentListData =
{
    Read = function(stream)
        local ret = {}
        ret.id_             = stream:ReadUInt()     -- dwCode
        ret.equip_items_	= ReadArray(stream, EquipItem.Read,LYBGlobalConsts.MAX_EQUIP_COUNT)	-- equip

        ret.max_hp_         = stream:ReadUInt()     -- dwMaxHP
        ret.sonic_atk_      = stream:ReadUInt()     -- Sonic_ATC
        ret.atk_            = stream:ReadUInt()     -- wAtc
        ret.def_            = stream:ReadUInt()     -- wDef
        ret.hit_            = stream:ReadUInt()     --wHit
        ret.duck_           = stream:ReadUInt()     --wDuck
        ret.crt_            = stream:ReadUInt()     --wCrit
        ret.resist_         = stream:ReadUInt()     --wResist
        ret.block_          = stream:ReadUInt()     --wBlock
        ret.hidden_atk_     = stream:ReadUInt()     --Hidden_ATC

        ret.sonic_res_      = stream:ReadUInt()     --Sonic_Res
        ret.hidden_res_     = stream:ReadUInt()     --Hidden_Res
        ret.move_speed_     = stream:ReadUInt()     --wMoveSpeed
        ret.sonic_ref_      = stream:ReadUInt()     --Sonic_Ref
        ret.hidden_ref_     = stream:ReadUInt()     --Hidden_Ref
        ret.atk_reduce_     = stream:ReadUInt()     --c_Reduce
        ret.def_reduce_     = stream:ReadUInt()     --d_Reduce
        ret.pvp_atk_        = stream:ReadUInt()     --Pvp_Act
        ret.pvp_def_        = stream:ReadUInt()     --Pvp_Def

        ret.skill_lve_      = stream:ReadUInt()     -- SSkill_Lve
        ret.hd_val_         = stream:ReadUInt()     -- HDVal
        ret.hd_js_          = stream:ReadUInt()     -- HDJs
        ret.hd_hf_          = stream:ReadUInt()     -- HDHf
        ret.hd_pd_          = stream:ReadUInt()     -- HDPd
        ret.hd_ct_          = stream:ReadUInt()     -- HDCt
        ret.extra_attr_     = stream:ReadUInt()     -- ExtraAtt

        ret.equip_ids_		= ReadArray(stream, stream.ReadUShort, ShowPlayerData.ETS_MAX)	-- wEquipIndex

        
        ret.suit_add_q_     = stream:ReadUInt()     -- wSuitAddQ 品质套装
        ret.suit_add_e_     = stream:ReadUInt()     -- wSuitAddE 强化套装
        ret.suit_add_j_     = stream:ReadUInt()     -- wSuitAddJ 玉石套装
        ret.tong_name_      = stream:ReadStringUtf8(LYBGlobalConsts.MAX_TONGNAMESIZE)    -- szTongName
        ret.spouse_name_    = stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE)    -- szSpouseName

        local temp          = stream:ReadUShort()
        ret.vip_level_      = BitAnd(temp, 0xF)     --vipLevel
        ret.job_            = BitAnd(BitRShift(temp, 4), 7)   --nSchool
        ret.request_type_   = BitAnd(BitRShift(temp, 7), 1)  --type 0场景查看别人装备 1排行榜查看别人装备
        ret.level_          = BitAnd(BitRShift(temp, 8), 255)  --wLevel

        ret.power_val_      = stream:ReadUInt()     -- powerVal
        ret.show_datas_		= ReadArray(stream, stream.ReadUShort, 9)	-- m_wShowID
        return ret
    end
}

HeroData =
{
    Read = function(stream)
		local ret = CreatureData.New()
		ret.type_id_		= stream:ReadUShort()	-- wTypeID      // 不为0代表副本
		ret.id_				= stream:ReadUInt()	-- dwGlobalID;		// 本次运行全局唯一标识符
		ret.master_id_		= stream:ReadUInt()	-- hostGID 主人GID
		ret.star_level_		= stream:ReadUInt()	-- startLv 书童星级
		ret.move_info_		= SyncWay.Read(stream)	-- ssp          // 校正趋向数据
		ret.dir_			= stream:ReadByte()	-- byDir 该NPC的方向
		ret.hp_percent_		= stream:ReadByte()	-- byHpPercent
		
		ret.move_speed_		= stream:ReadFloat() * 10	-- m_fMoveSpeed
		ret.buff_state_		= stream:ReadUInt()	-- dwBuffState
		ret.name_			= stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE)	-- sName
		ret.level_			= stream:ReadUShort()	-- ilevel 怪物等级 
		return ret
	end
}

LocalPlayerRefreshData =
{
    Read = function(stream)
		local ret = {}
		stream:Skip(2)
		ret.version_		= stream:ReadUInt()	-- m_dwVersion 版本号，用于二进制数据扩展！
		ret.consume_point_	= stream:ReadUInt()	-- m_dwConsumePoint 玩家消费积分
		ret.leave_time_		= stream:ReadUInt()	-- m_dwLeaveTime 玩家离线时的时间	
		ret.job_and_sex_	= stream:ReadByte()	-- SchoolAndSex

		stream:Skip(1)
		ret.level_			= stream:ReadByte()	-- m_wLevel 玩家的等级，255级上限 -->现改为WORD, 65535上限
		ret.exp_			= stream:ReadUInt64()	-- dwExp 玩家的当前经验值

		ret.money_			= stream:ReadUInt()	-- m_dwMoney 玩家当前所携带的金钱
		ret.yuan_bao_		= stream:ReadUInt()	-- m_dwYuanBao 玩家当前所携带的元宝
		ret.zeng_bao_		= stream:ReadUInt()	-- m_dwZenBao 玩家当前所携带的赠宝	
		ret.base_hp_		= stream:ReadUInt()	-- m_dwBaseHP 基本生命力
		ret.base_mp_		= stream:ReadUShort()	-- m_dwBaseMP 基本内力
		ret.base_sp_		= stream:ReadUShort()	-- m_wBaseSP 基本体力
		ret.max_hp_change_	= stream:ReadUShort()	-- wMaxHPOther 其他固定增加生命最大值
		ret.max_mp_change_	= stream:ReadUShort()	-- wMaxMPOther 其他固定增加内力最大值
		ret.max_sp_change_	= stream:ReadUShort()	-- wMaxSPOther 其他固定增加体力最大值
		stream:Skip(2)

		ret.hp_				= stream:ReadUInt()	-- m_dwCurHP 当前生命力
		ret.mp_				= stream:ReadUShort()	-- m_dwCurMP 当前内力
		ret.sp_				= stream:ReadUShort()	-- m_dwCurSP 当前体力
		stream:Skip(80 - 18 - 44)	-- ...

		ret.region_id_		= stream:ReadUShort()	-- m_wCurRegionID 玩家当前所在地图编号
		ret.practice_type_	= stream:ReadUShort()	-- m_wCurPracticeType 当前修炼的武功类型
		ret.tile_x_			= stream:ReadUShort()	-- m_wSegX 玩家当前的区域坐标
		ret.tile_y_			= stream:ReadUShort()	-- m_wSegY 玩家当前的区域坐标
		ret.dir_			= stream:ReadByte()	-- m_byDir 方向
        
		ret.server_name_, ret.name_	= GetPlayerNames(stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE))	-- m_szName 名字
		return ret
	end
}

RelationData = 
{
    Read = function(stream)
        local ret = {}
        ret.status_ = stream:ReadByte()            -- byStatus 0 不在线 ，1 在线，2 离线挂机 
		ret.job_ = stream:ReadByte();              -- bySchool 职业

		local temp_level_ = stream:ReadUShort();
        ret.level_ = temp_level_ % 1000            -- wLevel 等级
        ret.born_level_ = temp_level_ / 1000       -- wBornLevel

        ret.sid_ = stream:ReadUInt()               -- sid
        
		ret.server_name_, ret.name_	= GetPlayerNames(stream:ReadStringUtf8(LYBGlobalConsts.MAX_ROLENAMESIZE))	-- szName 名字

        local temp = stream:ReadByte()
		ret.relation_ = BitAnd(temp, 3)	        -- byRelation 0 好友, 1 密友, 2 结拜 ,3 夫妻
        ret.sex_ = BitAnd(BitRShift(temp, 2), 1)   -- sex

        if ret.relation_ > 0 then                  -- head
            ret.head_id_ = ret.relation_ - 1
        else
            ret.head_id_ = BitAnd(BitRShift(temp, 3), 0x1F)
        end

        ret.dear_value_ = stream:ReadUShort()      -- wDearValue 亲密度
		if ret.type_ == 0 or ret.type_ == 2 then
            ret.extend_ = stream:ReadUInt()        -- dwExtend
        end
        return ret
    end
}