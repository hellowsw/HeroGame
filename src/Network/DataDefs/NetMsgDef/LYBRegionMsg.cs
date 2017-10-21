using Network.Net;

namespace Network.DataDefs.NetMsgDef
{

    public class SASynGIDMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REGION_MESSAGE, SCMPS.EPRO_SYN_AREAGIDS);
        public SASynGIDMsg() : base(opCode) { }

        public ushort num_;	    // num 九宫格对象个数
        public ushort self_num_;// selfNum 当前区域对象个数

        public uint[] ids_;     // gids
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref num_);
            ar.Handle(ref self_num_);

            if (num_ > 1024)
                num_ = 1024;
            if (self_num_ > num_)
                self_num_ = num_;

            ar.Skip(2);
            ar.Handle(ref ids_, (short)num_);
        }
    }

    //请求目标数据
    public class SQObjectInfoMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REGION_MESSAGE, SCMPS.EPRO_OBJECT_INFO);
        public SQObjectInfoMsg() : base(opCode) { }

        public const int GIT_ALL = 0;       //获取对象 SASynGIDMsg下行ID的处理中使用
        public const int GIT_SALENAME = 1;  //获取名称 AS代码中, 受击时攻击对象在本地不存在时使用
        public const int GIT_WAY_TRACK = 2; //获取路径 AS代码中, SASynObjectMsg下行中, 本地对象不存在, 或本地对象wCheckID与下行不一致时使用

        public short request_type_; // wInfoType 无可接任务 wInfoType发送1
        public uint id_;
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref request_type_);
            ar.Handle(ref id_);
        }
    }
    /**
 *通知场景上出现了某个对象 
 */
    public class SASynObjectMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REGION_MESSAGE, SCMPS.EPRO_SYN_OBJECT);
        public SASynObjectMsg() : base(opCode) { }

        public ushort check_id_; //wCheckID
        public ushort curr_tile_x_;//wCurX
        public ushort curr_tile_y_;//wCurY
        public byte state_;//byState
        public uint id_; //dwGID
        public uint extra_;//dwExtra
        public uint extra2_;//dwExtra2

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref check_id_); //wCheckID
            ar.Handle(ref curr_tile_x_);
            ar.Handle(ref curr_tile_y_);
            ar.Handle(ref state_);
            ar.Skip(3);
            ar.Handle(ref id_); //dwGID
            ar.Handle(ref extra_);
            ar.Handle(ref extra2_);
        }
    }
    
    /**  
	 * 场景上某个对象需要出现的效果
	 */
    public class SASetEffectMsg : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REGION_MESSAGE, SCMPS.EPRO_SET_EFFECT);
        public SASetEffectMsg() : base(opCode) { }
        public const int EEFF_LEVELUP = 1;//该玩家升级了
        public const int EEFF_ONEEFFECT = 2;//某人出现特效
        public const int EEFF_COSTUME = 3;//变身

        public byte effect_type_;   // byEffectType
        public uint id_;            // dwOneEffectPlayerGID
        public ushort effect_id_;   // wEffectID / BuffID / EffectID

        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ar.Handle(ref effect_type_);
            ar.Skip(1);
            ar.Handle(ref id_);
            ar.Handle(ref effect_id_);
        }
    }

    /**
	 * 庄园玩家列表
	 * */
    public class SASendDRPlayerList : LYBMsgHeader
    {
        public static ushort opCode = MakeOpCode(SPMPS.EPRO_REGION_MESSAGE, SCMPS.EPRO_SEND_PLAYLIST);
        public SASendDRPlayerList() : base(opCode) { }

        public string[] names_;//names
        public override void Serialize(ILYBSerializerBase ar)
        {
            base.Serialize(ar);
            ushort count = 0;
            ar.Handle(ref count);

            names_ = new string[count];
            for (int i = 0; i < count; ++i)
            {
                ar.GetStream().Handle(ref names_[i]);
            }
        }
    }
}
