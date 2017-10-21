using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCommon
{
    public class InputState
    {
        public static float GetMouseX()
        {
            return Input.GetAxis("MouseX");
        }

        public static float GetMouseY()
        {
            return Input.GetAxis("MouseY");
        }

        public static bool GetTouchDown(int mouse_key)
        {
            return Input.GetMouseButtonDown(mouse_key);
        }

        public static bool GetTouchUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public static Vector2 GetMousePos()
        {
            return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        }

        public static bool GetKeyDown(int i)
        {
            return Input.GetKeyDown((KeyCode)i);
        }

        public static bool GetKeyUp(int i)
        {
            return Input.GetKeyUp((KeyCode)i);
        }

        //是否点击UGUI
        public static bool IsTouchUI()
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
#if UNITY_IPHONE || UNITY_ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (EventSystem.current.IsPointerOverGameObject())
#endif
                    return true;

                else
                    return false;
            }
            return false;
        }

        //是否停留在UI上面
        public static bool IsStayUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return true;
            else
                return false;
        }
    }
}
