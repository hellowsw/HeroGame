
namespace Network.DataDefs.NetMsgDef
{
    public class SPMPS
    {
        public const byte EPRO_MOVE_MESSAGE = 32;  // 移动消息
        public const byte EPRO_CHAT_MESSAGE = 33;  // 对话消息
        public const byte EPRO_FIGHT_MESSAGE = 34;  // 战斗消息
        public const byte EPRO_SCRIPT_MESSAGE = 35;  // 脚本消息
        public const byte EPRO_REGION_MESSAGE = 36;  // 场景消息
        public const byte EPRO_ITEM_MESSAGE = 37;  // 道具消息
        public const byte EPRO_SYSTEM_MESSAGE = 38;  // 系统管理消息
        public const byte EPRO_UPGRADE_MESSAGE = 39;  // 升级消息（玩家属性变化）
        public const byte EPRO_TEAM_MESSAGE = 40;  // 组队相关的消息  （包括聊天的消息） 
        public const byte EPRO_TONG_MESSAGE = 41;  // 帮会相关的消息  （包括聊天的消息）
        public const byte EPRO_MENU_MESSAGE = 42;  // 菜单选择操作
                                                     //		public const byte EPRO_NAMECARD_BASE          = 43;  // 名片
        public const byte EPRO_RELATION_MESSAGE = 43;  // 好友= NEXT;黑名单等等
                                                         //		public const byte EPRO_SPORT_MESSAGE          = 45;  // 运动、竞技
                                                         //		public const byte EPRO_BUILDING_MESSAGE       = 46;  // 动态建筑相关消息

        //		public const byte ERPO_MOUNTS_MESSAGE		   = 48;  // 坐骑相关消息
        //		public const byte ERPO_MYTHAGO_MESSAGE		   = 51;	// 元神消息
        /** 随从 */
        public const byte EPRO_HEROS_MESSAGE = 52;  // 随从消息
        public const byte EPRO_CHKREBIND_MESSAGE = 100; // 区域服务器返回的SACheckRebindMsg


        public const byte EPRO_COLLECT_MESSAGE = 0xd0;  // 数据采集功能类消息
        public const byte EPRO_GAMEMNG_MESSAGE = 0xe0;  // GM指令
        public const byte EPRO_GMM_MESSAGE = 0xe1;  // GM模块指令（增补）

        // 底层消息段
        public const byte EPRO_REFRESH_MESSAGE = 0xf0;  // 数据刷新消息
        public const byte EPRO_SERVER_CONTROL = 0xf1;  // 服务器控制消息
        public const byte EPRO_REBIND_MESSAGE = 0xf2;  // 连接重定向消息
        public const byte EPRO_DATABASE_MESSAGE = 0xf3;  // 数据库相关消息
        public const byte EPRO_ACCOUNT_MESSAGE = 0xf4;  // 帐号检测相关消息
        public const byte EPRO_ORB_MESSAGE = 0xf5;  // 跨区域服务器相关消息
        public const byte EPRO_DATATRANS_MESSAGE = 0xf6;  // 数据传送相关消息
        public const byte EPRO_DBOP_MESSAGE = 0xf7;  // 数据库操作用消息
        public const byte EPRO_POINTMODIFY_MESSAGE = 0xf8;  // 点数交易操作相关消息
        public const byte EPRO_MAIL_MESSAGE = 0xF9;   //系统邮件
        public const byte EPRO_EMAIL_MESSAGE = 0xFA;  //玩家邮件
                                                        //		public const byte EPRO_MAIL_MESSAGE           = 0xf9;  // 留言系统相关消息
                                                        //		public const byte EPRO_PHONE_MESSAGE          = 0xfa;  // 电话系统相关消息
        public const byte EPRO_UNION_MESSAGE = 0xfb;  // 结义相关消息
    }

    public class SCMPS
    {
        /**Login--------------------------------------------- */
        public const byte EPRO_LOGIN = 0; // 登陆
        public const byte EPRO_CREATE_CHARACTER = 1; // 创建角色
        public const byte EPRO_DEL_CHARACTER = 2; // 删除角色
        public const byte EPRO_SELECT_CHARACTER = 3; // 选择角色
        public const byte EPRO_LOGOUT = 4; // 退出
        public const byte EPRO_CHARACTER_LIST_INFO = 5; // 设置客户端角色列表
        public const byte EPRO_CHARACTER_DATA_INFO = 6; // 设置客户端主角资料
        public const byte EPRO_LOGIN_NOCHECKACCOUNT = 7; // 不到帐号服务器检查（如从特殊服务器返回原服务器）
        public const byte EPRO_PREVENT_INDULGE = 9; // 防沉迷
        public const byte EPRO_CLIENT_REQUEST_RPCOP = 11; // 客户端请求RPC操作 访问数据库
        public const byte EPRO_STORY_STEP = 12; // 单机保存数据

        /**Refresh------------------------------------------- */
        public const byte EPRO_REBIND_REGION_SERVER = 0; // 场景重定向消息
        public const byte EPRO_CHECK_REBIND = 1; // 重定向认证消息
        public const byte EPRO_REBIND_LOGIN_SERVER = 2; // 服务器之间无缝连接（不退出客户端直接连接）

        /**Move---------------------------------------------- */
        public const byte EPRO_SYN_WAYTRACK = 0; // 同步移动路径信息
        public const byte EPRO_SYN_POSITION = 1; // 同步位置消息
        public const byte EPRO_SET_ZAZEN = 2; // 打坐/解除消息
                                                //		public const byte EPRO_TEST_POSITION			= 3; // 测试用
        public const byte EPRO_SYN_WAYJUMP = 3; // 跳跃消息	
                                                  //		public const byte EPRO_TITLECHANGED				= 5; // 称号改变
                                                  //		public const byte EPRO_NAMECHANGED				= 6; // 名字改变	
                                                  //		public const byte EPRO_UP_SPEED					= 7; // 提升速度
                                                  //		public const byte EPRO_NOTIFY_MOVE				= 8;
        public const byte EPRO_LOCK_PLAYER = 4;   //锁定玩家
                                                    //		public const byte EPRO_WINK_MOVE				= 10;
                                                    //		public const byte EPRO_SET_CG_MSG				= 11;	// 打坐/解除消息
        public const byte EPRO_SYN_DIRECTION = 5; // 同步方向
        public const byte EPRO_SET_FLY = 6;   // 同步飞行
                                                //		public const byte EPRO_SET_ACTION				= 13;	// 同步动作
                                                //		public const byte EPRO_RUN_ACTIONTEXT			= 14;	// 执行动作脚本
                                                //		public const byte EPRO_STAND_AUTOWALK			= 15;	// 停止自动寻路

        /**Region---------------------------------------------- */
        public const byte EPRO_MOVE_OBJECT = 0;   // 删除场景上的对象
        public const byte EPRO_DEL_OBJECT = 0;    // 删除场景上的对象
        public const byte EPRO_SYN_PLAYER_INFO = 1;   // 同步玩家信息
        public const byte EPRO_SYN_NPC_INFO = 2;  // 同步NPC信息
        public const byte EPRO_SYN_MONSTER_INFO = 3;  // 同步怪物信息
        public const byte EPRO_SET_EFFECT = 4;    // 场景上某个对象需要出现的效果(声光效果)
        public const byte EPRO_OBJECT_INFO = 5;   // 请求获取场景上某个对象的数据
        public const byte EPRO_SET_REGION = 6;    // 通知客户端设置场景
        public const byte EPRO_SYN_OBJECT = 7;    // 通知场景上出现了某个对象
                                                    //		public const byte EPRO_SYN_SALENAME				= 8;	// 摆摊信息
        public const byte EPRO_SETEXTRASTATE = 8; // 设置附加状态

        //		public const byte EPRO_QUERY_SIMPLAYER				= 13;	//查看玩家信息
        public const byte EPRO_SYN_PLAYER_IS_TEAM_INFO = 10;  //同步玩家是否是队长	
        public const byte EPRO_SYN_AREAGIDS = 11; //新的区块同步逻辑（当玩家跨区块时，将会收到当前视野内的所有对象GID）
        public const byte EPRO_SYN_MUTATE_POS = 12;   //塔防副本同步妲已血量
        public const byte EPRO_SYN_MONSTER_INFO_EX = 13;  //同步怪帮会名字
        public const byte EPRO_SYN_PLAYER_INFO_EX = 14;   //聊天查看面板
                                                            //		public const byte EPRO_SYN_PTSTATE	= 25;	//广播更改阴阳八卦状态的消息
        public const byte EPRO_SYN_HERO_DATA = 15;    //上下随从消息
        public const byte EPRO_SYN_MOUNTS = 16;   //上下坐骑消息
        public const byte EPRO_SEND_PLAYLIST = 17;    //庄园玩家列表
                                                        //...



        /**upgrade------------------------------------------- */
        public const byte EPRO_RES_CHANGE = 0;    // 资源（当前生命、内力、体力）属性（包括定时变化）
                                                    //		public const byte EPRO_SET_POINT				= 1; // 分配点数
                                                    //		public const byte EPRO_PASS_VENAPOINT			= 2; // 穴道消息
                                                    //		public const byte EPRO_UPDATE_NPCLIST			= 3;	// 更新NPC消息
                                                    //		public const byte EPRO_UPDATE_STATE_PANEL_DATA	= 4;	// 更新客户端的状态面板的数据 EPRO_UPDATE_ATTACK_DEFENSE_VALUE
        public const byte EPRO_BUFF_UPDATE = 1;           // 更新buff
                                                            //		public const byte EPRO_SYN_VENA_DATA			= 6;
                                                            //		public const byte EPRO_PLAYER_ONE_ATTRI			= 7;
        public const byte EPRO_PLAYER_ATTRI = 2;
        public const byte EPRO_SKILL_CHANGE = 3;
        public const byte EPRO_TELERGY_CHANGE = 4;
        //		public const byte EPRO_FLY_CHANGE				= 11;
        //		public const byte EPRO_USE_LQ_TO_SKILL			= 12;  //灌注灵气到技能
        public const byte EPRO_BUFF_SPLITMOVE = 5;  //技能改变被攻击者坐标
                                                      //		public const byte EPRO_DRUG_INFO_SHOW			= 14;  //中毒效果
                                                      //		public const byte EPRO_SYN_CURPUSHEDLQ			= 15;	 // 更新某线脉存储的灵气值
                                                      //		public const byte EPRO_MYTHAGO_SKILL_CHANGE		= 16;	//元神技能更新
        public const byte EPRO_BUFF_DAMAGE_INFO_SHOW = 6; //buff伤害更新

        public const byte EPRO_MOVESPEED_CHANGE = 7; // 移动速度的改变
                                                       //		public const byte EPRO_ATKSPEED_CHANGE			= 20; // 攻击速度的改变
        public const byte EPRO_EXP_CHANGE = 8; // 攻击速度的改变
        public const byte EPRO_BUFF_PRO_SYN_TO_SELECT = 9; // 更新选中玩家数据
                                                             //		public const byte EPRO_XVALUE_CHANGE			= 22; // 威望值的变化，因为这个数据是整个场景的玩家都会看到的，所以单独处理
        public const byte EPRO_BUFF_EFFECT_SYN_TO_ALL = 10; // BUFF特效同步消息给所有人

        public const byte EPRO_MULTIPLICITY_EXP = 11; // 双倍经验
        public const byte EPRO_SET_TASK_MASK = 12; // 成就信息
        public const byte EPRO_SET_TONG_NAME = 13; // 设置怪物帮派名字
                                                     //		public const byte EPRO_SYN_TDATA				= 45; //成就累加数据
                                                     //		public const byte EPRO_SYN_FACTION_VAL				= 46; //荣誉积分
                                                     //		public const byte EPRO_UPDATE_LQS_TIME				= 47; //荣誉积分
                                                     //		public const byte EPRO_CLEAR_LQS_TIME				= 48; //荣誉积分
        public const byte EPRO_UPDATE_LEVELUP = 14; //自己升级
        public const byte EPRO_UPDATE_GIFT = 15; //更新天赋技能
        public const byte EPRO_UPDATE_EQUIPCOUNT = 16;    //更新自己的统计套装

        /**item---------------------------------------------- */
        public const byte EPRO_DROP_ITEM = 0;  // 丢掉物品
        public const byte EPRO_PICKUP_ITEM = 1;  // 拾取
        public const byte EPRO_ADDPACKAGE_ITEM = 2;  // 物品栏中加物品
        public const byte EPRO_ADDWAREHOUSE_ITEM = 3;  // 仓库中的物品
        public const byte EPRO_ADDGROUND_ITEM = 4;  // 地面加物品
        public const byte EPRO_PRE_USE_ITEM = 5;  // 物品预处理
        public const byte EPRO_USE_ITEM = 6;  // 使用物品
        public const byte EPRO_DELPACKAGE_ITEM = 7;  // 删除物品栏中道具
        public const byte EPRO_DELGROUND_ITEM = 8;  // 删除地表物品
        public const byte EPRO_MOVE_ITEM = 9;  // 移动物品在物品栏的位置
        public const byte EPRO_EQUIP_ITEM = 10; // 装备外表可见的物品（群发）
                                                  //		public const byte EPRO_EXCHANGE_MESSAGE    		= 12;  // 道具交换
                                                  //		public const byte EPRO_BUY_MESSAGE				= 13;	// 道具交易
        public const byte RPRO_EQUIP_INFO = 11;                           // 装备孔的信息
        public const byte RPRO_WAREHOUSE_BASE = 12;  //仓库基类
        public const byte RPRO_EQUIPMENTINFO = 13;  //发送一个人的装备数据
                                                      //		public const byte RPRO_SALEITEM_MESSAGE				= 17;  //出售道具相关
        public const byte EPRO_AUTOUSE_ITEM = 14;  //自动吃药
        public const byte EPRO_SPLIT_ITEM = 15; //拆分
        public const byte EPRO_NEATEN = 16; //整理背包
        public const byte EPRO_UPDATE_TEMPITEM = 17; //更新玩家身上的临时物品
                                                       //		public const byte EPRO_AUTOFIGHT				= 18;  //自动战斗
                                                       //		public const byte EPRO_EQUIP_WEAR_CHANGE= 29;  //耐久度同步消息
                                                       //		public const byte EPRO_NOTICE_MONEY= 32;  //通知钱的状态(已达到最大)
                                                       //		public const byte EPRO_EQUIP_REPAIR				= 35; //装备修理
        public const byte EPRO_USE_ITEM_CONFIRM = 18;  //使用任务物品
        public const byte ERPO_USE_RELIVEITEM_COUNT = 20;  //原地复活物品
        public const byte EPRO_ADD_PACK_OPENFLAGS = 19; //使用扩展背包标签物品
                                                          //		public const byte EPRO_UPDATE_POINTS				= 39; //增加一个所有消耗货币类的同步消息
                                                          //		public const byte EPRO_BUY_DIRECT				= 40; //快捷购买物品
                                                          //		public const byte EPRO_ITEM_SELECTED				= 41; //锁定吃的药
        public const byte ERPO_ADD_TEMPITEM_INFO = 21; //获得临时物品提示
                                                         //		public const byte EPRO_SUITADD_CHANGE= 43;	//套装属性
        public const byte EPRO_TEMPITEM_BROADCAST = 22;   //据点战捡到临时物品
                                                            //		public const byte EPRO_SEND_VIPLEVEL= 23;	//vip等级
        public const byte EPRO_SEND_JEWELLEVEL = 23;  //玉石纯化
        public const byte EPRO_USEINTERVAL_EFFECT = 24;  //播放使用间隔光效
        public const byte EPRO_SYN_MONEY = 25;  // 更新金钱
                                                  //		public const byte EPRO_REFRESH_POINT= 46;	//刷新当前银子
                                                  //		public const byte EPRO_HERO_DATA= 47;	//查看随从
                                                  //		public const byte EPRO_MOUNT_DATA= 48;	//查看坐骑

        /**--------------------------仓库消息三级消息----------------------------------*/
        public const byte RPRO_OPEN_WAREHOUSE = 0; // 请求激活一个仓库（返回仓库中的物品）
        public const byte RPRO_CLOSE_WAREHOUSE = 1; // 请求关闭一个仓库（通知服务器仓库使用完毕）
        public const byte RPRO_ITEM_MOVEIN = 2;// 将一个道具（或者金钱）从身上移动到仓库
        public const byte RPRO_ITEM_MOVEOUT = 3; // 将一个道具（或者金钱）从仓库移动到身上
        public const byte RPRO_ITEM_MOVESELF = 4;// 将一个道具在仓库中移动
        public const byte RPRO_ITEM_LOCKIT = 5;
        public const byte RPRO_ITEM_ARRANGE = 6;


        /** 摊位三级消息 */
        public const byte RPRO_MOVETO_ITEM = 0;  //移动买卖道具
        public const byte EPRO_ADDSALE_ITEM = 1;  //添加道具到买卖框
        public const byte EPRO_DELSALE_ITEM = 2;  //删除买卖框道具
        public const byte EPRO_SEESALE_ITEM = 3;  //察看买卖框
        public const byte EPRO_BUYSALE_ITEM = 4;  //交易买卖框的道具
        public const byte EPRO_CLOSESALE_ITEM = 5;  //关闭查看窗口
        public const byte EPRO_UPDATESALE_ITEM = 6;  //刷新
        public const byte EPRO_SEND_SALEINFO = 7;  //修改摊位名字
        public const byte EPRO_REQUEST_STALLLIST = 9;  //获取摊位列表
        public const byte EPRO_STALLLIST_NEXTPAGE = 10;  //下一页
        public const byte EPRO_SHOW_STALLLIST = 11;  //摊位列表每页数据(摊位)
        public const byte EPRO_SHOW_GOODSLIST = 12;  //摊位列表每页数据(物品)


        /**Script---------------------------------------------- */
        public const byte EPRO_CLICK_OBJECT = 0;
        //		public const byte EPRO_CLICK_MENU				= 1;
        public const byte EPRO_BACK_MSG = 1;  // 服务器一些返回消息
                                                //		public const byte EPRO_SCORE_LIST				= 2; //排行榜(接收)
                                                //		public const byte EPRO_SCORE_LISTSEND				= 3; //排行榜(发送)
        public const byte EPRO_LUACUSTOM_MSG = 2; // 脚本定制消息
                                                    //		public const byte EPRO_LUATIPS_MSG			= 17; // 脚本提示消息
                                                    //		public const byte EPRO_OTHEREQUIPMENTNAME			= 19; // 通过名字查看装备  42 2
        public const byte EPRO_SYNCTASKDATA = 3; // 同步完整脚本数据块消息
                                                   //		public const byte EPRO_CLICK_FUNCTION_NPC			= 26; // 玩家洗点
        public const byte EPRO_PROCESSACTION = 4; // 客户端解析一段字符串
                                                    //		public const byte EPRO_RELIVE_OP			= 37; // 复活
                                                    //		public const byte EPRO_SEND_SCOREEX			= 38; // 随从，坐骑排行榜

        /**fight战斗消息---------------------------------------------- */
        //		public const byte EPRO_FIGHT_OBJECT= 0;		// 攻击场景上的对象
        //		public const byte EPRO_WOUND_OBJECT= 1;     // 场景上的对象被攻击
        //		public const byte EPRO_FIGHT_POSITION= 2;	// 攻击场景上的某个地方
        //		public const byte EPRO_SET_CURSKILL= 3;		// 设置当前使用的武功
        //		public const byte EPRO_SET_EXTRASTATE= 4;    // 设置附加状态(点穴、眩晕)
        //		public const byte EPRO_RET_VENATIONSTATE= 5; // 返回经脉状态
        //		public const byte EPRO_CURE_VENATION= 6;     // 请求治疗经脉
        //		public const byte EPRO_CUREOK_VENATION= 7;   // 确认经脉治疗
        //		public const byte EPRO_DELETE_TELERGY= 8;    // 删除装备的心法
        //		public const byte EPRO_KILLED_COUNT= 9;      // 显示杀阵计数
        //		public const byte EPRO_PASSVENA_EFFECT= 10;   // 暴穴效果广播
        //		public const byte EPRO_ONGOAT_MONSTERCOUNT= 11;// 替身打怪计数
        //		public const byte EPRO_REFRESH_ONUP= 12;       // 离线后上线数据更新
        public const byte EPRO_WOUND_OBJECTEX = 0;     // 场景上的对象被攻击			
                                                         //		public const byte EPRO_UPDATE_CUREQUIPDURANCE= 14;// 更新耐久
                                                         //		public const byte EPRO_UPDATE_DECDURTIME= 15;  // 更新不掉耐久时间 
        public const byte EPRO_SWITCH_TO_SKILL_ATTACK_STATE = 1;  // 通知客户端切换到远程技能攻击状态下
                                                                    //		public const byte EPRO_SWITCH_TO_PREPARE_SKILL_ATTACK_STATE= 17;
                                                                    //		public const byte EPRO_CANCEL_SKILL_ATTACK_PROCESS_BAR= 18;
        public const byte EPRO_NOTIFY_START_COLD_TIMER = 2;   //技能CD时间
                                                                //		public const byte EPRO_MOUNTSKILL_COLDTIMER= 20;
        public const byte EPRO_MOVE_TO_TARGET_FOR_ATTACK = 3;
        //		public const byte EPRO_SHOW_ENEMY_INFO= 22;
        public const byte EPRO_QUEST_SKILL = 4;//发送攻击请求
        public const byte EPRO_SHOW_SKILL_HINT_INFO = 5;
        //		public const byte EPRO_OPEN_SKILL_PROCESS_BAR= 25	;	
        //		public const byte EPRO_QUEST_SKILL_FP= 26	;
        //		public const byte EPRO_LEARN_SKILL= 27;
        //		public const byte EPRO_UPDATE_SKILL_BOX = 28; 
        public const byte EPRO_SELECT_TARGET = 6; // 选中攻击目标
        public const byte EPRO_SETBUFFICON_CHANGE = 7; //选中目标的buff图标改变
        public const byte EPRO_SETPROGRESS = 8;   //开始采集
        public const byte EPRO_CANCELPROGRESS = 9;    //采集被打断
        public const byte EPRO_UPDATEBUFF_INFO = 10;  //即时更新目标所有buff 
        public const byte EPRO_ENDPROGRESS = 11;  //结束采集
        public const byte EPRO_SET_OBJ_STATE = 12;    //设置状态
        public const byte EPRO_SET_BF_STATE = 13;//设置为可(鞭尸)暴扁状态
        public const byte EPRO_BF_ATTACK_MONSTER = 14;//(鞭尸)暴扁怪物
        public const byte EPRO_FORCE_QUEST_ENEMY_INFO = 15;   //跳闪
        public const byte SANoticeHeroDead = 16;  //通知随从的死亡状态

        //		public const byte EPRO_TELERGY_STATE			= 31	//设置心法状态
        //		public const byte EPRO_SET_NEXT_RANGE_ATTACK_DST			= 49	//设置下一次方向性群攻的目标点
        //		public const byte EPRO_SET_OPENSATP				= 53;	// 打开/关闭坐标同步

        /*updateMsg更新消息*/
        public const byte EPRO_REFRESH_PLAYER = 1;

        /**mounts坐骑消息---------------------------------------------- */
        public const byte EPRO_ADD_MOUNTS = 0;    // 增加马匹
        public const byte EPRO_SET_HUNGER = 1;    // 设置饥饿度
        public const byte EPRO_FEED_MOUNTS = 2;   // 喂食
        public const byte EPRO_SET_EXP = 3;   // 增加马经验值
        public const byte EPRO_SET_MOUNTS = 4;    // 设置坐骑
        public const byte EPRO_SYNPLAYMOUNT_ACTION = 5;   //广播骑马状态
        public const byte EPRO_OPERATE_MOUNTS = 6; //操作骑乘
        public const byte EPRO_ADD_MOUNTSKILL = 7; //添加一个坐骑技能
        public const byte EPRO_ADD_QUALITY = 8; //提升坐骑品质
        public const byte EPRO_UPDATE_WUXING = 9; //更新坐骑悟性



        /**mythago元神消息---------------------------------------------- */
        public const byte EPRO_ADD_MYTHAGO = 0;   // 增加元神
        public const byte EPRO_OPERATE_MYTHAGO = 1;   // 操作元神
        public const byte EPRO_SET_MYTHAGO_EXP = 2;   // 增加元神验值
        public const byte EPRO_SET_MYTHAGO = 3;   // 广播显示元神
        public const byte EPRO_SYN_MYTHAGO_ACTION = 4;    // 广播元神状态
        public const byte EPRO_SET_MYTHAGO_POINT = 5; // 设置元神的潜力点
        public const byte EPRO_SYN_MYTHAGO_POINT = 6; // 同步元神的潜力点
        public const byte EPRO_SYN_MYTHAGO_LQ = 7;    // 同步元神当前灵气

        /**team 组队消息---------------------------------------------- */
        public const byte EPRO_TEAM_REFRESH = 0;  // 刷新成员信息
        public const byte EPRO_TEAM_JOIN = 1; // 加入
        public const byte EPRO_TEAM_KICK = 2; // 退出( 包括被T 和自己退出 )
                                                //public const byte EPRO_TEAM_TALK			= 3;	// 说话
        public const byte EPRO_TEAM_HEADMEN = 3;  // 设置队长
        public const byte EPRO_TEAM_BREAK = 4;    // 解散队伍
        public const byte EPRO_TEAM_SUC_JOIN = 5; // 成功加入队伍的通知
                                                    //		public const byte EPRO_TEAM_OPEN_SKILL		= 7;	// 开启阵法
                                                    //		public const byte EPRO_TEAM_CLOSE_SKILL		= 8;	// 关闭阵法
                                                    //		public const byte EPRO_TS_SETTROOPSKILL		= 14;	// 设置阵法，学习新阵法，或者设置经验
        public const byte EPRO_TEAM_SYN_BUFF_ICON = 6;    // 同步BUFF状态


        /**relation 社交消息---------------------------------------------- */
        public const byte EPRO_ADD_RELATION = 0;  // 添加关系
        public const byte EPRO_DEL_RELATION = 1;  // 删除关系
                                                    //		public const byte EPRO_REFRESH_ONLINE_FRIENDS			= 2;	// 在线好友列表刷新( 区域-客户端 狭义世界服务器已经屏蔽，所以不处理)
        public const byte EPRO_REQUEST_ADD_RELATION = 2;  // 请求加入关系
        public const byte EPRO_FRIEND_STATE = 3;  // 好友状态
        public const byte EPRO_FRIEND_FIGHT = 4;  // 好友杀人、被杀
        public const byte EPRO_FRIEND_LEVELUP = 5;    // 升级
                                                        //		public const byte EPRO_FRIEND_OPERVENA					= 7;	// 冲脉 灌注
        public const byte EPRO_FRIEND_SYN_DEARVALUE = 6;  // 好感	 //type0
                                                            //		public const byte EPRO_ENEMIE_SYN_HATREDVALUE			= 7;	// 仇恨	 //type2
                                                            //		public const byte EPRO_FRIEND_SYN_DEFEATVALUE			= 8;	// 击杀  //type4

        /**随从消息---------------------------------------------- */
        public const byte EPRO_ADD_HERO = 0;     // 添加一个随从
        public const byte EPRO_OPERATE_HEROS = 1;    // 随从操作。
                                                       //		public const byte EPRO_SYN_HERO_DATA		= 2;	   // 同步随从详细数据。
        public const byte EPRO_SYN_HERO_POS = 3;     // 同步客户端的随从坐标。
        public const byte EPRO_SYNPLAYHERO_ACTION = 4;       //  广播随从相关消息
        public const byte EPRO_SET_HEROSEXP = 5;     //  设置某个随从经验
        public const byte EPRO_HERO_ATTRI = 6;       //  同步随从战斗属性
        public const byte EPRO_ADD_HEROSKILL = 7;     // 增加技能
        public const byte EPRO_UP_ATT_ADD = 8;       //  洗点
        public const byte EPRO_UP_ATT_SURE = 9;      // 确认洗点
        public const byte EPRO_OVER_AUTO_ATTADD = 10;     //结束自动洗点
        public const byte EPRO_CHANGE_NAME = 11;      //修改随从名字
        public const byte EPRO_USEITEM_TO_HERO = 12;      //对随从使用道具
        public const byte EPRO_SET_LOGITY = 13;       //设置随从忠诚度
        public const byte EPRO_UPDATE_HERO = 14;      //更新随从
        public const byte EPRO_UPDATE_HEROTECH_CD = 15;       //武经奇书CD时间更新消息


        /**聊天消息---------------------------------------------- */
        //		public const byte EPRO_CHAT_SYSMSG			= 0;	   // 系统消息
        //		public const byte EPRO_CHAT_SYSCALL			= 1;		// 系统公告
        //		public const byte EPRO_CHAT_PUBLIC			= 2;	// 公众
        //		public const byte EPRO_CHAT_TEAM			= 3;	 // 组队
        //		public const byte EPRO_CHAT_GANG			= 4;	   // 帮派
        //		public const byte EPRO_CHAT_WHISPER			= 5;	// 密谈
        //		public const byte EPRO_CHAT_CHATROOM			= 6;	// 聊天室
        //		public const byte EPRO_CHAT_TEMP_SYSTEM		= 7;	 // 临时显示消息
        //		public const byte EPRO_CHAT_CHANNEL			= 8;    // 聊天频道
        //		public const byte EPRO_CHAT_GLOBAL			= 9;      // 全服务器
        //		public const byte EPRO_CHAT_FACTION			= 10;    // 帮派聊天
        //		
        //		public const byte EPRO_CHAT_SCRIPTBULLETIN		= 11; // 脚本公告
        //		public const byte EPRO_CHAT_TALKMASK			= 12;      // 服务器通知客户端 角色被禁言 和禁言时间
        //		public const byte EPRO_CHAT_SCHOOL			= 13;       // 门派聊天
        //		public const byte EPRO_CHAT_REGION			= 15;     // 场景聊天
        //		public const byte EPRO_CHAT_TIPS			= 16;  	// 提示信息	
        //		public const byte EPRO_PICK_ITEM			= 19;  	// 拾取提示信息
        //		public const byte EPRO_TEAM_TALK			= 24;    // 队伍聊天

        public const byte EPRO_CHAT_MSG = 0;     // 系统消息
        public const byte EPRO_TIP_INFO = 1;      // 提示信息				
        public const byte EPRO_BUFF_IMM = 2;      // 免疫提示信息	
        public const byte EPRO_CHAT_KILLED = 3; // 死亡通知(用于有帮的玩家死亡通知全帮)



        /** 帮派 */
        //		public const byte EPRO_TONG_CLIENT_CREATE= 0;		//创建帮派
        //		public const byte EPRO_TONG_REQUARE_FACTION_NAME= 1;
        //		public const byte EPRO_TONG_QUEST_JOIN= 2;
        //		public const byte EPRO_TONG_MEMBER_UPDATE= 3;
        //		public const byte EPRO_TONG_DELETE= 4; //删除帮会
        //		public const byte EPRO_TONG_INITFACTION_INFO= 5;
        //		public const byte EPRO_INPUT_MEMO= 7;// 要求输入帮派宣言
        //		public const byte EPRO_TONG_UPDATA_FACTION_HEAD= 8; // 客服端请求更新帮派数据
        //		public const byte EPRO_TONG_MEMBER_MANAGER= 9; //成员管理
        //		public const byte EPRO_TONG_UPDATA_RECRUIT= 12; // 更改招募状态
        //		public const byte EPRO_TONG_REMOTE_APPLY_JOIN= 13; //远程申请入帮
        //		public const byte EPRO_TONG_RECV_RAPPLY_JOIN= 14;// 处理远程申请进帮
        //		public const byte EPRO_TONG_INVITE_FACTION_NAME= 15;
        //		public const byte EPRO_TONG_PRETIGE_RANKING= 18;
        //		public const byte EPRO_TONG_CREATE= 24; //帮派创建成功
        //		public const byte ERPO_TONG_LEVEL_UP= 27; //帮会升级消息
        public const byte EPRO_TONG_INVITE_JOIN = 0;// 邀请玩家加入帮会
        public const byte EPRO_TONG_SEND_INVITE_JOIN = 1;// 发送一个邀请入帮的申请给被邀请者
        public const byte EPRO_TONG_QUEST_JOIN = 2;// 申请加入帮会
        public const byte EPRO_TONG_RECV_RAPPLY_JOIN = 3;// 回应加入帮会的申请
        public const byte EPRO_TONG_MEMBER_MANAGER = 4;// 帮派成员管理
        public const byte EPRO_INPUT_MEMO = 5;// 修改帮派宣言
        public const byte EPRO_TONG_MEMBER_UPDATE = 6;// 帮派成员更新
        public const byte EPRO_TONG_DELETE = 7;// 删除帮派
        public const byte EPRO_TONG_PRETIGE_RANKING = 8;// 请求帮会列表
        public const byte EPRO_TONG_INITFACTION_INFO = 9;// 发送帮派完整数据给客户端
        public const byte EPRO_TONG_UPDATA_FACTION_HEAD = 10;// 发送帮派基础数据给客户端
        public const byte EPRO_QUEST_MEMBERLIST = 11;// 请求其它帮会成员ID

        /** menu */
        public const byte EPRO_MENU = 0;  //通过Sid查看玩家装备
        public const byte EPRO_INVITE_JOIN_TEAM = 1;  // 邀请组队
        public const byte EPRO_OTHEREQUIPMENTNAME = 2;    //用名字查看其他人装备
        public const byte EPRO_RELIVE_OP = 3; //复活
        public const byte EPRO_SHOW_OUTEQUIP = 4; // 显示外装
        public const byte EPRO_BUFF_CLOSE_COSTUME = 5; //关闭变身buff
        public const byte RPRO_SET_HEROAI = 6; //设置随从AI
        public const byte EPRO_CHAT_GETINFO = 7;      // 聊天个人信息
        public const byte EPRO_GET_FBLIST = 8;    // 批量加好友
        public const byte EPRO_SETPKRULE = 9; // 设置PK规则
                                                /**交易*/
        public const byte RPRO_QUEST_EXCHANGE = 0;    // 请求和某个玩家进行交易（服务器通知客户端，客户端应答服务器）
        public const byte RPRO_MOVE_EXCHANGE_ITEM = 1;    // 移动一个道具到交易栏（包括金钱）
        public const byte RPRO_EXCHANGE_OPERATION = 2;    // 一个交易对象提交了一个交易操作（确认、取消。。。）
        public const byte RPRO_QUEST_REMOTE_EXCHANGE = 3;    // 请求和某个远程玩家进行基于指定道具的交易
        public const byte RPRO_DEL_EXCHANGE_ITEM = 4;   //删除一个交易栏中的道具，放回到包裹
        public const byte EPRO_INVITE_EXCHANGE = 0;    // 初始交易
        public const byte EPRO_UICLICK_DATA = 6;    // 数据统计
        public const byte EPRO_CLICK_HP = 7;    // 点击隐藏其他玩家

        /** 系统邮件 */
        public const byte EPRO_MAIL_SEND = 0; //发送留言
        public const byte EPRO_MAIL_RECV = 1; //接收留言
        public const byte EPRO_MAIL_DELETE = 2;   //删除留言
        public const byte EPRO_MAIL_NEWMAIL = 3;  //新留言通知
        public const byte EPRO_BROADCAST = 4; //活动广播
                                                /** 玩家邮件 */
        public const byte EPRO_EMAIL_SEND = 0;    //发送邮件
        public const byte EPRO_EMAIL_RECV = 1;    //接收邮件(全部一页的)
        public const byte EPRO_EMAIL_DELETE = 2;  //删除邮件
        public const byte EPRO_EMAIL_NEWMAIL = 0; //新邮件通知
        public const byte EPRO_EMAIL_SYSTEM = 4;  //系统邮件
        public const byte EPRO_EMAIL_SETREAD = 5; //设置已读
        public const byte EPRO_EMAIL_ADDITEM = 6; //收取附件
        public const byte EPRO_EMAIL_MONEY = 7;   //金钱的变化通知
    }
}
