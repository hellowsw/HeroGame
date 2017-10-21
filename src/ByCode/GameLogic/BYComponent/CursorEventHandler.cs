using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using LuaInterface;

namespace UnityDLL.GameLogic.BYComponent
{
    public class CursorEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,IPointerUpHandler
    {

        public virtual void OnPointerEnter(PointerEventData eventData)
        {

        }

        public virtual void OnPointerExit(PointerEventData eventData) 
        {

        }
    

        public virtual void OnPointerDown(PointerEventData eventData)
        {

        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {

        }
    }

    public class LuaCursorEventHandler : CursorEventHandler
    {
        static Color highColor = new Color(0.1f, 0.1f, 0.1f);
        LuaFunction enterEvent;
        LuaFunction exitEvent;
        private Action enterEvent_;
        private Action exitEvent_;
        Button button;
        Material mat;

        public LuaCursorEventHandler SetEnterEvent(Action func2)
        {
            enterEvent_ = func2;
            return this;
        }

        public LuaCursorEventHandler SetExitEvent(Action func2)
        {
            exitEvent_ = func2;
            return this;
        }

        public LuaCursorEventHandler SetEnterEvent(LuaFunction func)
        {
            enterEvent = func;
            return this;
        }
#if UNITY_ANDROID || UNITY_IPHONE//UNITY_STANDALONE 

        private float pressTime = 0.5f;
        private float cuPressTime = 0;
        private int pressFlag=0; //0默认状态 1按下时 2长按 

        private Vector3 lastPos;

        public override void OnPointerDown(PointerEventData eventData)
        {
            cuPressTime = 0;
            pressFlag = 1;
            lastPos = Input.mousePosition;

        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            pressFlag = 0;
            if (exitEvent_!=null)
            {
                exitEvent_();
            }
            if (exitEvent != null)
            {
                exitEvent.Call();
            }
        }

        private Vector3 mousePos;
        void Update()
        {
            if (pressFlag==2)
            {
                mousePos = Input.mousePosition;

                if (lastPos!=mousePos)
                {
                    if (Mathf.Abs(mousePos.x - lastPos.x) > 20 || Mathf.Abs(mousePos.y - lastPos.y) > 20)
                    {
                        OnPointerUp(null);
                    }
                    lastPos = Input.mousePosition;
                }
             
            }

            if (pressFlag==1)
            {
                if (cuPressTime < pressTime)
                {
                    cuPressTime += Time.deltaTime;
                }
                else
                {
                    pressFlag = 2;
                    if (enterEvent_ != null)
                    {
                        enterEvent_();
                    }
                    if (enterEvent != null)
                    {
                        enterEvent.Call();
                    }
                }

            }


        }

#endif

        public LuaCursorEventHandler SetExitEvent(LuaFunction func)
        {
            exitEvent = func;
            return this;
        }


#if UNITY_STANDALONE//UNITY_ANDROID || UNITY_IPHONE
        private bool isDown;
        private bool isEnter;
        public override void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            if (exitEvent_ != null)
            {
                exitEvent_();
            }
            if (exitEvent != null)
            {
                exitEvent.Call();
            }

        }

        void Update()
        {

            if (Input.GetMouseButtonUp(0))
            {
                isDown = false;
                if (isEnter )
                {
                    if (enterEvent_ != null)
                    {
                        enterEvent_();
                    }
                    if (enterEvent != null)
                    {
                        enterEvent.Call();
                    }

                }
            }

        }

      

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (button != null && button.image != null)
            {
                button.image.material = mat;
            }
            isEnter = true;
            if (isDown)
            {
                return;
            }
            
            base.OnPointerEnter(eventData);
            if (enterEvent_ != null)
            {
                enterEvent_();
            }
            if (enterEvent != null)
            {
                enterEvent.Call();
            }

        }
        

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (button != null && button.image != null)
                button.image.material = null;
            isEnter = false;
            if (isDown)
            {
                return;
            }
            
            base.OnPointerExit(eventData);
            if (exitEvent_ != null)
            {
                exitEvent_();
            }
            if (exitEvent != null)
            {
                exitEvent.Call();
            }

        }

#endif

        public void Init(bool highLight)
        {
            if (!highLight) return;
            button = GetComponent<Button>();
            if (button != null && button.image != null)
            {
                mat = new Material(ResourceManager.LoadResourceFromLocal("shader", "sprite_add") as Shader);
                mat.SetColor("_AddColor", highColor);
                button.transition = Selectable.Transition.None;
            }
        }

        void OnDisable()
        {
            if (exitEvent_ != null)
            {
                exitEvent_();
            }
            if (exitEvent != null)
            {
                exitEvent.Call();
            }
#if UNITY_STANDALONE
            isEnter = false;
            isDown = false;
#endif
            if (button != null && button.image != null)
                button.image.material = null;
        }

        void OnDestroy()
        {
            if (enterEvent != null)
                enterEvent.Dispose();
            if (exitEvent != null)
                exitEvent.Dispose();
            if (mat != null)
                Destroy(mat);
        }
    }
}
