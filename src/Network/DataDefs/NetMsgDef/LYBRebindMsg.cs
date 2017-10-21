using Network.Net;

namespace Network.DataDefs.NetMsgDef
{
    public class SARebindMsg :LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REBIND_MESSAGE, SCMPS.EPRO_REBIND_REGION_SERVER);
        public SARebindMsg() : base(opCode) { }
        public uint ip_;    // ip
        public uint port_;  // port
        public uint id_;    // gid
        public uint zID;
        public uint isLogin;
        public LYBStream stream_data_; // streamData

        // InStream
        //public string account;

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref ip_);
            ar.Handle(ref port_);
            ar.Handle(ref id_);
            ar.Handle(ref zID);
            ar.Handle(ref isLogin);

            ar.Handle(ref stream_data_);
        }
    }
    public class SQRebindMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REBIND_MESSAGE, SCMPS.EPRO_REBIND_REGION_SERVER);
        public SQRebindMsg() : base(opCode) { }

        public uint id_;    // gid
        public LYBStream stream_data_;    // streamData SARebindMsg返回的流

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref id_);
            ar.Handle(ref stream_data_);
        }
    }

    public class SACheckRebindMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_CHKREBIND_MESSAGE, SCMPS.EPRO_CHECK_REBIND);
        public SACheckRebindMsg() : base(opCode) { }

        public uint result_;    // result
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref result_);
        }
    }

    /**
	 * 跨服
	 * */
    public class SARebindLoginServerMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REBIND_MESSAGE, SCMPS.EPRO_REBIND_LOGIN_SERVER);
        public SARebindLoginServerMsg() : base(opCode) { }

        public uint id_;            // m_dwGID
        public uint gm_level_;      // m_dwGMLevel
        public uint rebind_type_;   // m_dwType 1:连接本服务器   2:连接跨服服务器　3:提示退出,不重新登录

        //连接跨服服务器时
        public uint ip_;
        public uint port_;
        public uint server_id_;     // serverID

        //连接本服务器时
        public string account_;     // strAccect 帐号
        public uint dyn_passwd_;    // pass 动态密码
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Skip(2);
            ar.Handle(ref id_);
            ar.Handle(ref gm_level_);
            ar.Handle(ref rebind_type_);

            ar.GetStream().Handle(ref ip_);
            ar.GetStream().Handle(ref port_);

			if(rebind_type_ == 2)
            {
                //连接跨服服务器
                ar.GetStream().SkipVariant();
                ar.GetStream().SkipVariant();
                ar.GetStream().Handle(ref server_id_);
			}
            else if(rebind_type_ == 1)
            {
                //连接本服务器时
                ar.GetStream().Handle(ref account_);
                ar.GetStream().Handle(ref dyn_passwd_);
			}
		}
	}
}
