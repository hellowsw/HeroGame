using System;
using System.Collections.Generic;
using Thrift.Protocol;

namespace Network.Serialize
{
    public enum VarParamType
    {
        E_ValType_None,
        E_ValType_bool,
        E_ValType_byte,
        E_ValType_int,
        E_ValType_int64_t,
        E_ValType_float,
        E_ValType_double,
        E_ValType_varstring,
        E_ValType_VarParamList,
        E_ValType_VarParamPair,
    }

    // 转化器
    public struct BaseVariant
    {
        public byte type;
        public long lVal;         //bool / byte / int / int64 / uX
        public double dbVal;       //float /double
        public object obj;         //string / VarParamList / VarParamPair
    }

    public class VarParamList : List<object>
    {
        public void Write(TProtocol protocol)
        {
            protocol.WriteI16((short)Count);
            for (int i = 0; i < Count; ++i)
            {
                VarParam.Write(this[i],protocol);
            }
        }

        public void Read(TProtocol protocol)
        {
            short count = protocol.ReadI16();
            for (short i = 0; i < count; ++i)
            {
                object val = null;
                VarParam.Read(ref val, protocol);
                Add(val);
            }
        }
    }

    public class VarParamPair
    {
        object key_ = null;
        object val_ = null;

        public void Write(TProtocol protocol)
        {
            VarParam.Write(key_, protocol);
            VarParam.Write(val_, protocol);
        }

        public void Read(TProtocol protocol)
        {
            VarParam.Read(ref key_, protocol);
            VarParam.Read(ref val_, protocol);
        }
    }

    public class VarParam
    {
        protected static Type stringType = typeof(string);
        protected static Type boolType = typeof(bool);
        protected static Type byteType = typeof(byte);
        protected static Type sbyteType = typeof(sbyte);
        protected static Type shortType = typeof(short);
        protected static Type ushortType = typeof(ushort);
        protected static Type intType = typeof(int);
        protected static Type uintType = typeof(uint);
        protected static Type Int64Type = typeof(Int64);
        protected static Type UInt64Type = typeof(UInt64);
        protected static Type floatType = typeof(float);
        protected static Type doubleType = typeof(double);

        protected static Type VarParamListType = typeof(VarParamList);
        protected static Type VarParamPairType = typeof(VarParamPair);

        public static sbyte GetVarParamType(object val)
        {
            if (val == null)
                throw new Exception("GetVarParamType val is null");

            Type t = val.GetType();
            if (t == VarParamListType)
                return (sbyte)VarParamType.E_ValType_VarParamList;
            else if (t == VarParamPairType)
                return (sbyte)VarParamType.E_ValType_VarParamPair;
            else if (t == intType ||
                t == uintType ||
                t == shortType ||
                t == ushortType)
                return (sbyte)VarParamType.E_ValType_int;
            else if (
                t == Int64Type ||
                t == UInt64Type)
                return (sbyte)VarParamType.E_ValType_int64_t;
            else if (t == doubleType)
                return (sbyte)VarParamType.E_ValType_double;
            else if (t == floatType)
                return (sbyte)VarParamType.E_ValType_float;
            else if (t == byteType || t == sbyteType)
                return (sbyte)VarParamType.E_ValType_byte;
            else if (t == boolType)
                return (sbyte)VarParamType.E_ValType_bool;
            else if (val is string)
                return (sbyte)VarParamType.E_ValType_varstring;
            else
                throw new Exception("GetVarParamType ValType error.");
        }

        public static void Write(object varVal, TProtocol protocol)
        {
            sbyte type = GetVarParamType(varVal);
            protocol.WriteByte(type);
            switch ((VarParamType)type)
            {
                case VarParamType.E_ValType_int:
                    protocol.WriteI32(Convert.ToInt32(varVal));
                    break;
                case VarParamType.E_ValType_double:
                    protocol.WriteDouble(Convert.ToDouble(varVal));
                    break;
                case VarParamType.E_ValType_float:
                    protocol.WriteDouble(Convert.ToDouble(varVal));
                    break;
                case VarParamType.E_ValType_bool:
                    protocol.WriteBool(Convert.ToBoolean(varVal));
                    break;
                case VarParamType.E_ValType_byte:
                    protocol.WriteByte(Convert.ToSByte(varVal));
                    break;
                case VarParamType.E_ValType_int64_t:
                    protocol.WriteI64(Convert.ToInt64(varVal));
                    break;
                case VarParamType.E_ValType_varstring:
                    protocol.WriteString((string)varVal);
                    break;
                case VarParamType.E_ValType_VarParamList:
                    VarParamList lst = (VarParamList)varVal;
                    lst.Write(protocol);
                    break;
                case VarParamType.E_ValType_VarParamPair:
                    VarParamPair pr = (VarParamPair)varVal;
                    pr.Write(protocol);
                    break;
                default:
                    throw new Exception("VarParam type error" + varVal.GetType().ToString());
            }
        }

        public static void Read(ref object varVal, TProtocol protocol)
        {
            sbyte type = protocol.ReadByte();
            switch ((VarParamType)type)
            {
                case VarParamType.E_ValType_int:
                    varVal = protocol.ReadI32();
                    break;
                case VarParamType.E_ValType_double:
                    varVal = protocol.ReadDouble();
                    break;
                case VarParamType.E_ValType_float:
                    varVal = (float)protocol.ReadDouble();
                    break;
                case VarParamType.E_ValType_bool:
                    varVal = protocol.ReadBool();
                    break;
                case VarParamType.E_ValType_byte:
                    varVal = protocol.ReadByte();
                    break;
                case VarParamType.E_ValType_int64_t:
                    varVal = protocol.ReadI64();
                    break;
                case VarParamType.E_ValType_varstring:
                    varVal = protocol.ReadString();
                    break;
                case VarParamType.E_ValType_VarParamList:
                    VarParamList lst = new VarParamList();
                    lst.Read(protocol);
                    varVal = lst;
                    break;
                case VarParamType.E_ValType_VarParamPair:
                    VarParamPair pr = new VarParamPair();
                    pr.Read(protocol);
                    varVal = pr;
                    break;
                default:
                    throw new Exception("VarParam type error" + varVal.GetType().ToString());
            }
        }

        public static void WriteArray(object[] varValArray, TProtocol protocol)
        {
            short count = (short)varValArray.Length;
            protocol.WriteI16(count);
            for (short i = 0; i < count; ++i)
            {
                VarParam.Write(varValArray[i], protocol);
            }
        }

        public static void ReadArray(ref object[] varValArray, TProtocol protocol)
        {
            short count = protocol.ReadI16();
            varValArray = new object[count];
            for (short i = 0; i < count; ++i)
            {
                object val = null;
                VarParam.Read(ref val, protocol);
                varValArray[i] = val;
            }
        }
        public static void ReadArrayReverse(ref object[] varValArray, TProtocol protocol)
        {
            short count = protocol.ReadI16();
            varValArray = new object[count];
            for (short i = count; i > 0; --i)
            {
                object val = null;
                VarParam.Read(ref val, protocol);
                varValArray[i - 1] = val;
            }
        }
    }
}
