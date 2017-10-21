AddressMsg={}



--更新成功
RpcService.update_address=function ( msg )
	table.print(msg,"update_address")
	MasterPlayer.Instance.address[1]=1
	local s={}

	for i=3,#msg do
		s[#s+1]=msg[i]
	end
	MasterPlayer.Instance.addressData[3][1]=s
	MessageCenter.SendMessage("get_address",msg)
end
--[[
    msg.name    姓名
    msg.sex     性别
    ,msg.province   省
    ,msg.city       市
    ,msg.addr,      详细地址
    msg.zip_code    邮编,
    msg.tel         电话号码
]]
--更新地址
function AddressMsg.UpdateAddress(name,sex,province,city,addr,zip,tel)
	local msg={}
	msg.name=name
	msg.sex=sex
	msg.province=province
	msg.city=city
	msg.addr=addr
	msg.zip_code=zip
	msg.tel=tel
    msg.ids = { 8, 1 }
    table.print(msg,"AddressMsg.UpdateAddress")
    ServerRpc.Call(8, 1)(msg)
end

--[[
玩家数据:address
	[1] =1 表示已经设置过详细地址
	
如果已经设置过详细地址,打开面板,请求详细地址
]]

function AddressMsg.GetAddress()
	local msg={}
    msg.ids = { 8, 2 }
    ServerRpc.Call(8, 2)(msg)
end

--更新成功 info = {RealName,Sex, CapitalId, CityId,FullAddress,Postcode,Tel}
RpcService.get_address=function ( msg )
	table.print(msg,"get_address")
	MasterPlayer.Instance.addressData=msg
	
	MessageCenter.SendMessage("get_address",msg)
end