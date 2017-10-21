Microdownloadpanel = BaseUI:New()

Microdownloadpanel.Instance = nil

local this = nil
function Microdownloadpanel:New()
    local obj = BaseUI:New("n=Microdownloadpanel,t=-p", FishDefine.UIType.GroupUI, true, true,true)
    setmetatable(obj, self)
    self.__index = self
    Microdownloadpanel.Instance = obj
    this = obj
    return obj
end
function Microdownloadpanel:Start()
	this.InitUI()
	UIEvent.SetButtonClick(this.CloseBtn,function (  )
		UIManager.Instance:CloseUI()
	end)

	table.print(MasterPlayer.Instance.cheerfull,"MasterPlayer.Instance.cheerfull")
	if MasterPlayer.Instance.cheerfull[3]==0 then

		if  MasterPlayer.Instance.cheerfull[2]==1 then
			--PC首次登陆
			UIEvent.SetButtonClick(this.DownloadBtn,function (  )
				CheerfulFishMsg.GetFirAwd()
			end)


		elseif MasterPlayer.Instance.cheerfull[2]>1 then
			
			UIEvent.SetButtonClick(this.DownloadBtn,function (  )
				CheerfulFishMsg.GetTaskAwd()
			end)
		end
	end

	this.Update()
	MessageCenter.AddListener("FinishTask",this.OnFinishTask)

end

function Microdownloadpanel.Update(  )

	if MasterPlayer.Instance.cheerfull[2]==1  then
		this.TaskDetail:SetActive(false)
		this.TaskAmount.gameObject:SetActive(false)
	else
		this.TaskDetail:SetActive(true)
		this.TaskAmount.gameObject:SetActive(true)
	end
	if MasterPlayer.Instance.cheerfull[2]>table.maxn(cheerfull_fish_conf[2]) then
		MasterPlayer.Instance.cheerfull[2]=table.maxn(cheerfull_fish_conf[2])
		MasterPlayer.Instance.cheerfull[3]=1
	end

	if  MasterPlayer.Instance.cheerfull[3]==0 then

		this.TaskOver:SetActive(false)

		if MasterPlayer.Instance.cheerfull[4]  then
			for k,v in pairs(MasterPlayer.Instance.cheerfull[4]) do
				
				local conf=cheerfull_fish_conf[2][MasterPlayer.Instance.cheerfull[2]]
				this.TaskAmount.text=v.."/"..conf[1][1][2]
				this.ShowAmount.text=conf[1][1][2]
				ByUtil.SetSliderValue(this.TaskProgress, v/conf[1][1][2])
				if true then
					return
				end
			end
		end
		if MasterPlayer.Instance.cheerfull[2]>1 then
			local conf=cheerfull_fish_conf[2][MasterPlayer.Instance.cheerfull[2]]
			this.TaskAmount.text="0/"..conf[1][1][2]
			this.ShowAmount.text=conf[1][1][2]
			ByUtil.SetSliderValue(this.TaskProgress, 0)
		end
	elseif MasterPlayer.Instance.cheerfull[2]>1 then
		local conf=cheerfull_fish_conf[2][MasterPlayer.Instance.cheerfull[2]]
		this.TaskAmount.text=conf[1][1][2].."/"..conf[1][1][2]
		this.ShowAmount.text=conf[1][1][2]
		ByUtil.SetSliderValue(this.TaskProgress, 1)
		this.TaskOver:SetActive(true)
		this.DownloadBtn.enabled=false
		ByUtil.SetGreyShader(this.DownloadBtn:GetComponent("Image"))
	else 
		
		ByUtil.SetSliderValue(this.TaskProgress, 1)
		this.TaskOver:SetActive(true)
		this.DownloadBtn.enabled=false
		ByUtil.SetGreyShader(this.DownloadBtn:GetComponent("Image"))
	end

	
end

function Microdownloadpanel.OnFinishTask( awd )
	local t, amount, id = UnpackItem(awd)
	local data = { { id = id, amount = amount } }
	UILingQu.ShowAndSet(data)
	this.Update()
end

function Microdownloadpanel:OnClose()
    Microdownloadpanel.Instance = nil
    MessageCenter.RemoveListener("FinishTask",this.OnFinishTask)
end
-- 以下注释为自动生成请勿修改
-- Start
function Microdownloadpanel.InitUI()
	this.CloseBtn= this.transform:FindChild("n=CloseBtn,c=Button"):GetComponent("Button")
	this.DownloadBtn= this.transform:FindChild("n=DownloadBtn,c=Button"):GetComponent("Button")
	this.TaskProgress= this.transform:FindChild("n=TaskProgress,c=Slider"):GetComponent("Slider")
	this.TaskAmount= this.transform:FindChild("n=TaskAmount,c=Text"):GetComponent("Text")
	this.TaskOver= this.transform:FindChild("n=TaskOver,c=GameObject").gameObject
	this.TaskDetail= this.transform:FindChild("n=TaskDetail,c=GameObject").gameObject
	this.ShowAmount= this.transform:FindChild("n=TaskDetail,c=GameObject/n=ShowAmount,c=Text"):GetComponent("Text")
end

-- End






return Microdownloadpanel

