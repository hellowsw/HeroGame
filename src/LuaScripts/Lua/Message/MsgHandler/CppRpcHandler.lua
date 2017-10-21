----------------------------------------------------------------------
-- 描述
----------------------------------------------------------------------
-- 包含
----------------------------------------------------------------------
-- 上值定义
local GameWorld = GameWorld
local LYBStreamOut = LYBStreamOut
local CRpcPop = CRpcPop
local Logger = Logger
local EmailData = EmailData
----------------------------------------------------------------------
-- 实现
CppRpcHandlers = { }

CppRpcType =
{
    PRPC_GETMAILLIST = 0,
    --  获取邮件列表
    PRPC_SENDMAIL = 1,
    --  发送邮件
    PRPC_SETREADMAIL = 2,
    --  设置已读
    PRPC_DELMAIL = 3,
    --  删除邮件
    PRPC_GETNEW = 4,
    --  获取新邮件
    PRPC_GETFIGHT = 5,
    --  获取战报
    PRPC_AUCTION = 6,
    --  拍卖
    PRPC_AUCTIONBID = 7,
    --  竞拍
    PRPC_AUCTIONCANCEL = 8,
    --  取消拍卖
    PRPC_AUCTIONLIST = 9,
    --  获得拍卖列表
    PRPC_TOPLIST = 10,
    --  获得排行榜列表
    PRPC_GARDENLOG = 11,
    --  获得果实操作日志
    PRPC_FACTIONJOINLIST = 12,
    --  获得帮会申请列表
    PRPC_BUYBACK = 13,
    --  道具回购
    PRPC_DACTIVESHOW = 14,
    -- 开服达人秀
    PRPC_OffLineData = 15,-- 玩家离线数据
}

----------------------------------------------------------------------
-- 获取邮件列表结果
CppRpcHandlers[CppRpcType.PRPC_GETMAILLIST] = function(rpc_stream)

    local size = CRpcPop(rpc_stream)
    if size ~= 109 then
        MessageCenter.SendMessage("mail_update", { })
        return
    end
    -- 无邮件

    local table_type = CRpcPop(rpc_stream)
    -- tableType not used

    local stream_data = CRpcPop(rpc_stream)
    if stream_data == nil or stream_data.Size <= 4 then
        return
    end
    -- 邮件总数
    local total = CRpcPop(rpc_stream)
    local page = CRpcPop(rpc_stream)
    local type = CRpcPop(rpc_stream)



    -- 新的RPC流
    rpc_stream:Reset(stream_data)
    local col = CRpcPop(rpc_stream)
    local row = CRpcPop(rpc_stream)


    look(rpc_stream)
    if row < 0 or row > 100 then
        Logger.Log("PRPC_GETMAILLIST出错,请截图:", row)
        return
    end
    local mail = nil
    local mails = { }

    mails.total=total
    mails.page=page
    mails.type=type
    for i = 1, row do
        mail = EmailData.Read(rpc_stream)
        table.insert(mails, mail)
        look(mail)
        if mail.mail_type_ == 16 then
            -- 竞技场
            --[[
			mail.sender_ = g_Language.getContent("DMailPanelMediator_2");
			mail.title_ = g_Language.getContent("DMailPanelMediator_21");
			mail.text_ = g_Language.getContent("DMailPanelMediator_22",mail.itemid);
			mail.item_id_ = g_luaProxy.topTitle[mail.itemid];
			mail.item_count_ = 1;
            --]]
        elseif mail.mail_type_ == 11 then
            -- 竞拍失败
            --[[
			local item_cfg = g_itemMgr.FindItemData( itemID );
			mail.text_ = g_Language.getContent("DMailPanelMediator_16",itemData.strName,itemNum );
            --]]
        elseif mail.mail_type_ == 12 then
            -- 拍卖成功
            --[[
			local item_cfg = g_itemMgr.FindItemData( itemID );
			mail.text_ = g_Language.getContent("DMailPanelMediator_18",itemData.strName,itemNum );
            --]]
        end
    end
    
    MessageCenter.SendMessage("mail_update", mails)

end

----------------------------------------------------------------------
-- 邮件发送结果 -1 没找到接收者或者发送者名字为空  -2 发送者和接收者为同一人  -3 接收者邮箱已满  0 发信成功
CppRpcHandlers[CppRpcType.PRPC_SENDMAIL] = function(rpc_stream)
    local result = CRpcPop(rpc_stream)
    Logger.Log("发送邮件返回: ", result)
end

----------------------------------------------------------------------
-- 此消息不处理
CppRpcHandlers[CppRpcType.PRPC_SETREADMAIL] = function(rpc_stream)
end

----------------------------------------------------------------------
-- 删除邮件结果
CppRpcHandlers[CppRpcType.PRPC_DELMAIL] = function(rpc_stream)
    local ids = { }
    local id = 0
    for i = 1, 4 do
        id = CRpcPop(rpc_stream)
        if id > 0 then table.insert(ids, id) end
    end

    local result = CRpcPop(rpc_stream)
    -- 0成功 1失败(前方为LYB AS 代码注释, 实际测试结果为: 1 成功, 0 失败)
    local type = CRpcPop(rpc_stream)
    -- ...
    Logger.Log("删除邮件返回: ", result, type)
    table.print(ids)
    MessageCenter.SendMessage("mail_delete_success",result)
end

----------------------------------------------------------------------
-- 检查新邮件结果
-- result: 0  无新邮件  1  收件箱有新邮件  2 战报有新邮件  3  收件箱和战报都有新邮件
CppRpcHandlers[CppRpcType.PRPC_GETNEW] = function(rpc_stream)

    local result = CRpcPop(rpc_stream)
    if result>0 then 
        MessageCenter.SendMessage("change_effect",{7,true})
    end
    -- ...
    Logger.Log("检查新邮件返回: ", result)
end

----------------------------------------------------------------------
CppRpcHandlers[CppRpcType.PRPC_GETFIGHT] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_AUCTION] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_AUCTIONBID] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_AUCTIONCANCEL] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_AUCTIONLIST] = function(rpc_stream)
end

-- 取排行列表返回
CppRpcHandlers[CppRpcType.PRPC_TOPLIST] = function(rpc_stream)


    print("##################PRPC_TOPLIST################")
    local size = CRpcPop(rpc_stream)
    if size == nil or size ~= 109 then
        Logger.Log("无排行数据！")
        MessageCenter.SendMessage("on_nil_rank")
        return
    end
    -- 无排行数据

    local table_type = CRpcPop(rpc_stream)
    -- tableType not used

    local stream_data = CRpcPop(rpc_stream)
    if stream_data == nil then
        return
    end

    local page_size = CRpcPop(rpc_stream)

    local data = { }
    data.page_ = CRpcPop(rpc_stream)
    data.type_ = CRpcPop(rpc_stream)
    data.totle_ = CRpcPop(rpc_stream)

    data.my_ranking_ = 0
    local my_ranking = CRpcPop(rpc_stream)
    if my_ranking ~= nil and my_ranking > 0 then
        data.my_ranking_ = my_ranking
    end

    local num = 8
    if data.type_ == 601 then num = 10 end

    if stream_data ~= nil and stream_data.Size > 4 then
        -- 新的RPC流
        rpc_stream:Reset(stream_data)
        local col = CRpcPop(rpc_stream)
        local row = CRpcPop(rpc_stream)
        if row < 0 or row > 100 then return end

        for i = 1, row do
            local info = { }
            for k = 1, num do
                table.insert(info,(CRpcPop(rpc_stream) or 0))
            end
            table.insert(data, info)
        end
    end


    --[[
    data数据结构注释
    {
        [1] =
        {
            [1] = 1000025376,         --roleid          玩家ID
            [2] = 1,                  --ranking         排名
            [3] = 1001000lh12,        --rolename        玩家名字
            [4] = 1,                  --school          职业
            [5] = 32,                 --rankingvalue    排行值（例：等级排行榜表示等级，战力排行榜表示战力）
            [6] = 0,                  --rankingid       用于存放当前排名的坐骑Id或者灵武Id之类的
            [7] = 0,                  --vip             VIP等级
            [8] = 1,                  --lastranking     上一次排名
        },
        type_ = 101,            --排行榜类型
        my_ranking_ = 1,        --自己排名
        page_ = 0,              --页码编号
        totle_ = 1,             --当前消息排行数据条数
    },
--]]
    table.print(data, "PRPC_TOPLIST")
    MasterPlayer.Instance.rankList = data
    MessageCenter.SendMessage("on_update_rank", data)

    -- RankPanel.RefreshRank(data)
end

CppRpcHandlers[CppRpcType.PRPC_GARDENLOG] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_FACTIONJOINLIST] = function(rpc_stream)
    local size = CRpcPop(rpc_stream)
    local data = { }
    if size == 109 then
        local table_type = CRpcPop(rpc_stream)
        -- tableType not used

        local stream_data = CRpcPop(rpc_stream)
        if stream_data == nil then
            return
        end

        if stream_data ~= nil and stream_data.Size > 4 then
            -- 新的RPC流
            rpc_stream:Reset(stream_data)
            local col = CRpcPop(rpc_stream)
            local row = CRpcPop(rpc_stream)
            if row < 0 or row > 100 then return end

            for i = 1, row do
                local info = { }
                info.id_ = CRpcPop(rpc_stream)
                -- 表中索引
                info.sid_ = CRpcPop(rpc_stream)
                -- sid
                info.name_ = CRpcPop(rpc_stream)
                -- 申请人名字
                info.vip_ = CRpcPop(rpc_stream)
                -- vip
                info.sex_ = CRpcPop(rpc_stream)
                -- 性别
                info.level_ = CRpcPop(rpc_stream)
                -- 等级
                info.job_ = CRpcPop(rpc_stream)
                -- 职业
                info.head_ = CRpcPop(rpc_stream)
                -- 头像编号
                table.insert(data, info)
            end
        end
    end

    -- 总数
    local count = 0
    local temp_count = CRpcPop(rpc_stream)
    local page = CRpcPop(rpc_stream)
    local ref = CRpcPop(rpc_stream)

    if page == 1 then
        count = temp_count
    end

    local component
    if GameWorld.local_player ~= nil then
        component = GameWorld.local_player.faction_component
    else
        component = LYBFactionComponent
    end

    component:UpdateJoinList(data, page, count, ref)
end

CppRpcHandlers[CppRpcType.PRPC_BUYBACK] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_DACTIVESHOW] = function(rpc_stream)
end

CppRpcHandlers[CppRpcType.PRPC_OffLineData] = function(rpc_stream)
end