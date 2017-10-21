UIBossawdpanel = BaseUI:New()

UIBossawdpanel.Instance = nil

local this = nil
function UIBossawdpanel:New(awd,room)
    local obj = BaseUI:New("n=UIBossawdpanel,t=-p", FishDefine.UIType.GroupUI, false, true,true)
    setmetatable(obj, self)
    self.__index = self
    UIBossawdpanel.Instance = obj
    this = obj
    this.awd=awd
    this.room=room
    return obj
end
function UIBossawdpanel:Start()
	math.randomseed(Time.time)
	this.InitUI()
	this.conf=fish_config.choose_boss_awd[this.room]

	local all=this.GetRandom(1,#this.conf,8,this.awd,this.conf)
	local awdIndex=this.GetRandom(1,8,1)
	local type,amount,id=nil,nil,nil
	local itemIcon,itemName=nil,nil
	local icon,text=nil,nil
	for i=1,#all do--修改图标 数量
		icon,text=this.GetItem(this.ItemGroup[i])
		if i==awdIndex[1] then
			type,amount,id=UnpackItem(this.awd)
		else
			type,amount,id=UnpackItem(this.conf[i])
		end
		itemIcon,itemName=GetItemInfo(type,id)
		icon.sprite=ResourceManager.LoadSpriteFromResources("item",itemIcon)

		text.text=getWan(amount)
	end

	logBlue("当前索引:"..awdIndex[1])
	this.rote.StartTime=2+awdIndex[1]*0.03	

	this.rote.EndPos=Vector3.New(0,0,(awdIndex[1]*46-15)*-1)
	UIEvent.SetButtonClick(this.btn,function (  )
		BossMsg.SendPlate()
		this.btn.interactable=false

		this.rote:Play(function (  )
			StartCoroutine(function (  )
				WaitForSeconds(0.5)
				BossMsg.SendPlate()
			end)
		end)
	end)
end

function UIBossawdpanel.GetRandom(min, max ,maxnum,filterItem,conf)
	maxnum=maxnum or 1
	min=min or 1
	max=max or 1
	logBlue(min..":"..max..":"..maxnum)
	local r={}
	local temp=nil
	local isAdd=false
	local index=0
	while true do 
		index=index+1
		if index>100000 then
			logE("boss奖励列表配置错误")
			for i=1,#r do
				r[i]=i
			end
			return r
		end
		temp=math.random(min,max)
		isAdd=true
		for i=1,#r do
			if r[i]==temp then
				isAdd=false
				break
			end
			if filterItem then
				local type,amount,id=UnpackItem(filterItem)
				local type2,amount2,id2=UnpackItem(conf[temp])
				if amount==amount2 and id==id2 then
					isAdd=false
				end
			end
		end

		if isAdd ==true then
			r[#r+1]=temp
		end

		if #r>=maxnum then
			return r
		end
	end

end

function UIBossawdpanel:OnClose()
    UIBossawdpanel.Instance = nil
end

function UIBossawdpanel.Open( awd,room)
	room=room or 1001
	if UIBossawdpanel.Instance==nil then
		UIManager.Instance:OpenUI(UIBossawdpanel:New(awd,room))
	end
end
--返回 图标 和 标签框
function UIBossawdpanel.GetItem( item )
	return item:FindChild("image"):GetComponent("Image"),item:FindChild("text"):GetComponent("Text")
end


-- 以下注释为自动生成请勿修改
-- Start

function UIBossawdpanel.InitUI()
	this.ItemGroup={}
	this.t1= this.transform:FindChild("n=t1,g=ItemGroup,i=1")
	this.ItemGroup[1]= this.t1
	this.t2= this.transform:FindChild("n=t2,g=ItemGroup,i=2")
	this.ItemGroup[2]= this.t2
	this.t3= this.transform:FindChild("n=t3,g=ItemGroup,i=3")
	this.ItemGroup[3]= this.t3
	this.t4= this.transform:FindChild("n=t4,g=ItemGroup,i=4")
	this.ItemGroup[4]= this.t4
	this.t5= this.transform:FindChild("n=t5,g=ItemGroup,i=5")
	this.ItemGroup[5]= this.t5
	this.t6= this.transform:FindChild("n=t6,g=ItemGroup,i=6")
	this.ItemGroup[6]= this.t6
	this.t7= this.transform:FindChild("n=t7,g=ItemGroup,i=7")
	this.ItemGroup[7]= this.t7
	this.t8= this.transform:FindChild("n=t8,g=ItemGroup,i=8")
	this.ItemGroup[8]= this.t8
	this.rote= this.transform:FindChild("n=rote,c=HuoyueAnimation"):GetComponent("HuoyueAnimation")
	this.btn= this.transform:FindChild("n=btn,c=Button"):GetComponent("Button")
end
-- End






return UIBossawdpanel

