using System;

namespace Network.DataDefs
{
    public class LYBGlobalConsts
    {
        public const string ENCODE = "GBK";

        //---------已确认的全局常量---------------
        // "ping"的ASCII码
        public const uint PING = 0x676e6970;

        // "pong"的ASCII码 
        public const uint PONG = 0x676e6f70;

        // "tick"的ASCII码
        public const uint TICK = 0x6b636974;

        public const short MAX_ROLENAMESIZE = 21;
        public const short MAX_TONGNAMESIZE = 21;
        public const short MAX_TITLENAMESIZE = 21;

        public const short ITEM_HEAD_LENGTH = 7;
        public const short ITEM_DATA_LENGTH = 38;   // 数据块大小
        public const short ITEM_ALL_LENGTH = 50;    // 物品数据大小
        public const short MAX_EQUIP_COUNT = 16;
        
        public const short MAX_SKILL_COUNT = 20;
        public const short MAX_CITTA_COUNT = 16;
        public const short MAX_TALNET_COUNT = 20;

        // 移动路径最大值
        public const short MAX_TRACK_LENGTH = 20;

        //-------------------------------------------------------------
        public const uint INVALID_ID = 0x0b000000;
		
		public const uint PLAYER_VERSION = 215;
		
		public const string XYD_VERSION = "version 0.0.1.0224";
		/**
		 * 错误日志服务器
		 */		
		public const string ERROR_LOG_SERVER = "//127.0.0.1";
		public const int CHAT_WIDTH = 219;
		
		// 缩放比例
		public const double ZOOM_NUM = 1;
		
		// 视野的显示对象最大宽高
		public const int VIEWOBJ_WIDTH = 1920;
		public const int VIEWOBJ_HEIGHT = 1080;
		
		// 场景距舞台X轴的偏移量（像素单位）
		public const int VIEW_LEFT_OFFSET = 0;
		
		// 场景距舞台Y轴的偏移量（像素单位）
		public const int VIEW_TOP_OFFSET = 0;
		
		// 格子宽高 （像素单位）
		public const int GRID_WIDTH = 64;
		public const int GRID_HEIGHT = 32;
		//服务器区块大小
		public const int SERVER_AREA_X = 8;
		public const int SERVER_AREA_Y = 10;
		// 地图Chunk的宽高 下载用（一个swf包的大小）
		public const int CHUNK_WIDTH = ( 256 << 1 );
		public const int CHUNK_HEIGHT = ( 256 << 1 );
		
		// Area大小,这里和服务器的同步大小匹配 （ 单位区块 ） 注：每个区块（64*32）.
		public const int AREA_SIZEX = 12;
		public const int AREA_SIZEY = 16;
		//同屏最大显示对象数
		public static int SCREEN_MAX_OBJECT = 150;
		//同屏最大玩家
		public static int SCREEN_MAX_PLAYER = 80;
		// 视野区块宽高 （ 单位像素 ） 服务器决定 九宫格之一  为了让npc显示范围大，故把此区域放大
		public const int ZONE_WIDTH = (int)(GRID_WIDTH* AREA_SIZEX * ZOOM_NUM + 64 * 4);  // 64 * 12 +256 = 1024
		public const int ZONE_HEIGHT = (int)(GRID_HEIGHT* AREA_SIZEY * ZOOM_NUM + 32 * 4);  // 32 * 16 +128= 640		
		
		// 玩家数据
		// 玩家的基础移动速度（已缩放）
		public const double BASE_SPEED = 100 * ZOOM_NUM;

		
		public const uint MAX_ACCOUNT = 140;
		public const uint MAX_PASSWORD = 140;
//		public const uint MAX_ACCAPASS = MAX_ACCOUNT + MAX_PASSWORD;
		public const uint MAX_ACCAPASS = 1164;
		public const uint CONST_USERPASS = 21;

		/*
		public const uint ROOT_RES_PATH:JFolder = Sgls.mainWorld.path.md( "res" );
		public const uint ANIMATION_RES_PATH:JFolder = ROOT_RES_PATH.md( "animation" );
		public const uint RES_MOV_PATH:JFolder = ROOT_RES_PATH.md( "mov" );
		public const uint ANIMATION_EFFECT_PATH:JFolder = ANIMATION_RES_PATH.md( "effect" );
		public const uint ANIMATION_NPC_PATH:JFolder = ANIMATION_RES_PATH.md( "npc" );
		public const uint UI_RES_PATH:JFolder = ROOT_RES_PATH.md( "ui" );
		public const uint CONFIG_TXT_PATH:JFolder = ROOT_RES_PATH.md( "config" ).md( "txt" );
        public const uint HEAD_RES_PATH:JFolder = UI_RES_PATH.md( "head" );
		public const uint SMALLHEAD_RES_PATH:JFolder = UI_RES_PATH.md( "smallhead" );
		public const uint BATTLE_RES_PATH:JFolder = UI_RES_PATH.md( "battle" );
		public const uint CONFIG_LUA_PATH:JFolder = ROOT_RES_PATH.md( "config" ).md( "lua" );
        public const uint CONFIG_OTHER_PATH:JFolder = ROOT_RES_PATH.md( "config" ).md( "other" );
        public const uint CJ_RES_PATH:JFolder = UI_RES_PATH.md( "cj" );
		public const uint HP_RES_PATH:JFolder = UI_RES_PATH.md( "hp" );
		public const uint SOUND_RES_PATH:JFolder = ROOT_RES_PATH.md( "sound" );
		public const uint ICON_RES_PATH:JFolder = ROOT_RES_PATH.md( "icon" );
		public const uint UI_BG_PATH:JFolder = UI_RES_PATH.md( "bg" );
		public const uint RES_PLOT_PATH:JFolder = ROOT_RES_PATH.md( "plot" );
		public const uint UI_GARDEN_PATH:JFolder = UI_RES_PATH.md( "garden" );
		public const uint UI_TITLE_PATH:JFolder = UI_RES_PATH.md( "title" );
		public const uint JOB_BODY_PATH:JFolder = UI_RES_PATH.md( "smallhead" );*/

		// ping消息发送间隔
		public const uint PINGMARGIN = 15000;
		
		//10大排行
		public const uint ST_ALEVEL = 0;
		public const uint ST_AMONEY = 1;
		public const uint ST_RFAME = 2;
		public const uint ST_LFAME = 3;
		//邮件信息最大长度
		public const uint MAX_MAILSIZE = 400;
		//邮件标题长度
		public const uint MAX_MAILTITLESIZE = 40;
		//邮件附件最大物品个数
		public const uint MAX_MAILITEMSIZE = 5;
		/**
		 * 默认的移动到目标对象附近的半径(格子) 
		 */		
		public const int DEFAULT_NEAR_RADIUS = 2;
		
		/**
		 * 数据对象类型
		 */
		public const int LIFETYPE_BASEOBJECT 	= 0x0;
		public const int LIFETYPE_BASEITEM 	= 0x1;		// 有实体
		public const int LIFETYPE_BASELIFE 	= 0x2;
		public const int LIFETYPE_FIGHTLIFE  	= 0x3;
		public const int LIFETYPE_BASEPLAYER	= 0x4;		// 有实体
		public const int LIFETYPE_PLAYER		= 0x5;		// 有实体
		public const int LIFETYPE_NPC			= 0x6;		// 有实体
		public const int LIFETYPE_HEROS			= 0x10;		// 有实体,随从
		public const int LIFETYPE_DROPITEM			= 0x11;		// 有实体,地面物品
		public const int LIFETYPE_MONSTER		= 0x7;		// 有实体
		public const int LIFETYPE_BUILDING		= 0x8;		// 有实体
		public const int LIFETYPE_ROBOT		= 0x9;		// 机器人
		
		/**
		 * 显示对象类型 
		 */		
		public const int IID_BASELIFE = 1;
		public const int IID_BASESTATIC = 2;
		public const int IID_MAPEFFECT = 3;
		public const int IID_SKILLEFFECT = 4;
		public const int IID_BASEPLAYER = 5;
		public const int IID_PLAYER 			= 6;
		public const int IID_NPC				= 0x11210094;
		public const int IID_MONSTER			= 0x112100a4;
		public const int IID_ITEM				= 0x1168eb14;
		public const int IID_HEROS				= IID_PLAYER+3;
		public const int IID_CONTESTPLAYER 	= IID_PLAYER+1;
		public const int IID_HORSE			= IID_PLAYER+2;
		
		/***动作相关**服务器*/ 
		public const int EA_STAND			= 0;
		public const int EA_WALK			= 1;
		public const int EA_RUN			= 1;
		public const int EA_JUMP			= 2;
		public const int EA_JUMP2			= 40;	//第二个跳
		public const int EA_JUMP3			= 45;	//二段跳
		public const int EA_RUN2			= 42;	//过江跑
		public const int EA_ZAZEN			= 3;	//打坐
		public const int EA_WOUND 			= 4;
		public const int EA_DEAD			= 5;
		public const int EA_HIDE			= 6;	//隐藏
//		public const int EA_SKILL_ATTACK	= 7;	// 新逻辑使用的攻击状态
		public const int EA_ATTACK			= 7;	// 新逻辑里不会使用到这个状态，但是，暂时保留。
		public const int EA_ATTACK2			= 51;	// 攻击动作2
		public const int EA_ATTACK3			= 52;	// 攻击动作3
		public const int EA_PROGRESS 		= 8;	// 采集 20100511_chenj
		public const int EA_BEAT_FLAT 		= 9;	//可被(鞭尸)暴扁	
		public const int EA_KNOCKOUT 		= 10;	//击飞	
		public const int EA_AC_MAX 		= 11;	// 前面插值后改变
		public const int EA_AC_NIL 		= 12;
		public const int EA_RIDE 			= 15;   //骑马状态
		public const int EA_RIDEATTACK 			= 30;	//马上攻击
		public const int EA_RIDEATTACK2 			= 31;	//马上攻击
		public const int EA_RIDEWOUND 			= 22;	//马上受伤
		public const int EA_RIDESTAND 			= 23;	//马上站立
		public const int EA_FLY			= 24;
		public const int EA_HORSEJUMP			= 25;	//扑
//		public const int EA_ATTACKSTAND			= 40;	//战斗待机
		public const int EA_STANDBIG			= 41;	//人物面板大形象
		public const int EA_RUN3 			= 44;	//过锁道
		public const int EA_VILLRun 			= 46;	//新手副本冲跑
		public const int EA_SPRING_ATT = 10;//温泉中的攻击动作
		public const int EA_SPRING_JUMPUP = 2;//温泉中的跳上跳台动作
		public const int EA_SPRING_JUMPDOWN = 12;//温泉中的跳下跳台动作
		public const int EA_SPRING_PLAY = 13;//温泉中的表演动作
		public const int EA_SPRING_STAND = 14;//温泉中的站立动作
		/**宠物的特殊动作*/
		public const int EA_PET1 = 67;
		public const int EA_PET2 = 68;
		public const int EA_PET3 = 69;
		
		/***动作相关**前端*/ 
		public const string EQ_STAND			= "01";
		public const string EQ_WALK			= "02";
		public const string EQ_RUN			= "02";
		public const string EQ_JUMP			= "06";
		public const string EQ_JUMP2			= "40";
		public const string EQ_ZAZEN			= "05";
		public const string EQ_ATTACKSTAND			= "07";	//战斗待机
		public const string EQ_STANDBIG			= "09";	//人物面板大形象
		public const string EQ_ATTACK		= "10";	// 新逻辑里不会使用到这个状态，但是，暂时保留。
		public const string EQ_ATTACK2		= "11";	// 新逻辑里不会使用到这个状态，但是，暂时保留。
		public const string EQ_ATTACK3		= "12";	// 新逻辑里不会使用到这个状态，但是，暂时保留。
		public const string EQ_WOUND 			= "03";
		public const string EQ_DEAD			= "04";
		public const string EQ_RIDE 			= "21";   //骑马跑
		public const string EQ_RIDEATTACK 			= "30";	//马上攻击
		public const string EQ_RIDEATTACK2 			= "31";	//马上攻击2
		public const string EQ_RIDEWOUND 			= "22";	//马上受伤
		public const string EQ_RIDSTAND 			= "23";	//马上站立
		public const string EQ_FLY 			= "24";	//马上跳
		public const string EQ_FLYSTAND			= "40";	//飞行
		public const string EQ_HORSEJUMP			= "25";	//马扑
		public const string EQ_RUN2			= "42";	//过江跑
		public const string EQ_RUN3 			= "44";	//过锁道
		public const string EQ_JUMP3 			= "45";	//二段跳
		public const string EQ_VILLRun 			= "46";	//新手副本冲跑
		
		public const string EQ_11 = "11"; //特殊动作
		public const string EQ_12 = "12";
		public const string EQ_13 = "13";
		public const string EQ_14 = "14";
		public const string EQ_15 = "15";
		public const string EQ_16 = "16";
		public const string EQ_17 = "17";
		public const string EQ_18 = "18";
		public const string EQ_30 = "30";
		public const string EQ_31 = "31";
		public const string EQ_32 = "32";
		public const string EQ_33 = "33";
		/**宠物的特殊动作*/
		public const string EQ_PET1 = "67";
		public const string EQ_PET2 = "68";
		public const string EQ_PET3 = "69";
		/**
		 * EMoveTo
		 */
		public const int MT_INPACKAGE    		= 0; 		// 从背包到背包
		public const int MT_PACKAGE			= 1;        // 从装备到背包
		public const int MT_EQUIP				= 2; 		// 从背包到装备
		public const int MT_Package_F_Mythago 	= 3;		// 元神到背包	
		public const int MT_Package_F_Heros 	= 3;		// 随从到背包
		public const int MT_BUNDLE				= 4;		// 包裹框移动到背包
		
		/**
		 *MoveWhatType 
		 */		
		public const int MWT_DelEquip		   = 0;
		public const int MWT_DelMythago	   = 1;
		
		/**
		 * 骑乘操作类型
		 * */
		public const int EOM_EQMOUNT 				= 0;			//装备马
		public const int EOM_UNEQMOUNT 			= 1;			//撤销马
		public const int EOM_UPMOUNT 				= 2;			//上马
		public const int EOM_DOWNMOUNT			= 3;			//下马
		public const int EOM_DELMOUNT				= 4;			//删除马
		
		// 元神操作类型
		public const int EOM_EQUIP_MYTHAGO 		= 0;			//装备元神
		public const int EOM_UNEQUIP_MYTHAGO 		= 1;			//撤销元神
		
		/**
		 *移动间隔 
		 */		
		public const int MOVE_INTERVAL           = 300;
		/**
		 *跳跃间隔 
		 */		
		public const int JUMP_INTERVAL           = 1000;
		
		/**
		 * 队伍成员最大值 
		 */		
		public const int MAX_TEAMER = 3;
		/*		
		聊天相关
		*/
		public const int MAX_CHAT_LEN             = 128;  
		
		//buff 离线数据保存
		public const int BUFF_SAVE_MAX = 16;
		
		/**快捷栏相关*/
		public const int SHORTCUT_MAX = 10;
        //		public const int SHORTCUT_ROW_MAX = 2;
        //		public const int SHORTCUT_COL_MAX = 10;

        /*
                public const uint horseColors:Array = [ "#FFFFFF", "#4eff00", "#00cbfe", "#ff00d8","#ff7e1e" ];
                public const uint equipColors:Array = [ "#edebdf", "#86f800", "#27baff", "#f500f0", "#ff9b00", "#fd1c1c" ];
                public const uint horseColors2:Array = [ 0xFFFFFF, 0x4eff00, 0x00cbfe, 0xff00d8,0xff7e1e ];
                public const uint equipColors2:Array = [ 0xedebdf, 0x86f800, 0x27baff, 0xf500f0, 0xff9b00, 0xfd1c1c ];
                public const uint horseNewColor:Array = ["#FFFFFF","#FFFFFF","#00CBFE","#ff00d8","#ff7e1e"];
                public const uint horseNewColor2:Array = [0xFFFFFF,0xFFFFFF,0x00CBFE,0xff00d8];*/

        /** 跳跃xy的最大距离 */
        public const int JumpMaxX = 8;
		public const int JumpMaxY = 10;
		/** 跳斩空中暂停帧数 */
		public const int JumpAttackStopIndex = 8;
		/** 跳上升暂停帧数 */
		public const int JumpUpStopIndex = 5;
		/** 跳下降暂停帧数 */
		public const int JumpDownStopIndex = 11;
		
		/** 跳斩空中暂停帧数 */
		public const int JumpAttackStopIndex2 = 3;
		/** 跳上升暂停帧数 */
		public const int JumpUpStopIndex2 = 12;
		/** 跳下降暂停帧数 */
		public const int JumpDownStopIndex2 = 11;
		/** 扑空中暂停帧数 */
		public const uint JumpPuffStopIndex1 = 14;
		/** 扑下降暂停帧数 */
		public const uint JumpPuffStopIndex2 = 18;
/*
		/ ** 属性类型对应的名字 * /
		private static var AttributeName:Array;
		/ ** 萌宠属性类型对应的名字 * /
		private static var MCAttributeName:Array;
		
		//（攻击+防御）1+生命0.2+（命中+闪避+暴击+格挡+抵抗）*1.3+特殊攻击2+（火系抗性+冰系抗性+木系抗性）1+抗性减免1.5
		// 装备评分各属性系数
		public const uint AttributeScore:Array = [0.2,2,1,1,1.3,1.3,1.3,1.3,1.3,1,1,1,0,1.5];*/

		/** PK规则 */
		public const int ePK_ALL			= 0x00;		// 0x00: 所有不避(全体)
		public const int ePK_SAMECAMP	= 0x01;		// 0x01: 回避同阵营玩家(阵营)
		public const int ePK_DIFCAMP		= 0x02;		// 0x02: 回避异阵营玩家
		public const int ePK_MIDCAMP		= 0x04;		// 0x04: 回避中立玩家
		public const int ePK_SAMEGROUP		= 0x04;		// 0x04: 回避同服务器玩家
		public const int ePK_SAMESCHOOL	= 0x08;		// 0x08: 回避同门派玩家
		public const int ePK_DIFSCHOOL	= 0x10;		// 0x10: 回避异门派玩家
		public const int ePK_SAMEFAC		= 0x20;		// 0x20: 回避同帮派玩家(帮会)
		public const int ePK_DIFFAC		= 0x40;		// 0x40: 回避异帮派玩家
		public const int ePK_NOALL		= 0xFF;		// 0xFF: 避开所有(和平)
		public const int areaUTC = 480;	//北京时间和utc的差值
		public const int nameHight = -155;	//名字高度
		public const int corssType = 2;	//跨服方式
		
		
		
		/******************下面为游戏特殊判断***100000的是没配的地图 *************************/
		public const int map_Qualifying = 512;	//排位赛地图
		public const int map_Qualifying2 = 510;	//跨服排位赛地图
		public const int map_FactionTowerStart = 552;//帮会闯塔
		public const int map_FactionTowerEnd = 567;//帮会闯塔
		public const int map_MineRob = 516;	//金矿掠夺地图
//		public const int buff_weapon = 126;	//附灵buff编号
		public const int map_new = 3;	//新手村地图编号
		public const int map_main = 1;	//主城编号
		public const int map_finish = 100000;//100;	//钩鱼地图编号
//		public const int map_top = 512;	//排位赛地图编号
		public const int map_WunQuan = 802;//5081;	//温泉地图编号
		public const int map_FF =803; //10000;	//帮会战地图
		public const int map_TJBX = 805;	//天降宝箱副本地图
		public const int map_Hunt = 100000;//501;	//天宫狩猎地图编号
		public const int map_BUYU = 100000;//502;	//捕鱼地图编号
		public const int map_BUYUMain = 100000;//1001;	//捕鱼地图主线副本编号
		public const int map_ARENAWAIT = 100000;//101;	//竞技场报名地图
		public const int map_ARENA = 100000;//510;	//竞技场PK地图
		public const int map_LongCity = 101;//510;	//龙城争霸
//		public const int map_QSLS = 500;	//曲水流觞地图
		public const int map_SOUL = 100000;//5051;	//帮会神兽地图
		public const int map_Task = 100000;//519;	//熔岩灵府
		public const int map_factionBase = 550;//550;	//帮会驻地编号
		public const int map_factionMagic = 100000;//521;	//帮会秘境编号
		public const int map_WCZB = 515;//龙城争霸 王城争霸地图编号
		public const int map_LCYD = 523;//龙城异动地图编号
//		public const int map_LCYD2 = 524;//龙城异动地图2
		public const int map_NXLC = 525;//逆袭龙城地图编号
		public const int map_SHUILAO_FB = 517;//水牢FB
		public const int map_SHUILAO_FB1 = 518;//水牢FB1
		public const int map_HLDT = 801;//欢乐答题
		public const int map_factionEscore = 100;//11;	//帮会运镖地图
		public const int map_BWZQ = 804;//比武招亲地图
		public const int map_BWZQJS = 806;//比武招亲决赛地图
		public const int map_FRIST = 807;//论剑的第一层
		public const int map_SECOND = 808;//论剑第二层
		public const int map_THIRD = 809;//论剑第三层
		public const int map_FOURTH = 810;//论剑第四层
		public const int map_BWZQJSFB = 520;//比武招亲决赛主线副本地图
		public const int map_crossCityFight = 745;//城邦战地图编号
//		public const int buffid_road = 83;	//巡防buff编号
//		public static var res_Wind;	//旋风资源编号(旋风状态放最上层 方向始终为0)
//		public const int buffid_light = 106;	//九黎激光buffid
//		public const int skill_light = 106;	//九黎激光skillid
//		public const int skill_Wind = 84;	//修仙暴风雪skillid
		public const int map_Tower = 504;	//守护梁王
		public const int map_1v1 = 622;//诸神之战地图编号 巅峰之战
		public const int map_BestServer = 625;//最强服务器，地图编号
		public const int map_SWBoss = 816;//最强服务器，地图编号
		public const int map_1v1_ready = 110;//诸神之战准备地图编号，巅峰之战准备地图编号
		public const int taskid_horse = 1054;	//骑乘技任务编号
		public const int taskid_money = 1181;	//铜钱副本任务编号
		public const int taskid_awake = 2002;//2006;	//觉醒技任务编号
		public const int taskid_suodao = 1143;//2006;	//跳索道任务编号
		public const int buffid_SanJie = 169;	//三界战场不能上马，不能采集的buff
		public const int buffid_BioYs = 265;	//列传6幕隐身buff
		public const int buffid_downHorse = 509;	//三界战场不能上马，不能采集的buff
		public const int buffid_equipHorse = 216;	//坐骑装备落马buff
//		public const int map_showplayer:Array = [];	//不隐藏玩家的地图编号
		public const int buff_client = 95;	//微端登录buff
		public const int buff_Intimacy = 86;	//亲密度buff86
		public const int buff_HP = 503;	//血包buff
		public const int buff_Method1 = 491;	//风阵buff
		public const int buff_Method2 = 489;	//阳阵buff
		public const int buff_Method3 = 490;	//雨阵buff
//		public const int buff_vip4 = 300;	//vip4buff
//		public const int buff_city = 217;	//弩车buff
//		public const int buff_city2 = 219;	//弩车buff2
		public const int buff_Pro = 242;	//挂机保护buff
		public const int buff_Pro2 = 459;	//夜间保护buff
		public const int buff_King = 304;	//国王特权buff
		public const int buff_Faction = 125;	//帮会庇佑buff
		public const int buff_Light = 500;	//水牢火把buff
		public const int buff_Invisible = 26;	//水牢隐身buff
		public const int buff_feijian = 245;//卸甲buff
		public const int Fashion_Marry = 9921;	//结婚变身,处理成时装,buff162
		public const int map_CBoss = 1041;	//跨服boss地图编号
		public const int map_ES = 1042;	//装备升星副本地图编号
		public const int map_Treasure = 1043;	//跨服寻宝地图编号
		public const int buff_NoCross = 196;	//5分钟不能进跨服的buff编号
		public const int buff_fire = 309;	//需要所有人看到掉血的buff编号
		public const int buff_fire2 = 353;	//需要所有人看到掉血的buff编号
		public const int buff_fire3 = 366;	//炸弹
		public const int buff_Scale = 450;	//缩放buff
		public const int buff_Sky = 468;	//浮空眩晕buff
		public const int buff_myKill = 509;	//自杀buff
		public const int buff_bossLight1 = 126;	//boss闪光buff
		public const int buff_bossLight2 = 127;	//boss闪光buff
		public const int buff_bossLight3 = 128;	//boss闪光buff
		public const int buff_couplesFishing_hide = 187;	//夫妻捕鱼-隐身buff
		public const int buff_dbSite = 463;//双倍打坐buff
		public const int buff_reicarnation = 214;//转生buff
		public const int buff_reicarnationTwo = 255;//轮回buff
		public const int buff_newCard = 226;//新月卡包月优惠卡图标
		/**名牌buffid**/
		public const int buff_designer = 164;//
		/**石头buffid**/
		public const int buff_scrossStone = 178;//
		/**剪刀buffid**/
		public const int buff_scrossClipper = 177;//
		/**布buffid**/
		public const int buff_scrossCloth = 179;//
		/**冰凍buffid**/
		public const int buff_frozen = 143;//
		/**帮会温泉buffid**/
		public const int buff_spring = 130;
		/**帮会闯塔-漩涡BUFF1**/
		public const int buff_tower1 = 276;
		/**帮会闯塔-漩涡BUFF1**/
		public const int buff_tower2 = 277;
		
		public const int map_ExpStage = 514;//经验副本底图id
		public const int map_VipStage = 513;//vip副本底图id
		public const int map_StoreHouseStage = 509;//宝藏副本底图id
		public const int map_MapBoss = 220;//打宝地图ID
		public const int map_SuitBoss = 223;//套装boss地图
		public const int map_GuildBoss = 519;//40级BOSS副本底图id-新手引导
		public const int map_GuildBoss_ID = 1022;//40级BOSS副本底图id-新手引导
		/**新手副本id*/
		public const int Map_NewCopy = 1001;	//新手副本id
		/**单人塔防副本id*/
		public const int one_tower = 22001;//单人塔防副本id
		/**帮会-采花副本id*/
		public const int map_FactionFlower = 551;//帮会-采花副本id
		/**野外BOSS地图一层*/
		public const int map_WildBossMapOne = 221;//野外BOSS地图一层
		/**野外BOSS地图二层*/
		public const int map_WildBossMapTwo = 222;//野外BOSS地图二层
		/**野外BOSS地图三层*/
		public const int map_WildBossMapThree = 224;//野外BOSS地图三层
		/**神龙幻境*/
		public const int map_Dragon = 526;//神龙幻境
		/**装备升星副本*/
		public const int map_EquipStar = 527;
		/**夫妻捕鱼*/
		public const int map_couplesFishing = 528;
		/**夫妻捕鱼-船的名字的偏移量*/
		public const int couplesFishBoatNameHeight = -50;
		
		/**跨服3v3地图编号*/
		public const int map_3v3PkMap1 = 601;
		public const int map_3v3PkMap2 = 602;
		public const int map_3v3PkMap3 = 603;
		public const int map_3v3PkMap4 = 604;
		
		/**跨服BOSS**/
		public const int map_crossBossMap = 610;
		
		/**跨服驻地-木材场**/
		public const int map_crossStationWoodMap = 611;
		/**跨服驻地-铁矿场**/
		public const int map_crossStationIronMap = 612;
		/**跨服驻地-战场**/
		public const int map_crossStationBattleMap = 613;
		/**跨服驻地-驻地1**/
		public const int map_crossStationMap1 = 614;
		/**跨服驻地-驻地2**/
		public const int map_crossStationMap2 = 615;
		/**跨服驻地-驻地3**/
		public const int map_crossStationMap3 = 616;
		/**跨服驻地-驻地4**/
		public const int map_crossStationMap4 = 617;
		/**跨服驻地-驻地5**/
		public const int map_crossStationMap5 = 618;
		/**跨服驻地-战场pvp**/
		public const int map_crossStationBattleMapPvP = 619;
		
		/**帮会决战**/
		public const int map_factionJZ = 811;
		
		/**坐骑装备副本**/
		public const int map_horseEquipStageMap = 529;
		
		/**组队护送**/
		public const int map_teamEscort = 530;
		
		/**三王夺嫡-刺探副本*/
		public const int map_threeKingsSpyMap = 531;
		
		/**地牢夺宝-报名地图*/
		public const int map_dungeonTreasureWaitMap = 621;
		/**地牢夺宝-地图*/
		public const int map_dungeonTreasureMap = 626;
		
		/**光翼梦境-地图*/
		public const int map_wingStageMap = 532;
		
		///跨服龙族墓地///////////////////////////////////////////
		/**龙族墓地主地图*/
		public const int map_dragonBurailMain = 812;
		/**龙族墓地大地图*/
		public const int map_dragonBurailBig = 813;
		/**龙族墓地小地图*/
		public const int map_dragonBurailSmall = 814;
		/**龙族墓地和平地图*/
		public const int map_dragonBurailPeac = 815;
		/**王者之盾任务地图*/
		public const int map_shieldTaks = 817;
		/**飞剑任务地图*/
		public const int map_feijianTaks = 821;
		
		/**第七幕  7 个地图*/
		public const int map_Biography7_1 = 713;
		public const int map_Biography7_2 = 714;
		public const int map_Biography7_3 = 715;
		public const int map_Biography7_4 = 716;
		public const int map_Biography7_5 = 717;
		public const int map_Biography7_6 = 718;
		
		/**琅琊列传比武招亲副本地图*/
		public const int map_biographyBWZQ = 708;
		/**琅琊列传第2章第2幕地图*/
		public const int map_biography12 = 729;
		
		/**琅琊列传第5幕副本地图*/
		public const int map_biographyStage5 = 709;
		
		/**夫妻守卫-地图*/
		public const int map_couplesGuardMap = 535;
		
		/**元神开启副本*/
		public const int map_stageOpenYS = 1006;
		/**跨服比武招亲副本*/
		public const int map_kfBWZQ = 823;
	
		/**跨服守护-地图*/
		public const int map_crossGuardMap = 537;
		
		/**宠物岛挂机地图--开始编号*/
		public const int map_petIslandMapStart = 40;
		/**宠物岛挂机地图--结束编号*/
		public const int map_petIslandMapEnd = 49;
		
		/**宠物岛挂机地图--开始编号*/
		public const int map_petIslandMapStartL = 5401;
		/**宠物岛挂机地图--结束编号*/
		public const int map_petIslandMapEndL = 5584;
		
		/**萌宠副本-地图*/
		public const int map_newPetStageMap = 824;
		
		/**水牢副本结算道具数量最大格数*/
		public const int SHUILAO_ITEMS_MAX_NUM = 16;
		
		public const string effect_trans = "1_0_28";	//传送特效编号,循环
		public const string effect_trans2 = "1_0_29";	//传送特效编号,传送完毕播的
		public const string effect_Fashion = "1_0_60";	//变身播一次
		public const string effect_LevelUp = "1_0_21";	//升级
		public const string effect_Life = "1_1_64";	//原地复活
		public const string effect_faction1 = "1_0_244";	//帮会镖车牵引,怪身上
		public const string effect_faction2 = "T_101";	//帮会镖车牵引,玩家身上
		public const string effect_faction3 = "T_102";	//帮会镖车身上元宝
		public const string effect_faction4 = "1_0_42";	//帮会镖车打烂
		public const string effect_1v1Arrow = "1_0_106";	//1v1地上指向对方的箭头
		public const string effect_horseOn = "1_0_31";	//上坐骑
		public const string effect_horseDown = "1_0_30";	//下坐骑
		public const string effect_smoke = "1_2_91";	//击飞烟灰
		public const string effect_water = "1_2_92";	//击飞水花
		public const string effect_enemy = "T_100";	//仇人
		public const string effect_Kill1 = "T_318";	//连斩1
		public const string effect_Kill2 = "T_319";	//连斩2
		public const string effect_Kill3 = "T_320";	//连斩3
		public const string effect_BWZQFM = "T_103";	//比武招亲驸马特效
		public const string effect_LUNJIAN = "T_104";//论剑的称号
		public const string effect_WEATH = "T_327";	//财神特效
		public const string effect_HorseSkill = "1_2_33";	//践踏
		public const string effect_JumpDown = "1_0_30";	//跳落地
		public const string effect_AutoRoad = "T_321";	//自动寻路
		public const string effect_AutoFight = "T_322";	//自动战斗
		public const string effect_Protect = "T_323";	//夜间保护
		public const string effect_Hunt1 = "1_0_75";	//天宫狩猎箭1
		public const string effect_Hunt2 = "1_0_62";	//天宫狩猎箭2
		public const string effect_Hunt3 = "1_0_31";	//天宫狩猎笼子1
		public const string effect_Hunt4 = "1_0_103";	//天宫狩猎笼子2
		public const string effect_Flow1 = "4_1_0_04";	//花雨:茉莉花
		public const string effect_Flow2 = "4_1_0_03";	//花雨2
		public const string effect_Flow3 = "3_22";	//送人的烟花
		public const string effect_SuitAddQ = "1_0_80";	//套装特效
		public const string effect_SuitSence = "1_0_82";	//新套装特效
		public const string effect_SuitUI = "1_0_85";	//UI上的套装特效
		public const string effect_myCir = "1_1_36";	//自身地圈
		public const string effect_monsterIce = "1_2_98";	//家将副本,boss出来带破冰特效
		public const string effect_fire2 = "1_1_68";	//火墙地圈
		public const int NEW_COPY_TASKNPC = 72;//新手副本中弹出对话的npc
		public const string effect_hd_active = "2_0_57";//护盾激活特效
		public const string effect_NXLC_Protect = "2_0_26";	//逆袭龙城-祭坛保护特效
		public const string effect_suit_panel = "1_0_84";
		public const string effect_spring = "1_0_86";
		public const string effect_spring_soap = "1_1_72";//丢肥皂
		public const string effect_spring_soapAtc = "1_2_111";//丢肥皂被丢特效
		public const string effect_equipstarCopy = "1_2_112";
		public const string effect_equipstarCopy2 = "2_0_32";	//星辰副本拖尾特效
		public const string effect_framingPlat = "1_0_132";	//种萝卜特效
		public const string effect_framingPlat2 = "1_0_146";	//种树特效
		public const string effect_towerBeeEffect = "1_2_116";	//帮会闯塔-蜂巢特效
		public const string effect_couplesFishingEffect = "1_1_75";	//夫妻捕鱼-鱼叉特效
		public const string effect_couplesFishingAddHP = "2_0_29";	//夫妻捕鱼-加血特效
		public const string effect_couplesFishing1 = "1_1_79";	//夫妻捕鱼-特效1
		public const string effect_couplesFishing2 = "1_1_80";	//夫妻捕鱼-特效2
		public const string effect_couplesFishing3 = "1_1_81";	//夫妻捕鱼-特效3
		public const string effect_bomb = "1_2_44";	//怪自爆特效
		public const string effect_couplesFishingBeauty = "N_14";	//夫妻捕鱼-美人鱼称号
		public const string effect_MonsterDisEff = "2_0_59";	//探测离怪地圈
		public const string effect_marry_male = "T_105";//新郎称号
		public const string effect_marry_female = "T_106";//新娘称号
		/**名牌effect**/
		public const string effect_designer = "T_331";//
		/**敌人名牌effect**/
		public const string effect_designer_def = "T_332";//
		/**石头effect**/
		public const string effect_scrossStone = "T_329";//
		/**石头effect**/
		public const string effect_scrossStone_def = "T_334";//
		/**剪刀effect**/
		public const string effect_scrossClipper = "T_328";//
		/**剪刀effect**/
		public const string effect_scrossClipper_def = "T_333";//
		/**布effect**/
		public const string effect_scrossCloth = "T_330";//
		/**布effect**/
		public const string effect_scrossCloth_def = "T_335";//
		/**剪刀石头布effect**/
		public const string effect_ClipperStoneCloth = "T_336";//
		/**帮会决战-第1名**/
		public const string effect_factionJZ1 = "T_111";
		/**帮会决战-第2名**/
		public const string effect_factionJZ2 = "T_112";
		/**帮会决战-第3名**/
		public const string effect_factionJZ3 = "T_113";
		/**帮会决战-第4名**/
		public const string effect_factionJZ4 = "T_114";
		/**帮会决战-第5名**/
		public const string effect_factionJZ5 = "T_115";
		/**帮会决战-第6名**/
		public const string effect_factionJZ6 = "T_116";
		/**帮会决战-第7名**/
		public const string effect_factionJZ7 = "T_117";
		/**帮会决战-第8名**/
		public const string effect_factionJZ8 = "T_118";
		/**帮会决战-第9名**/
		public const string effect_factionJZ9 = "T_119";
		/**帮会决战-第10名**/
		public const string effect_factionJZ10 = "T_120";
		
		/**城邦战-自己占领**/
		public const string effect_cityFightSelf = "N_18";
		/**城邦战-敌方占领**/
		public const string effect_cityFightdef = "N_17";
		
		/**组队护送-天之卷轴**/
		public const string effect_teamEscortTian = "2_0_40";
		/**组队护送-地之卷轴**/
		public const string effect_teamEscortDi = "2_0_41";
		/**组队护送-夏冬称号**/
		public const string effect_teamEscortTitle = "N_15";
		
		/**刺探副本-1星护卫**/
		public const string effect_spyFirstGuard = "T_410";
		/**刺探副本-2星护卫**/
		public const string effect_spySecondGuard = "T_411";
		/**刺探副本-3星护卫**/
		public const string effect_spyThirdGuard = "T_412";
		/**刺探副本-技能特效**/
		public const string effect_spySkill = "1_2_138";
		
		/**组队副本-1转副本陷阱爆炸特效**/
		public const string effect_teamStageBoxBoom = "1_2_44";
		
		public const int chariot_call_buffId = 506;//可以召唤变声buffid
//		public const uint chariot_buffIdList:Array = [110,111,112,113,114,115,116,117,118,119];//变身成战车的buffid
		

		public const int reachPointType_BHZ = 1;//积分达成的类型-帮会战
		public const int reachPointType_LCZB = 2;//积分达成的类型-龙城争霸
		public const int reachPointType_BWZQ = 3;//积分达成的类型-比武招亲
		public const int reachPointType_LCYD = 4;//积分达成的类型-龙城异动
		public const int reachPointType_NXLC = 5;//积分达成的类型-逆袭龙城
		public const int reachPointType_LYLJ = 6;//积分达成的类型-琅琊论剑
		public const int reachPointType_KFZD = 7;//积分达成的类型-跨服驻地
		public const int reachPointType_RRYZ = 8;//积分达成的类型-跨服驻地
		public const int reachPointType_LUNJIAN = 9;//积分达成的类型-跨服论剑 和论剑
		public const int reachPointType_CITYFight = 10;//积分达成的类型-城邦战（经验，跨服荣誉，功勋）
		
		public const int stageInvateType_CT = 1;//副本的类型-三王夺嫡-刺探
		public const int stageInvateType_LZ = 2;//副本的类型-琅琊列传
		
		//跨服邀请的类型
		public const int crossInvateType_PK = 1;//跨服的类型-跨服3V3
		public const int crossInvateType_SH = 2;//跨服的类型-跨服守护
		
		public const int pic_xiuqiu = 3043; //秀球资源编号

        /**
		 * 获取一个区间内的随机整数
		 * @param min
		 * @param max
		 * @return 
		 */
        static Random rdm = new Random();
        double randomDouble = rdm.NextDouble();
        public static int random(int min, int max)
        {
            return (int)(rdm.NextDouble() * (max - min)) + min;
        }
    }
}
