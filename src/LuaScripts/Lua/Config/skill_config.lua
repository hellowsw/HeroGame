--技能效果类型
effect_type = {
    --回复
    recover = 1,
    --伤害
    damage = 2,
 }

--技能施放类型
cast_type = { 
    --立即
    soon = 1,
    --远程
    far = 2,
}

--目标类型
target_type = {
    --单个
    single = 1,
    --全体
    all = 2,
}

skill_config = { 
    [1001] = { _name = "拔刀斩", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 5, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4" },
    [1002] = { _name = "拔刀斩·续", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 10, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4" },
    [1003] = { _name = "拔刀斩·收", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 15, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4" },
    [1004] = { _name = "拔刀斩·破空", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 20, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4", extreme = 1 },
    [1011] = { _name = "暗黑球", _effect_type = effect_type.damage, _cast_type = cast_type.far,  _power = 25, _speed = 10, _anim = "Skill01", explode_effect = "t_pw_6", be_cast_effect = "t_p_6"  },
    [1021] = { _name = "挥击", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 5, _anim = "Skill01", explode_effect = "t_pw_2" },
    [1022] = { _name = "恢复", _effect_type = effect_type.recover, _cast_type = cast_type.soon, _power = 40, _anim = "Skill01", explode_effect = "t_pw_7" },
    [1023] = { _name = "神圣", _effect_type = effect_type.recover, _cast_type = cast_type.soon, _target_type = target_type.all, _power = 30, _anim = "Skill01", explode_effect = "t_pw_7" },
    [5001] = { _name = "啃咬", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 8, _anim = "Skill01", explode_effect = "t_pw_3" },
    [5002] = { _name = "地狱火", _effect_type = effect_type.damage, _cast_type = cast_type.far,  _power = 25, _speed = 10, _anim = "Skill01", explode_effect = "t_pw_4", be_cast_effect = "t_p_4"  },
    [5003] = { _name = "旋风斩", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 30, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4" },
    [5004] = { _name = "小子吃我一刀，嘎嘎嘎", _effect_type = effect_type.damage, _cast_type = cast_type.soon, _power = 35, _target_type = target_type.all, _anim = "Skill01", explode_effect = "t_pw_4", extreme = 1 },
}