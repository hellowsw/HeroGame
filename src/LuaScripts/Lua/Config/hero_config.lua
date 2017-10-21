racial_type = {
	human = 1,	--人类
	Mogo = 2,	--妖兽
	ancientKnight = 3,	--远古骑士
	spirit = 4,	--精灵
}


att_lib = {
	liliang = 1,	--力量
	zhili = 2,	--智力
	speed = 3,	--速度
	


}


school_lib = {	--职业类型
	Qiangxie = 1,	--枪械
	Jijia = 2,	--机甲
	Jianxia = 3,	--剑侠
	Doctor = 4,		--医术
	Mijing = 5,		--秘境

	KuangZ = 6,	--狂战士
	OrigFight = 7,	--原始战斗

	
	Black_knight = 9,	--黑骑士
	DragonKnight = 10,	--巨龙骑士
	DeadthKnight = 11,	--死灵骑士
	ShengsKnight = 12,	--圣光骑士

}




hero_config = {
	[1001] = {	--人类
		sub_school = {	--可转职方向
			[school_lib.Qiangxie] = 1,
			[school_lib.Jijia] = 1,
			[school_lib.Jianxia] = 1,
			[school_lib.Doctor] = 1,
		},

        --职业
	    vocation = school_lib.Jianxia,
	    
        --技能
        skills = { 
            1001, 1002, 1002, 1003, 1004,
        },

        --形象
        image = "hero_02",

        --动作前摇时长
        anim_delay_time = {
            ["Skill01"] = 0.4,
        },
	},
	
	[1003] = {	--人类
		sub_school = {	--可转职方向
			[school_lib.Qiangxie] = 1,
			[school_lib.Jijia] = 1,
			[school_lib.Jianxia] = 1,
			[school_lib.Doctor] = 1,
		},
	
	    --职业
	    vocation = school_lib.Doctor,
	    
        --技能
        skills = { 
            1011, 1011, 1022, 1023
        },

        --形象
        image = "hero_03",

        --动作前摇时长
        anim_delay_time = {
            ["Skill01"] = 0.2,
        },
	},
	
	[1011] = {	--妖兽
		sub_school = {	--可转职方向
			[school_lib.KuangZ] = 1,
			[school_lib.OrigFight] = 1,
		}
	
	
	},
	
	[1021] = {	--远古骑士
		sub_school = {	--可转职方向
			[school_lib.Black_knight] = 1,
			[school_lib.DragonKnight] = 1,
			[school_lib.DeadthKnight] = 1,
			[school_lib.ShengsKnight] = 1,
		}
	
	
	},



}



SchoolConfig = {
	[school_lib.Qiangxie] = {
		att = {
			[att_lib.liliang] = 10,
			[att_lib.zhili] = 10,
			[att_lib.speed] = 10,
		
		}
	
	}
}


