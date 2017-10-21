using Network.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Network.DataDefs
{
    //[Pair("Lua", "Pt2D")]
    public class Pt2D
    {
        public int x_ = 0;
        public int y_ = 0;

        public Pt2D() { }
        public Pt2D(Pt2D src)
        {
            this.Clone(src);
        }

        public void Clone(Pt2D src)
        {
            x_ = src.x_;
            y_ = src.y_;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Coodinate2D(");
            sb.Append("x_: ");
            sb.Append(x_);
            sb.Append(",y_: ");
            sb.Append(y_);
            sb.Append(")");
            return sb.ToString();
        }
    }

    //[Pair("Lua", "CSSyncWay")]
    public class CSSyncWay : ILYBSerializable
    {
        public short lyb_x_;      // m_wSegX 起点
        public short lyb_y_;      // m_wSegY 起点

        // m_wAction 移动类型, 高位:路径长度, 低位:移动类型
        //public ushort move_type_;
        public byte move_type_;
        public byte way_len_;

        public short[] corners_;     // m_track 相对于起点的偏移, 20160924 更改为世界坐标(LYB, 左上为原点, CM)

        public void Serialize(ILYBSerializerBase ar)
        {
            ar.Handle(ref lyb_x_);
            ar.Handle(ref lyb_y_);
            ar.Handle(ref move_type_);
            ar.Handle(ref way_len_);
            ar.Handle(ref corners_, LYBGlobalConsts.MAX_TRACK_LENGTH);
        }

        public bool Compare(CSSyncWay rval)
        {
            if (lyb_x_ != rval.lyb_x_ ||
                lyb_y_ != rval.lyb_y_ ||
                move_type_ != rval.move_type_ ||
                way_len_ != rval.way_len_ ||
                corners_.Length != rval.corners_.Length)
                return false;

            for(int i = 0; i < corners_.Length; ++i)
            {
                if (corners_[i] != rval.corners_[i])
                    return false;
            }
            
            return true;
        }
    }

    public class SCharListData : ILYBSerializable
    {
        ushort version_;    // m_wVersion
        char[] name_;       // m_szName
        char[] tong_name_;  // m_szTongName

        byte prefix_;       // m_charPrefix
        byte prefix2_;      // m_charPrefix2
        ushort level_;      // m_wLevel

        uint sid_;          // m_dwStaticID
        ushort[] equip_ids_;// m_wEquipIndex
        ushort out_equip_id_;//m_wOutEquipIndex

        public void Serialize(ILYBSerializerBase ar)
        {
            ar.Handle(ref version_);
            ar.Handle(ref name_, LYBGlobalConsts.MAX_ROLENAMESIZE);
            ar.Handle(ref tong_name_, LYBGlobalConsts.MAX_TONGNAMESIZE);

            ar.Handle(ref prefix_);
            ar.Handle(ref prefix2_);
            ar.Handle(ref level_);
            ar.Handle(ref sid_);

            ar.Handle(ref equip_ids_, 13);//LYBGlobalConsts.MAX_EQUIP_COUNT
            ar.Handle(ref out_equip_id_);
        }
    }

    public class CreateFixProperty : ILYBSerializable
    {
        public ushort version_;     // m_Version
        public string name_;        // m_name
        public byte prefix_;        // m_charPrefix
        public ushort region_id_;   // m_curRegionID
        public ushort tile_x_;      // m_SegX
        public ushort tile_y_;      // m_SegY
        public byte dir_;           // m_byDir
        public int sid_;            // m_dwStaticID

        public void Serialize(ILYBSerializerBase ar)
        {
            ar.Handle(ref version_);
            ar.Handle(ref name_, LYBGlobalConsts.MAX_ROLENAMESIZE);
            ar.Handle(ref prefix_);

            ar.Handle(ref region_id_);
            ar.Handle(ref tile_x_);
            ar.Handle(ref tile_y_);
            ar.Handle(ref dir_);
            ar.Skip(1);
            ar.Handle(ref sid_);
        }
    }
}