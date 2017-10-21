CheerfulFishMsg={}
--[[
数据区:cheerfull
        [1] = { --各终端登陆奖励领取标示
            [1] = web
            [2] = pc
            [3] = android
            [4] = ios
         }

        [2] = 畅玩斗鱼第几天登陆(领取过奖励才算一天)
        [3] = 今天是否已经完成任务并领取过奖励
        [4] = { --今天任务数据
            [任务编号] = 已完成数量
            ...      
        }
    ]]
--领取首次登陆奖励
function CheerfulFishMsg.GetFirAwd()
	logBlue("asdasd11")
	local msg={}
	msg.ids = { 14,1}
    ServerRpc.Call(14,1)(msg)

end

RpcService.get_fir_awd=function ( msg )
	table.print(msg,"get_fir_awd")
	-- MasterPlayer.Instance.cheerfull={}
	-- MasterPlayer.Instance.cheerfull[2]=1
	
	if msg[1]==1 then
		createPrompt(232)
	elseif msg[1]==0 then
		MasterPlayer.Instance.cheerfull[3]=1
		MessageCenter.SendMessage("FinishTask",msg[2])
	end
end
RpcService.get_cheerful_task_awd=function ( msg )
	table.print(msg,"get_cheerful_task_awd")
	
	if msg[1]==1 then
		createPrompt(233)
	elseif msg[1]==2 then
		createPrompt(232)
	elseif msg[1]==0 then
		MasterPlayer.Instance.cheerfull[3]=1
		MessageCenter.SendMessage("FinishTask",msg[2])
	end
end

RpcService.cheerful_task_num=function ( msg )
	table.print(msg,"cheerful_task_num")
	if MasterPlayer.Instance.cheerfull[4] ==nil then
		MasterPlayer.Instance.cheerfull[4]={}
	end
	MasterPlayer.Instance.cheerfull[4][msg[1]]=msg[2]
	MessageCenter.SendMessage("SynTaskNum",msg)
end

RpcService.day_reset=function ( msg )
	table.print(msg,"msg_s2c_cheerful_fish.day_reset")
	MasterPlayer.Instance.cheerfull={}
	MasterPlayer.Instance.cheerfull[2]=msg[1]
	MasterPlayer.Instance.cheerfull[3]=0

end


--领取首次登陆奖励
function CheerfulFishMsg.GetTaskAwd()
	local msg={}
	msg.ids = { 14, 2 }
    ServerRpc.Call(14, 2)(msg)
end