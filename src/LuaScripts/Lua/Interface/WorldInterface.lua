local GameWorld = nil
local NetMsg = nil
local SQSynWayTrackMsg = nil

local printLog = UnityEngine.Debug.Log

-- 获取服务器时间
function GetServerTime()

        return GameServer._GetServerTime();
end


-- LYB中所有脚本数据都在这个表里 ...
function OnSyncTaskData(tbl)
    logBlue("OnSyncTaskData 同步脚本数据")
   
end
