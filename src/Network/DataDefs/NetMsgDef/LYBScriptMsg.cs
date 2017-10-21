using Network.Net;

namespace Network.DataDefs.NetMsgDef
{
    public class SALuaCustomMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SCRIPT_MESSAGE, SCMPS.EPRO_LUACUSTOM_MSG);
        public SALuaCustomMsg() : base(opCode) { }

        public byte flags_;     // flags
        public byte flags2_;    // flags2 (firstId?)
        public byte flags3_;    // flags3 (secondId?)
        public TBinaryData lua_bin_data_;
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref flags_);
            ar.Handle(ref flags2_);
            ar.Handle(ref flags3_);

            short lua_data_size = 0;
            ar.Handle(ref lua_data_size);
            lua_data_size = (short)(lua_data_size & 0x7fff);

            if (lua_bin_data_ == null)
                lua_bin_data_ = new TBinaryData();
            else
                lua_bin_data_.Reset();

            lua_bin_data_.Reserve(lua_data_size);
            var buff = lua_bin_data_.Buff;
            ar.Handle(ref buff, lua_data_size);
            lua_bin_data_.Write(null, 0, lua_data_size);
        }
    }

    public class SQLuaCustomMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SCRIPT_MESSAGE, SCMPS.EPRO_LUACUSTOM_MSG);
        public SQLuaCustomMsg() : base(opCode) { }

        public byte flags = 250;    //?
        public byte first_id_;      // ids[1] (firstId)
        public byte second_id_;     // ids[1] (secondId)
        public TBinaryData lua_bin_data_;

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref flags);
            ar.Handle(ref first_id_);
            ar.Handle(ref second_id_);
            ar.Handle(ref lua_bin_data_);
        }
    }


    public class SAItemMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(37, 25);
        public SAItemMsg()
            : base(opCode)
        { }

        public byte MoneyType;
        public uint Money;
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);

            ar.Handle(ref MoneyType);
            ar.Skip(1);
            ar.Handle(ref Money);


            //UnityEngine.Debug.Log(MoneyType +":"+Money+"-------");
        }
    }
    /**  
	 * 主要是L_setmessage
	 */

    public class Ret_BackMsg
    {
        public const uint B_FULLBAG = 1;   // 背包满
        public const uint B_NOTSCHOOL = 2;// 门派不同
        public const uint B_SEXLIMITED = 3;// 性别不符
        public const uint B_LV_NOTENOUGH = 4;// 等级不足
        public const uint B_LIMIT_TALK = 5;// 禁止刷屏
        public const uint SYS_LEVEL_NOT_ENOUGH = 6;   // 进入场景的等级不足
        public const uint SYS_CAN_NOT_DO_PK = 7; // 场景限制不能进行PK

        public const uint B_ITEM_PROTECTED = 9;// 物品保护时间
        public const uint B_LEVEL_TOO_HIGH = 10;               // 等级太高
        public const uint N_ITEM_DISABLED = 11;// 该道具已经被绑定或锁定
        public const uint N_TEMPITEM_MAX = 12;                 // 您的临时物品数量已达到最大
        public const uint N_ITEM_CANTDROP = 13;                // 该道具为不能丢弃的道具
        public const uint N_TEMPITEM_GETFAIL_FULL = 14;        // 您的背包已满，临时道具掉落！
        public const uint N_NOTENOUGH_PACK = 15;               // 您的背包空间不够，请整理背包以后再试
        public const uint N_NOTFIND_ITEM = 16;             // 没有找到指定的道具，或道具无效！
        public const uint N_CREATEFACTION_ALREADYIN = 17;      // 您已加入帮派，无法创建新的帮派！
        public const uint N_CREATEFACTION_NOCAMP = 18;         // 您还没加入阵营，无法创建帮派！
        public const uint N_CREATEFACTION_NAMEINVALID = 19;    // 帮派名称非法
        public const uint N_CREATEFACTION_MAX = 20;            // 对不起，帮派数量已达到上线
        public const uint N_CREATEFACTION_NAMEEXISTS = 21; // 该帮派名称已存在

        public const uint N_JOINFACTION_LEVEL = 22;        // 等级小于30级无法加入帮派!
        public const uint N_JOINFACTION_ALREADYIN = 23;    // 您已经加入了帮派
        public const uint N_JOINFACTION_NOEXISTS = 24;         // 该帮派并不存在
        public const uint N_JOINFACTION_MAX = 25;              // 本帮会的人数已经到达上限了
        public const uint N_FACITONNAME_CHANGE = 26;           // 你所在的帮派已经被改名了
        public const uint N_FACTION_DEL = 27;                  // 您所在的帮派已解散
        public const uint N_FACTION_KICK = 28;             // 您已经被帮主开除出帮派了

        public const uint N_FACTION_JOBMAX = 29;               // 职位人数超过上限
        public const uint N_JOINFACTION_TIMELIMIT = 30;    // 您退帮时间不满24小时,不能再次加入帮派！！
        public const uint N_JOINFACITON_TIMELIMIT_EX = 31;     // 对方退帮时间不满24小时,不能再次加入帮派
        public const uint N_JOINFACTION_CAMPLIMIT = 32;        // 只能加入同阵营玩家
        public const uint N_JOINFACTION_SUCCESS = 33;          // 成功加入帮会

        public const uint N_ADDFRIEND_NOTONLINE = 34;          // 对方已经下线了，添加好友失败
        public const uint N_PLAYER_NOTFIND = 35;               // 对方不存在或不在线
        public const uint N_ADDFRIEND_MAPLIMIT = 36;           // 你所在的地图无法进行组队
        public const uint N_ADDFRIEND_MAPLIMIT_EX = 37;        // 对方所在的地图无法进行组队

        public const uint N_CHANGREGION_ERR_WITH_TEMPITEM = 38;// 您身上携带临时物品，无法离开本场景!

        public const uint N_ADDMEMBER_NOFACTION = 39;          // 您未加入任何帮派，无法邀请入帮
        public const uint N_ADDMEMBER_LIMIT = 40;              // 您的权限不够，无法邀请入帮
        public const uint N_ADDMEMBER_NOTFIND = 41;            // 找不到目标玩家。邀请失败
        public const uint N_ADDMEMBER_ALREADYIN = 42;          // 目标玩家已有帮派了。邀请失败！
        public const uint N_ADDMEMBER_WAIT = 43;               // 已经向目标发出邀请。请等待回应！

        public const uint N_CREATEFACTION_ERR = 44;            // 创建帮派出现错误
        public const uint N_JOINFACTION_OFFLINE = 45;          // 邀请您入帮的人已经下线了
        public const uint N_ADDMEMBER_NOACCEPT = 46;           // 对方拒绝了你的邀请
        public const uint N_ADDMEMBER_NOFACTION_EX = 47;       // 邀请加入帮派错误
        public const uint N_JOINFACTION_ERR = 48;              // 加入帮派失败
        public const uint N_DELFACTION_ERR = 49;               // 你不是帮主不能解散帮派
        public const uint N_CHANGENOTIE_ERR = 50;              // 只有帮主才可以修改宣言
        public const uint N_LEAVEFACTION_SUCCESS = 51;         // 你已经正式离开了之前所在的帮派
        public const uint N_DELFACTION_SUCCESS = 52;           // 阁下已经把你的帮派解散了

        public const uint N_CHANGELEADER_CSLIMIT = 53;         // 您已接受多人副本，不能更换队长
        public const uint N_JOINTEAM_WAIT = 54;            // 组队请求已经发出，等待对方回应..
        public const uint N_DELTEAM_CSLIMIT = 55;          // 副本状态不能解散队伍
        public const uint N_JOINTEAM_ALREADYIN = 56;           // 你和对方都加入了队伍
        public const uint N_JOINTEAM_NOTLEADER = 57;           // 对方不是队长,无权允许您加入队伍

        public const uint N_JOINTEAM_NOTLEADER_EX = 58;        // 您不是队长，不能邀请其他队员加入队伍
        public const uint N_JOINTEAM_MAX = 59;                 // 队伍已经满了
        public const uint N_TIMEOUT = 61;                  // 该道具已过期!
        public const uint B_FAIL_UPPINZHI = 95; //坐骑品质提升失败
    }
    public class SABackMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SCRIPT_MESSAGE, SCMPS.EPRO_BACK_MSG);
        public SABackMsg() : base(opCode) { }

        public ushort result_;    // wType
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref result_);
		}
	}

    /**  
	 * 和服务器脚本系统交互的定制消息!
	 */
    public class SSyncTaskDataMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SCRIPT_MESSAGE, SCMPS.EPRO_SYNCTASKDATA);
        public SSyncTaskDataMsg() : base(opCode) { }

        public TBinaryData lua_bin_data_;
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);

            if (lua_bin_data_ == null)
                lua_bin_data_ = new TBinaryData();
            else
                lua_bin_data_.Reset();

            int len = 0;
            ar.Handle(ref len);
            lua_bin_data_.Reserve(len);
            var buff = lua_bin_data_.Buff;
            ar.Handle(ref buff, (short)len);
            lua_bin_data_.Write(null, 0, len);
		}
	}

    // 估计是新手引导使用
    public class SAScriptProcessactionMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SCRIPT_MESSAGE, SCMPS.EPRO_PROCESSACTION);
        public SAScriptProcessactionMsg() : base(opCode) { }

        public string action_;// strAction 这个怎么处理 ? 直接dostring?
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref action_);
		}
	}
}
