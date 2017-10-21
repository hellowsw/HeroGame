using UnityEngine;
using GameCommon;
using UnityEngine.UI;

namespace GameLogic.BYComponent
{
    public class AdjustAspect : MonoBehaviour
    {
        public EAspectPlane plane;
        public bool WEqualH = false;

        void Start()
        {
            Apply();
        }

        public AdjustAspect Apply()
        {
            switch (plane)
            {
                case EAspectPlane.Image:
                    Canvas canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
                    if (WEqualH)
                        CommonLib.AdjustImageWEqualH(GameConst.STANDARD_WIDTH, GameConst.STANDARD_HEIGHT, GetComponent<RectTransform>(), canvas);
                    else
                        CommonLib.AdjustImage(GameConst.STANDARD_WIDTH, GameConst.STANDARD_HEIGHT, GetComponent<RectTransform>(), canvas);
                    break;
                case EAspectPlane.YPlane:
                    CommonLib.AdjustYPlane(GameConst.STANDARD_WIDTH, GameConst.STANDARD_HEIGHT, transform, Camera.main);
                    break;
                case EAspectPlane.ZPlane:
                    if (WEqualH)
                        CommonLib.AdjustBgWEqualH(GameConst.STANDARD_WIDTH, GameConst.STANDARD_HEIGHT, transform);
                    else
                        CommonLib.AdjustBg(GameConst.STANDARD_WIDTH, GameConst.STANDARD_HEIGHT, transform, Camera.main);
                    break;
            }
            return this;
        }

        public AdjustAspect SetPlane(EAspectPlane plane)
        {
            this.plane = plane;
            return this;
        }

        public AdjustAspect SetWEqualH(bool equal)
        {
            this.WEqualH = equal;
            return this;
        }

        public static AdjustAspect Attach(GameObject go, EAspectPlane plane, bool equal)
        {
            AdjustAspect comp = go.GetComponent<AdjustAspect>();
            if (comp == null)
            {
                comp = go.AddComponent<AdjustAspect>();
            }
            return comp.SetPlane(plane).SetWEqualH(equal).Apply();
        }

        public enum EAspectPlane
        {
            Image,
            YPlane,
            ZPlane,
        }
    }

}
