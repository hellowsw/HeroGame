monster_config = { 
    [1001] = { 
        name = "游荡恶鬼",  
        image = "monster_01",
        skills = { 5001 },

        --动作前摇时长
        anim_delay_time = {
            ["Skill01"] = 0.12,
        },

        hp = 150,
        cd = 2,
    },
    [1002] = { 
        name = "熔岩恶魔",  
        image = "monster_02",
        skills = { 5002, 5001, 5002 },

        --动作前摇时长
        anim_delay_time = {
            ["Skill01"] = 0.12,
        },

        hp = 200,
        cd = 2,
    },
    [1003] = { 
        name = "骷髅王",  
        image = "monster_03",
        skills = { 5003, 5002, 5002, 5004 },

        --动作前摇时长
        anim_delay_time = {
            ["Skill01"] = 0.12,
        },
        hp = 600,
        cd = 2.2,
        boss = 1,
    },
}