using Network.DataDefs;
using UnityEngine;
namespace Network.DataDefs.DefsHelper
{
    public class Coodinate2DHelper
    {
        static Pt2D result = new Pt2D();
        public static void Set(Pt2D dest, Vector2 rVal)
        {
            dest.x_ = (int)(rVal.x * 100);
            dest.y_ = (int)(rVal.y * 100);
        }

        public static void Set(Pt2D dest, Vector3 rVal)
        {
            dest.x_ = (int)(rVal.x * 100);
            dest.y_ = (int)(rVal.z * 100);
        }

        public static Pt2D From(Vector2 rVal)
        {
            result.x_ = (int)(rVal.x * 100);
            result.y_ = (int)(rVal.y * 100);
            return result;
        }

        public static Pt2D From(Vector3 rVal)
        {
            result.x_ = (int)(rVal.x * 100);
            result.y_ = (int)(rVal.z * 100);
            return result;
        }

        public static Pt2D Sub(Pt2D lVal, Pt2D rVal)
        {
            result.x_ = lVal.x_ - rVal.x_;
            result.y_ = lVal.y_ - rVal.y_;
            return result;
        }

        public static Vector2 GetVector2(Pt2D val)
        {
            Vector2 ret = default(Vector2);
            ret.x = (float)val.x_ / 100;
            ret.y = (float)val.y_ / 100;
            return ret;
        }

        public static Vector3 GetVector3(Pt2D val)
        {
            Vector3 ret = default(Vector3);
            ret.x = (float)val.x_ / 100;
            ret.z = (float)val.y_ / 100;
            return ret;
        }
    
    }
}
