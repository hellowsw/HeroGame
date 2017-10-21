----------------------------------------------------------------------
-- 描述

----------------------------------------------------------------------
-- 上值定义
local SPMPS = SPMPS
local SCMPS = SCMPS
local NetMsg = NetMsg
local Logger = Logger
local log = UnityEngine.Debug.Log
local CmFromTilePos = CmFromTilePos
local GameWorld = nil
function AfterLoaded.UpValues()
    GameWorld = _G["GameWorld"]
end

----------------------------------------------------------------------
-- 实现

local Handlers = AddHandlerToMsgHandler(SPMPS.EPRO_REGION_MESSAGE)
local RefreshMsgHandlers = AddHandlerToMsgHandler(SPMPS.EPRO_REFRESH_MESSAGE)


----------------------------------------------------------------------
-- 刷新玩家数据, 只此一个消息, 未单独添加文件
local SARefreshPlayerMsg = NetMsg.SARefreshPlayerMsg
RefreshMsgHandlers[SCMPS.EPRO_REFRESH_PLAYER] = function(stream)
    local msg = SARefreshPlayerMsg:Read(stream)
end

----------------------------------------------------------------------
local SASynNpcMsg = NetMsg.SASynNpcMsg
Handlers[SCMPS.EPRO_SYN_NPC_INFO] = function(stream)
    local msg = SASynNpcMsg:Read(stream)
    local scene = GameWorld.cur_scene
	local data = msg.data_
    Scene.OnGotObjectInfo(data.id_, false)

    data.pos_ = CmFromTilePos(data.tile_x_, data.tile_y_)
    scene:OnNpcEnterAoi(data)
    -- Logger.Fmt("Npc %d EnterAOI Tile(%d,%d) => Pos(%d,%d)", data.id_, data.tile_x_,data.tile_y_, data.pos_.x_, data.pos_.y_)
end

local SASynMonsterMsg = NetMsg.SASynMonsterMsg
Handlers[SCMPS.EPRO_SYN_MONSTER_INFO] = function(stream)
    local msg = SASynMonsterMsg:Read(stream)

    local scene = GameWorld.cur_scene
	local data = msg.data_
    Scene.OnGotObjectInfo(data.id_, false)

    local move_info = data.move_info_
    data.pos_ = CmFromTilePos(move_info.tile_x_, move_info.tile_y_)
    scene:OnMonsterEnterAoi(data)
    -- Logger.Fmt("Monster %d EnterAOI Tile(%d,%d) => Pos(%d,%d)", data.id_, move_info.tile_x_,move_info.tile_y_, data.pos_.x_, data.pos_.y_)
end

-- 对SASynMonsterMsg的补充
local SASynMonsterMsgEx = NetMsg.SASynMonsterMsgEx
Handlers[SCMPS.EPRO_SYN_MONSTER_INFO_EX] = function(stream)
    local msg = SASynMonsterMsgEx:Read(stream)
end

--local SASynPlayerMsg = NetMsg.SASynPlayerMsg
--Handlers[SCMPS.EPRO_SYN_PLAYER_INFO] = function(stream)
--    local msg = SASynPlayerMsg:Read(stream)
--    local scene = GameWorld.cur_scene
--	local data = msg.data_
--    Scene.OnGotObjectInfo(data.id_, true)

--    local local_player = GameWorld.local_player
--    local move_info = data.move_info_

--    -- table.print(data, "SASynPlayerMsg")

--    if local_player ~= nil and local_player:IsMyselfByID(data.id_) == true then
--        local_player:OnGotSyncData(data)
--    else
--        data.pos_ = CmFromTilePos(move_info.tile_x_, move_info.tile_y_)
--        scene:OnPlayerEnterAoi(data)
--        -- Logger.Fmt("Player %d EnterAOI Tile(%d,%d) => Pos(%d,%d)", data.id_, move_info.tile_x_,move_info.tile_y_, data.pos_.x_, data.pos_.y_)
--    end
--end

-- 同步侍从数据
local SASynHerosMsg = NetMsg.SASynHerosMsg
Handlers[SCMPS.EPRO_SYN_HERO_DATA] = function(stream)
    local msg = SASynHerosMsg:Read(stream)
    local scene = GameWorld.cur_scene
	local data = msg.data_
    Scene.OnGotObjectInfo(data.id_, false)

    local move_info = data.move_info_
    data.pos_ = CmFromTilePos(move_info.tile_x_, move_info.tile_y_)
    scene:OnHeroEnterAoi(data)
end

-- 聊天框查看玩家信息返回
local SASynPlayerMsgEx = NetMsg.SASynPlayerMsgEx
Handlers[SCMPS.EPRO_SYN_PLAYER_INFO_EX] = function(stream)
    local msg = SASynPlayerMsgEx:Read(stream)
    table.print(msg, "SASynPlayerMsgEx 聊天框查看玩家信息返回")
end

-- 场景切换
local SASetRegionMsg = NetMsg.SASetRegionMsg
Handlers[SCMPS.EPRO_SET_REGION] = function(stream)
--    local msg = SASetRegionMsg:Read(stream)

--    -- msg => change_scene_msg
--    Logger.Fmt("SASetRegionMsg => dyn_map_id_: %d", msg.dyn_map_id_)
--    GameWorld.local_player_data.pos_ = CmFromTilePos(msg.to_tile_x_, msg.to_tile_y_)
	--GameWorld.ChangeScene(2)
--    GameWorld.cur_scene.pk_type_ = msg.pk_type_
--    GameWorld.cur_scene.pk_mode_ = msg.pk_mode_
--    GameWorld.cur_scene.option_ = msg.option_
end

-- 场景内坐骑上下马
--local SASetMounts = NetMsg.SASetMounts
--Handlers[SCMPS.EPRO_SYN_MOUNTS] = function(stream)
--    local msg = SASetMounts:Read(stream)

--    table.print(msg, "SASetMounts")

--    local obj = GameWorld.cur_scene:GetPlayer(msg.id_)
--    if obj == nil then return end

--    obj.data.mount_id_ = msg.mount_id_
--    if msg.mount_id_ < 0 then
--        obj:Ride(0)
--    else
--        obj:Ride(msg.mount_id_)
--    end
--end

-- 更新表现数据, 包含了称号数据
--local SASetExtraStateMsg = NetMsg.SASetExtraStateMsg
--Handlers[SCMPS.EPRO_SETEXTRASTATE] = function(stream)
--    local msg = SASetExtraStateMsg:Read(stream)

--    table.print(msg, "SASetExtraStateMsg")

--    local obj = GameWorld.cur_scene:GetPlayer(msg.id_)
--    if obj == nil then return end
--    obj.data.sys_flags_ = msg.sys_flags_
--    obj.data.script_title_ = msg.script_title_
--    obj.data.temp_title_ = msg.temp_title_

--    -- ... 表现逻辑
--end

-- 移动对象 移动类型 0从场景移除 1瞬移
--local SAMoveObjectMsg = NetMsg.SAMoveObjectMsg
--Handlers[SCMPS.EPRO_MOVE_OBJECT] = function(stream)
--    local msg = SAMoveObjectMsg:Read(stream)
--   -- table.print(msg, "SAMoveObjectMsg")
--    if msg.move_type_ == 0 then
--        GameWorld.cur_scene:RemoveObject(msg.id_)
--    else
--        local obj = GameWorld.cur_scene:GetCreature(msg.id_)
--        if obj == nil then return end
--        obj:SetPosOfTileXZ(msg.to_tile_x_, msg.to_tile_y_)
--    end
--end

-- 场景上某个对象需要出现的效果
--local SASetEffectMsg = NetMsg.SASetEffectMsg
--Handlers[SCMPS.EPRO_SET_EFFECT] = function(stream)
--    local msg = SASetEffectMsg:Read(stream)
--    local creature = GameWorld.cur_scene:GetCreature(msg.effect_player_id_)
--    if creature == nil then
--        log("SASetEffectMsg Creature 不存在，id = "..tostring(msg.effect_player_id_))
--        return
--    end

--    local EeffTypes = msg.EeffTypes
--    if msg.effect_type_ == EeffTypes.EEFF_LEVELUP then
--        log("场景中有玩家升级了，播放升级特效！")
--    elseif msg.effect_type_ == EeffTypes.EEFF_ONEEFFECT then
--        if msg.effect_id_ == 14 then
--            log("场景中有玩家复活了，播放复活特效！")
--            if msg.effect_player_id_ == GameWorld.local_player_data.id_ then
--                GameWorld.local_player:Relive()
--            end
--        end
--    elseif msg.effect_type_ == EeffTypes.EEFF_COSTUME then
--        log("场景中有玩家变身了，播放变身特效！")
--    end
--end
