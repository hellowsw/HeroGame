using Network.DataDefs;
using Network.Net;
using System;
using System.Runtime.InteropServices;

namespace Network.DataDefs.NetMsgDef
{
    public class SQLoginMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_LOGIN);
        public SQLoginMsg() : base(opCode) { }

        public ushort version_;			        // wVersion 版本信息
        public string account_;                 // account 账号
        public string passwd_;                  // pwd 密码

        public int login_type = 1;              // loginType 登录类型 1： 普通， 2：跨服

        public ulong device_uid_;               // deviceUID 设备唯一标识
        public uint server_id_;                 // serverID
        public string client_type_ = "0";       // clientType 微端1 网页0

        public uint cross_server_id_;           // crossServerId 跨服时的目标服ID
        public uint crossMid;

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref version_);
            ar.GetStream().Handle(ref account_);
            ar.GetStream().Handle(ref passwd_);

            uint rpc_op_type = 1;   // For HandleRpcOP
            if (login_type == 1)
            {
                ar.GetStream().HandleRpcOP(ref rpc_op_type);
                ar.GetStream().Handle(ref device_uid_);
            }
            else
            {
                ar.GetStream().HandleRpcOP(ref rpc_op_type);
                ar.GetStream().Handle(ref cross_server_id_);
            }

            rpc_op_type = 2;
            ar.GetStream().HandleRpcOP(ref rpc_op_type);
            ar.GetStream().Handle(ref client_type_);

            rpc_op_type = 3;
            ar.GetStream().HandleRpcOP(ref rpc_op_type);
            ar.GetStream().Handle(ref server_id_);

            if (login_type == 2)
            {
                rpc_op_type = 4;
                ar.GetStream().HandleRpcOP(ref rpc_op_type);
                ar.GetStream().Handle(ref crossMid);
            }
        }
    }

    public enum Ret_Login : ushort
    {
        ERC_LOGIN_SUCCESS = 0,              // 登陆成功  
        ERC_INVALID_VERSION = 1,            // 非法的版本号
        ERC_INVALID_ACCOUNT = 2,             // 无效的账号
        ERC_INVALID_PASSWORD = 3,            // 错误的密码
        ERC_LOGIN_ALREADY_LOGIN = 4,         // 此账号已登陆
        ERC_GETLIST_TIMEOUT = 5,             // 获取角色列表超时
        ERC_GETLIST_FAIL = 6,                // 获取角色列表失败
        ERC_CHECKACC_TIMEOUT = 7,           // 账号检测超时
        ERC_SEND_GETCL_TO_DATASRV_FAIL = 8, // 向数据库服务器发送获取列表消息失败
        ERC_SEND_CACC_TO_ACCSRV_FAIL = 9,   // 向账号服务器发送账号检测消息失败
        ERC_ALREADYLOGIN_AND_LINKVALID = 10, // 此账号已登陆，同时相应的连接还未失效
        ERC_ALREADYLOGIN_BUT_INREBIND = 11, // 此账号已登陆，同时相应的连接已失效，但是处于重定向连接中[BUG]
        ERC_NOTENOUGH_CARDPOINT = 12,        // 此账号点数不足
        ERC_SERVER_UPDATE = 13,             // 服务器更新中，暂时不能登陆
        ERC_LOGIN_ERROR = 14,               // 登陆失败
        ERC_CREATE_TEST_CHARACTER = 15,     // 试玩账号，直接通知进入角色创建画面
        ERC_BLOCKED = 16,                   // 账号被停权
        ERC_LIMITED = 17,                   // 登陆时间太短
        ERC_MAXCLIENTS = 18,                // 已经达到连接上限
        ERC_LOCKEDON_SERVERID = 19,          // 此账号数据已经被其他游戏服务器锁定，拒绝重复登陆
        ERC_QUEUEWAIT = 20,                  // 排队等待
        ERC_LOCKEDON_MOBILPHONE = 99,        // 手机锁
    };



    public class SALoginMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_LOGIN);
        public SALoginMsg()
            : base(opCode) { }

        public ushort result_;      // dwRetCode 操作反馈信息
        public ushort gm_level_;    // wGMLevel
        public uint echo_ip_;       // dwEchoIP 回显IP
        public string login_key_;   // loginKey

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref result_);
        }
    }

    //角色列表
    public class SAChListMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_CHARACTER_LIST_INFO);
        public SAChListMsg() : base(opCode) { }

        public SCharListData[] charactors_;    //charListData

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref charactors_, 3);
        }
    }

    public class SQCreatePlayerMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_CREATE_CHARACTER);
        public SQCreatePlayerMsg() : base(opCode) { }

        public byte idx_;   // idx
        public CreateFixProperty player_data_;

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref idx_);
            ar.Skip(1);
            ar.Handle(ref player_data_);
        }
    }

    public enum Ret_CreatePlayer : ushort
    {
        ERC_SUCCESS = 1,
        ERC_INDEXTAKEN = 2,
        ERC_NAMETAKEN = 3,
        ERC_UNHANDLE = 4,
        ERC_EXCEPUTION_NAME = 5,
    };
    public class SACreatePlayerMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_CREATE_CHARACTER);
        public SACreatePlayerMsg() : base(opCode) { }

        public byte result_;    // byResult
        public byte idx_;       // idx
        public CreateFixProperty player_data_;
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref result_);
            ar.Handle(ref idx_);

            if (result_ == (byte)Ret_CreatePlayer.ERC_SUCCESS)
                ar.Handle(ref player_data_);
        }
    }

    public enum Ret_DeletePlayer : ushort
    {
        ERC_SUCCESS = 1,
        ERC_UNHANDLE = 2,
    };
    public class SADeletePlayerMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_DEL_CHARACTER);
        public SADeletePlayerMsg() : base(opCode) { }

        public byte result_;    // retCode
        public uint id_;        // playerId
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref result_);
            ar.Handle(ref id_);
        }
    }

    public enum Ret_SelPlayer
    {
        ERC_SELPLAYER_SUCCESS,  // 登陆成功
        ERC_GETCHDATA_TIMEOUT,  // 向数据库发送获取角色资料消息超时
        ERC_SEND_GETCHDATA_FAIL,// 向数据库发送获取角色资料消息失败
        ERC_GETCHDATA_FAIL,     // 向数据库获取角色资料失败
        ERC_PUTTOREGION_FAIL,   // 将角色放入具体场景失败
        ERC_PLAYER_FULL,        // 该服务器组已经达到人数上限。。。
    }
    public class SASelPlayerMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_SELECT_CHARACTER);
        public SASelPlayerMsg() : base(opCode) { }

        public ushort result_;  //retCode
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref result_);
        }
    }
    
    /**
	 * 防沉迷提示
	 * */
    public class SAPreventIndulge : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_PREVENT_INDULGE);
        public SAPreventIndulge() : base(opCode) { }

        public uint indulge_level_;     // indulgeFlage 沉迷等级吧?
        public uint online_time_;       // online 在线时间
        public uint offline_time_;	    // outTime 离线时间

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref indulge_level_);
            ar.Handle(ref online_time_);
            ar.Handle(ref offline_time_);
		}
	}

    public class SQLogoutMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_SYSTEM_MESSAGE, SCMPS.EPRO_LOGOUT);
        public SQLogoutMsg() : base(opCode) { }

        public const int EST_LOGOUT_NORMAL = 0;
        public const int EST_LOGOUT_HANGUP = 1;

        public uint id_ = 0; // gid
        public byte logout_type_ = 0;   // byLogoutState 登出类型 0 正常 1 登出后离线挂机

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref id_);
            ar.Handle(ref logout_type_);
        }
    }
}
