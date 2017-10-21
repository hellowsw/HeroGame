using GameLogic;
using GameLogic.BYComponent;
using LuaInterface;
using Manager;
using Network.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityDLL;
using UnityDLL.GameLogic.BYComponent;
using UnityEngine;
using UnityEngine.UI;
using UObject = UnityEngine.Object;

namespace GameCommon
{
    public class CommonLib
    {
        public static string GetGBK(TBinaryData b)
        {
            string s = Encoding.GetEncoding("GBK").GetString(b.Buff);

            return s;
        }

        public static string RegxStr(string str, string regx, string replace)
        {
            Regex r = new Regex(regx);
            if (string.IsNullOrEmpty(replace))
            {
                str = r.Replace(str, "");
            }
            else
            {
                str = r.Replace(str, replace);
            }

            return str;

        }

        public static void SetSliderChanged(GameObject go, LuaFunction lf)
        {
            Slider s = go.GetComponent<Slider>();
            if (s != null)
            {
                s.onValueChanged.AddListener((x) =>
                {
                    if (lf != null)
                    {
                        lf.Call(x);
                    }
                });
            }
        }

        public static string GetUTF8(TBinaryData b)
        {
            string s = Encoding.UTF8.GetString(b.Buff);
            return s;
        }
        static string[] animClips = { "shot", "move" };

        static string[] animStates = { "Present", "Death", "Hit" };

        static string[] matParams =
        {
            "_AddColor", "_AddTexColor", "_Alpha", "_RimWidth", "_RimColor",
            "_FlowLightIntensity", "_Speed"
        };

        static string[] matTextures = { string.Empty, "_MainTex", "_FlowLightTex", "_AddTex" };

        static string[] shaders =
        {
            string.Empty, "selfluminous", "flowlight3d", "rim_high_light",
            "rim_high_light_and_flow", "flowlight3d_alpha_blend", "multi_tex_and_flow_rim"
        };

        public static bool isPrepareQuit = false;
        private static Material greyMat;

        public static Material GreyMaterial
        {
            get
            {

                if (greyMat == null)
                {
                    greyMat = new Material(ResourceManager.LoadResourceFromLocal("shader", "sprite_grey") as Shader);
                    greyMat.hideFlags = HideFlags.HideAndDontSave;
                }
                return greyMat;
            }
        }


        private static Material greyTextMat;

        public static Material GreyTextMaterial
        {
            get
            {
                if (greyTextMat == null)
                {
                    greyTextMat = new Material(ResourceManager.LoadResourceFromLocal("shader", "Font") as Shader);
                    greyTextMat.hideFlags = HideFlags.HideAndDontSave;
                }
                return greyTextMat;
            }
        }


        private static Camera MainCamera
        {
            get
            {
                if (GameApp.Instance == null)
                    return Camera.main;
                return GameApp.Instance.MainCamera;
            }
        }

        private static Vector2 tempVector2;
        private static Vector3 tempVector3;
        private static Vector4 tempVector4;
        private static Color tempColor;

        //得到屏幕坐标
        public static Vector2 GetScreenPos(float worldPosx, float worldPosy, float worldPosz)
        {
            tempVector3.x = worldPosx;
            tempVector3.y = worldPosy;
            tempVector3.z = worldPosz;
            Vector3 pos = MainCamera.WorldToScreenPoint(tempVector3);
            return new Vector2(pos.x, Screen.height - pos.y);
        }

        public static void SetDropDownOnChange(Dropdown dd, LuaFunction lf)
        {
            if (dd != null)
            {
                dd.onValueChanged.RemoveAllListeners();
                dd.onValueChanged.AddListener((x) =>
                {

                    if (lf != null)
                    {
                        lf.Call(x);
                    }
                });
            }

        }

        public static string[] validate =
        {
            "^(\\d{3.4}-)\\d{7,8}$",
            "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$"
        };

        public static bool Validate(string d, string rex)
        {
            return new Regex(rex).IsMatch(d);
        }

        /// <summary>
        /// 验证聊天输入框
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool ValidateChatInput(string d)
        {
            string dd = "[\\^\\'\\\"<\\[	]";
            //  string dd = "[ 1]";
            Match m = new Regex(dd, RegexOptions.IgnoreCase).Match(d);

            if (m.Value != String.Empty)
            {
                Debug.Log("匹配到:" + m.Value);
                return true;
            }
            return false;
        }

        public static string Replace(string t, string old, string new_)
        {
            return t.Replace(old, new_);
        }

        /// <summary>
        /// 1 姓名 2 手机号码 3详细地址  4邮政编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ValidateAddress(int type, string s)
        {
            string t1 = "^([\u4e00-\u9fa5]+|([a-zA-Z]+\\s?)+)$";
            string t2 = "^1[0-9]{10}$";
            string t3 = "^(?=.*?[\u4E00-\u9FA5])[\\d\u4E00-\u9FA5]+";
            string t4 = "^[0-9]\\d{5}$";


            if (type == 1)
            {
                return Regex.IsMatch(s, t1, RegexOptions.IgnoreCase);
            }
            else if (type == 2)
            {
                return Regex.IsMatch(s, t2, RegexOptions.IgnoreCase);
            }
            else if (type == 3)
            {
                return s.Trim() != "";
            }
            else if (type == 4)
            {
                return Regex.IsMatch(s, t4, RegexOptions.IgnoreCase);
            }
            return false;
        }


        /// <summary>
        /// 验证昵称输入框
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool ValidateChangeNameInput(string d)
        {
            string dd = "[\\^* #?%;:/\\'\\\"\\\\>< 　\\]	]";
            //  string dd = "[ 1]";
            Match m = new Regex(dd, RegexOptions.IgnoreCase).Match(d);
            Debug.Log("匹配到:" + m.Value);
            if (m.Value != String.Empty)
            {
                return true;
            }
            return false;
        }

        //将像素单位转换为世界单位
        public static float PixelToView(float pixel, float standardHeight, Camera camera)
        {
            return pixel / (standardHeight / (camera.orthographicSize * 2));
        }

        public static float PixelToViewFactor(float standardHeight, Camera camera)
        {
            return 1 / (standardHeight / (camera.orthographicSize * 2));
        }

        //将世界单位转换为像素单位
        public static float ViewToPixel(float view, float standardHeight, Camera camera)
        {
            return view * (standardHeight / (camera.orthographicSize * 2));
        }

        public static float ViewToPixelFactor(float standardHeight, Camera camera)
        {
            return (standardHeight / (camera.orthographicSize * 2));
        }

        public static void WorldPosToFlashWorldPos(float worldX, float worldY, float worldZ, out float oX, out float oY,
            out float oZ)
        {
            Vector3 pos = GetScreenPos(worldX, worldY, worldZ);
            oX = pos.x - Screen.width * 0.5f;
            oY = -(pos.y - Screen.height * 0.5f);
            oZ = pos.z;
        }

        public static Vector3 ScreenPointToLocalPointInRectangle(RectTransform tr, Vector3 pos2, Camera ca)
        {
            Vector2 pos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(tr, pos2, ca, out pos);
            return pos;
        }

        public static void SetMousePos(Transform t, Camera cm)
        {
            RectTransform tt = (RectTransform)t;
            tt.anchoredPosition = ScreenPointToLocalPointInRectangle((RectTransform)t.parent, Input.mousePosition, cm);
        }

        //Flash世界坐标转Unity世界坐标
        public static Vector3 FlashWorldPosToWorldPos(float x, float y, Camera camera, int bgLayer)
        {
            Vector3 unityPos = new Vector3();
            unityPos.x = x + Screen.width * 0.5f;
            unityPos.y = -y + Screen.height * 0.5f;
            unityPos = ScreenToWorld(unityPos, camera, bgLayer);
            unityPos.z = -unityPos.z;
            return unityPos;
        }


        public static void Translate(Transform transform, Vector3 v, int space)
        {
            transform.Translate(v, (Space)space);
        }

        public static void SetRectSize(Transform t, float width, float height)
        {

            RectTransform rt = t as RectTransform;

            rt.anchoredPosition = new Vector2(width, height);


        }

        public static void SetScrollbarValue(Scrollbar s, float v)
        {
            if (s != null)
            {
                s.value = v;
            }
        }

        public static void AddScrollbarValue(Scrollbar s, float v)
        {
            if (s != null)
            {
                s.value += v;
            }
        }

        public static void SubScrollbarValue(Scrollbar s, float v)
        {

            if (s != null)
            {
                s.value -= v;
            }
        }

        public static void SetSliderValue(Slider s, float v)
        {
            if (s != null)
            {
                s.value = v;
                Debug.Log("ssssssssssssssssssssssssssssssssssss");
            }
        }

        public static void AddSliderbarValue(Slider s, float v)
        {
            if (s != null)
            {
                s.value += v;
            }
        }

        public static string ConvertRichText(string t)
        {

            try
            {
                string tt = string.Format("<html>{0}</html>", t);
                Debug.Log(tt);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(tt);

                StringBuilder s = new StringBuilder();
                foreach (var VARIABLE in xd.ChildNodes)
                {
                    XmlElement e = (XmlElement)VARIABLE;
                    if (e.Name == "html")
                    {
                        foreach (var v2 in e.ChildNodes)
                        {

                            if (v2.GetType() == typeof(XmlText))
                            {
                                XmlText tex = (XmlText)v2;
                                s.Append(tex.InnerText);
                            }
                            else
                            {
                                XmlElement e2 = (XmlElement)v2;

                                if (e2.Name == "font")
                                {

                                    foreach (var ve in e2.ChildNodes)
                                    {
                                        XmlLinkedNode xl = ve as XmlLinkedNode;
                                        //Debug.Log(xl.InnerText);

                                        if (e2.HasAttribute("color")) s.AppendFormat("<color=\"{0}\">", e2.GetAttribute("color"));
                                        if (xl.Name == "b")
                                        {
                                            s.AppendFormat("<b>{0}</b>", xl.InnerText);
                                        }
                                        else
                                        {
                                            s.Append(xl.InnerText);
                                        }
                                        if (e2.HasAttribute("color")) s.Append("</color>");
                                    }


                                }
                            }


                        }
                    }
                }


                return s.ToString();
            }
            catch (Exception e)
            {

                Debug.LogException(e);
            }

            return t;
        }

        public static void SubSliderbarValue(Slider s, float v)
        {
            if (s != null)
            {
                s.value -= v;
            }
        }

        //得到两Vector3向量的夹角，忽略Y分量
        public static float Vector3AngleIgnoreY(Vector3 from, Vector3 to)
        {
            float angle = Vector2.Angle(new Vector2(from.x, from.z), new Vector2(to.x, to.z));
            if (to.x < from.x)
                angle = -angle;
            return angle;
        }

        //随机在YZ平面的圆形内生成一个点
        public static Vector3 RandomPos()
        {
            Vector3 pos = UnityEngine.Random.insideUnitCircle;
            pos.z = pos.y;
            pos.y = 0;
            return pos;
        }

        //根据时间蹉获取时间 返回格式化时间
        public static DateTime GetTimeByUnixTimestamp(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        //根据时间蹉获取时间 返回格式化时间
        public static string GetFormatTimeByUnixTimestamp(string timeStamp, string format)
        {
            return GetTimeByUnixTimestamp(timeStamp).ToString(format);
        }

        public static string GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime d = dtStart.Add(toNow);
            Dropdown dd;
            //   return d.Hour+":"+d.Minute+":"+d.Second;
            return d.ToString("hh:mm:ss");
        }

        public static List<string> ToStringList(object[] s)
        {
            List<string> ss = new List<string>();
            for (int i = 0; i < s.Length; i++)
            {
                ss.Add(s[i].ToString());
            }

            return ss;

        }


        public static string SubNowTimeToString(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            TimeSpan t = dtStart.Add(toNow).Subtract(GetTimeByUnixTimestamp(GameServer._GetServerTime().ToString()));

            //   return d.Hour+":"+d.Minute+":"+d.Second;
            string h = t.Hours.ToString();
            string m = t.Minutes.ToString();
            string s = t.Seconds.ToString();

            if (h.Length == 1)
            {
                h = "0" + h;
            }
            if (m.Length == 1)
            {
                m = "0" + m;
            }
            if (s.Length == 1)
            {
                s = "0" + s;
            }

            return string.Format("{0}:{1}", m, s);
        }
        /// <summary>
        /// 小于零
        ///此实例早于 value。零
        ///此实例与 value 相同。
        ///大于零
        ///此实例晚于 value
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static int NowTimeEqules(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime t = dtStart.Add(toNow);
            DateTime d = GetTimeByUnixTimestamp(GameServer._GetServerTime().ToString());
            return d.CompareTo(t);

        }

        //static float oneDaySeconds = 1 / 86400;
        //static float oneHourSeconds = 1 / 3600;
        //static float oneMinuteSeconds = 1 / 60;

        public static string GetFormatTime(int second_count)
        {
            StringBuilder sb = new StringBuilder();

            int day = second_count / 86400;
            int hour = (second_count % 86400) / 3600;
            int minute = second_count % 3600 / 60;
            int second = second_count % 60;
            if (day > 0) sb.Append(day + ":");
            if (hour > 0)
            {
                if (hour < 10) sb.Append("0" + hour);
                else sb.Append(hour);
                sb.Append(":");
            }
            if (minute < 10) sb.Append("0" + minute);
            else sb.Append(minute);
            sb.Append(":");
            if (second < 10) sb.Append("0" + second);
            else sb.Append(second);

            return sb.ToString();
        }

        //减去当前的时间
        public static int SubNowTime(DateTime dt)
        {

            return dt.Subtract(GetTimeByUnixTimestamp(GameServer._GetServerTime().ToString())).Days;
        }

        //得到点击在背景上的位置
        public static Vector3 GetInputPosInBg(Camera camera, int bgLayer)
        {
            return ScreenToWorld(Input.mousePosition, camera, bgLayer);
        }

        static Vector3 inputPos;

        //得到点击在背景上的位置
        public static void GetInputPosInBg(int bgLayer, out float x, out float y, out float z)
        {
            inputPos = ScreenToWorld(Input.mousePosition, MainCamera, bgLayer);
            x = inputPos.x;
            y = inputPos.y;
            z = inputPos.z;
        }

        //得到点击在背景上的位置
        public static bool IsStayInBg(Camera camera, int bgLayer)
        {
            LayerMask layerMask = 1 << bgLayer;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask.value))
            {
                if (hit.transform.tag == "Background")
                {
                    return true;
                }
            }
            return false;
        }

        //UGUI坐标投影到世界坐标(以背景为平面的交点)
        public static Vector3 UIToWorld(Vector3 uiPos, Camera camera, Camera uiCamera, int bgLayer)
        {
            Vector3 screen = uiCamera.WorldToScreenPoint(uiPos);
            return ScreenToWorld(screen, camera, bgLayer);
        }

        //屏幕坐标投影到世界坐标(以背景为平面的交点)
        public static Vector3 ScreenToWorld(Vector3 screenPos, Camera camera, int bgLayer)
        {
            LayerMask layerMask = 1 << bgLayer;
            Ray ray = camera.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask.value))
            {
                if (hit.transform.tag == "Background")
                {
                    return new Vector3(hit.point.x, 0, hit.point.z);
                }
            }
            return Vector3.zero;
        }

        public static void PlayAnim(int transformId, int animatorId, int stateId, bool bValue)
        {
            ObjectManager.GetComponent<Animator>(transformId, animatorId).SetBool(animStates[stateId], bValue);
        }

        public static void PlayAnim(int transformId, int animatorId, int stateId, float fValue)
        {
            ObjectManager.GetComponent<Animator>(transformId, animatorId).SetFloat(animStates[stateId], fValue);
        }

        public static void PlayAnim(int transformId, int animatorId, int stateId)
        {
            ObjectManager.GetComponent<Animator>(transformId, animatorId).SetTrigger(animStates[stateId]);
        }

        public static void SetAnimSpeed(int transformId, int animatorId, float speed)
        {
            ObjectManager.GetComponent<Animator>(transformId, animatorId).speed = speed;
        }

        public static void PlayAnimFromStart(int transformId, int animatorId, int animId)
        {
            ObjectManager.GetComponent<Animator>(transformId, animatorId).Play(animClips[animId], 0, 0);
        }

        //得到子物体中的组件
        public static Component GetComponentInChildren(Transform trans, string name)
        {
            Component component = null;
            foreach (Transform t in trans.GetComponentsInChildren<Transform>())
            {
                if ((component = t.GetComponent(name)) != null)
                    return component;
            }
            return null;
        }

        //得到子物体中的组件
        public static Component[] GetComponentsInChildren(Transform trans, string name)
        {
            List<Component> components = new List<Component>();
            Component component = null;
            foreach (Transform t in trans.GetComponentsInChildren<Transform>())
            {
                if ((component = t.GetComponent(name)) != null)
                {
                    components.Add(component);
                }
            }
            return components.ToArray();
        }

        public static void SetMaterial(Renderer renderer, Material mat)
        {
            renderer.material = mat;
        }

        public static void SetMaterial(int transformId, int rendererId, Material mat)
        {
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material = mat;
        }

        public static void SetShader(Renderer renderer, string shaderName)
        {
            Shader shader = ResourceManager.LoadResourceFromLocal("shader", shaderName) as Shader;
            renderer.material.shader = shader;
        }

        public static void SetShader(int transformId, int rendererId, int shaderId)
        {
            Shader shader = ResourceManager.LoadResourceFromLocal("shader", shaders[shaderId]) as Shader;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.shader = shader;
        }

        public static Material CreateMaterial(Shader shader)
        {
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            return mat;
        }

        public static Material GetMaterial(int transformId, int rendererId)
        {
            return ObjectManager.GetComponent<Renderer>(transformId, rendererId).material;
        }

        public static void SetMaterialFloat(int transformId, int rendererId, int paramId, float value)
        {
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetFloat(matParams[paramId], value);
        }

        public static void SetMaterialColor(int transformId, int rendererId, int paramId, float r, float g, float b, float a)
        {
            tempColor.r = r;
            tempColor.g = g;
            tempColor.b = b;
            tempColor.a = a;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetColor(matParams[paramId], tempColor);
        }

        public static void SetMaterialVector(int transformId, int rendererId, int paramId, float x, float y, float z, float w)
        {
            tempVector4.x = x; tempVector4.y = y; tempVector4.z = z; tempVector4.w = w;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetVector(matParams[paramId], tempVector4);
        }

        public static void SetMaterialTexture(int transformId, int rendererId, int texId, Texture texture)
        {
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetTexture(matTextures[texId], texture);
        }

        public static void SetMaterialTextureScale(int transformId, int rendererId, int texId, float scalex, float scaley)
        {
            tempVector2.x = scalex; tempVector2.y = scaley;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetTextureScale(matTextures[texId], tempVector2);
        }

        public static void SetMaterialTextureOffset(int transformId, int rendererId, int texId, float offsetx, float offsety)
        {
            tempVector2.x = offsetx; tempVector2.y = offsety;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.SetTextureOffset(matTextures[texId], tempVector2);
        }

        public static void SetMaterialFloat(Material mat, string paramName, float value)
        {
            mat.SetFloat(paramName, value);
        }

        public static void SetMaterialColor(Material mat, string paramName, Color color)
        {
            mat.SetColor(paramName, color);
        }

        public static void SetMaterialVector(Material mat, string paramName, Vector4 vector)
        {
            mat.SetVector(paramName, vector);
        }

        public static void SetMaterialTexture(Material mat, string paramName, Texture texture)
        {
            mat.SetTexture(paramName, texture);
        }

        public static void SetMaterialTextureScale(Material mat, string paramName, Vector2 scale)
        {
            mat.SetTextureScale(paramName, scale);
        }

        public static void SetMaterialTextureOffset(Material mat, string paramName, Vector2 offset)
        {
            mat.SetTextureOffset(paramName, offset);
        }

        //删除Component
        public static void RemoveComponent(GameObject go, int index)
        {
            Component[] coms = go.GetComponents<Component>();
            if (coms.Length > index)
            {
                GameObject.Destroy(coms[index]);
            }
        }

        //得到屏幕Rect 下标:0--左 1--右 2--上 3--下
        //public static float[] GetScreenRect(Camera camera)
        //{
        //    float[] rect = new float[4];
        //    rect[0] = -camera.aspect * camera.orthographicSize;
        //    rect[1] = -rect[0];
        //    rect[2] = camera.orthographicSize;
        //    rect[3] = -rect[2];
        //    return rect;
        //}

        public static void GetScreenRect(out float left, out float right, out float up, out float down)
        {
            left = -MainCamera.aspect * MainCamera.orthographicSize;
            right = -left;
            up = MainCamera.orthographicSize;
            down = -up;
        }

        public static void SetActive(GameObject go, int active)
        {
            go.SetActive(active == 1);
        }

        public static void SetActiveB(GameObject go, bool active)
        {
            go.SetActive(active);
        }

        //设置Y轴的旋转,根据向量
        public static void SetDir(Transform trans, float x, float y, float z)
        {
            Vector3 euler = new Vector3(0, Vector3AngleIgnoreY(new Vector3(0, 0, 1), new Vector3(x, y, z)), 0);
            Quaternion quat = Quaternion.Euler(euler);
            quat.x = 0;
            quat.z = 0;
            trans.localRotation = quat;
        }

        public static void SetDir(int transformId, int childId, float x, float y, float z)
        {
            Vector3 euler = new Vector3(0, Vector3AngleIgnoreY(new Vector3(0, 0, 1), new Vector3(x, y, z)), 0);
            Quaternion quat = Quaternion.Euler(euler);
            quat.x = 0;
            quat.z = 0;
            ObjectManager.SetLocalRotation(transformId, childId, quat.x, quat.y, quat.z, quat.w);
        }

        //添加碰撞盒
        public static BoxCollider AttachCollider(SkinnedMeshRenderer renderer)
        {
            BoxCollider collider = null;
            if ((collider = renderer.gameObject.GetComponent<BoxCollider>()) != null)
                return collider;
            collider = renderer.gameObject.AddComponent<BoxCollider>();
            //collider.bounds.SetMinMax(collider.bounds.min * 0.7f, collider.bounds.max * 0.7f);
            return collider;
        }

        //添加AudioSource
        public static AudioSource AttachAudioSource(GameObject go, bool playOnAwake, bool loop)
        {
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = playOnAwake;
            audioSource.loop = loop;
            return audioSource;
        }

        public static LuaCursorEventHandler AttachCursorEventHandler(GameObject go, int style, bool highLight)
        {
            LuaCursorEventHandler com = null;
            if ((com = go.GetComponent<LuaCursorEventHandler>()) == null)
            {
                com = go.AddComponent<LuaCursorEventHandler>();
                com.Init(highLight);
            }
            else
            {
                com.Init(highLight);
            }
            return com;
        }

        public static ButtonSleep AttachButtonSleep(GameObject go)
        {
            return AttachButtonSleep(go, 0.5f);
        }
        public static ToggleSleep AttachToggleSleep(GameObject go, float duration)
        {
            ToggleSleep com = null;
            if ((com = go.GetComponent<ToggleSleep>()) == null)
            {
                com = go.AddComponent<ToggleSleep>();
            }
            com.duration = duration;
            return com;
        }
        public static ButtonSleep AttachButtonSleep(GameObject go, float duration)
        {
            ButtonSleep com = null;
            if ((com = go.GetComponent<ButtonSleep>()) == null)
            {
                com = go.AddComponent<ButtonSleep>();
            }
            com.duration = duration;
            return com;
        }

        public static void PlayAudio(AudioSource audioSource, AudioClip audioClip, bool oneShot)
        {
            if (oneShot)
            {
                audioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public static void PauseAudio(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        public static void UnPauseAudio(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public static void StopAudio(AudioSource audioSource)
        {
            audioSource.Stop();
        }

        public static float GetAudioClipLength(AudioClip clip)
        {
            return clip.length;
        }

        //检测碰撞 从(x,y,z)的位置根据radius检测是否碰撞到collider
        public static bool IsCollision(BoxCollider collider, Vector3 pos, Vector3 dir, float distance, int layer)
        {
            Ray ray = new Ray(pos, dir);
            float d;
            if (collider.bounds.IntersectRay(ray, out d))
            {
                if (d <= distance)
                    return true;
            }

            return false;
        }

        public static bool IsCollision(int transformId, int colliderId, float posx, float posy, float posz, float halfWidth, int halfHeight)
        {
            return ObjectManager.GetComponent<BoxCollider>(transformId, colliderId).bounds.Intersects(new Bounds(new Vector3(posx, posy, posz), new Vector3(halfWidth, 1, halfHeight)));
        }

        //检测碰撞 从(x,y,z)的位置根据radius检测是否碰撞到collider
        //public static bool IsCollision(BoxCollider collider, Vector3 pos)
        //{
        //    return collider.bounds.Contains(pos);
        //}

        public static bool IsCollision(Bounds bounds, Vector3 pos)
        {
            return bounds.Contains(pos);
        }

        public static void GetColliderSize(int transformId, int colliderId, out float x, out float y, out float z)
        {
            Bounds bounds = ObjectManager.GetComponent<BoxCollider>(transformId, colliderId).bounds;
            x = bounds.size.x;
            y = bounds.size.y;
            z = bounds.size.z;
        }

        //检测碰撞
        //public static bool IsCollision(BoxCollider collider, Vector3 pos, float hWidth, float hHeight)
        //{
        //    Bounds bounds = new Bounds(pos, new Vector3(hWidth, 1, hHeight));
        //    if (collider.bounds.Intersects(bounds))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //检测碰撞
        public static bool IsCollision(Bounds bounds, Vector3 pos, float hWidth, float hHeight)
        {
            return bounds.Intersects(new Bounds(pos, new Vector3(hWidth, 1, hHeight)));
        }

        static float r1_l;
        static float r1_r;
        static float r1_t;
        static float r1_b;
        static float r2_l;
        static float r2_r;
        static float r2_t;
        static float r2_b;
        static float scaleSizeX;
        static float scaleSizeY;

        public static bool IsCollisionRect(BoxCollider collider, Transform trans, float width, float height, float scaleX, float scaleY)
        {
            scaleSizeX = collider.bounds.size.x * scaleX;
            scaleSizeY = collider.bounds.size.z * scaleY;
            r1_l = collider.bounds.center.x - scaleSizeX;
            r1_r = collider.bounds.center.x + scaleSizeX;
            r1_t = collider.bounds.center.z + scaleSizeY;
            r1_b = collider.bounds.center.z - scaleSizeY;
            r2_l = trans.position.x - width;
            r2_r = trans.position.x + width;
            r2_t = trans.position.z + height;
            r2_b = trans.position.z - height;
            return !(r1_l > r2_r || r1_r < r2_l || r1_t < r2_b || r1_b > r2_t);
        }

        public static bool IsCollisionRect(int transformId, int colliderId, float width, float height, float scaleX, float scaleY)
        {
            return IsCollisionRect(ObjectManager.GetComponent<BoxCollider>(transformId, colliderId), ObjectManager.Get(transformId), width, height, scaleX, scaleY);
        }

        public static bool IsCollisionRect(int transformId, int colliderId, float x, float z, float width, float height, float scaleX, float scaleY)
        {
            BoxCollider collider = ObjectManager.GetComponent<BoxCollider>(transformId, colliderId);
            scaleSizeX = collider.bounds.size.x * scaleX;
            scaleSizeY = collider.bounds.size.z * scaleY;
            r1_l = collider.bounds.center.x - scaleSizeX;
            r1_r = collider.bounds.center.x + scaleSizeX;
            r1_t = collider.bounds.center.z + scaleSizeY;
            r1_b = collider.bounds.center.z - scaleSizeY;
            r2_l = x - width;
            r2_r = x + width;
            r2_t = z + height;
            r2_b = z - height;
            return !(r1_l > r2_r || r1_r < r2_l || r1_t < r2_b || r1_b > r2_t);
        }

        public static bool IsCollisionPoint(int transformId, int colliderId, float pointx, float pointy, float pointz)
        {
            BoxCollider collider = ObjectManager.GetComponent<BoxCollider>(transformId, colliderId);
            return collider.bounds.Contains(new Vector3(pointx, collider.bounds.center.y, pointz));
        }

        public static void AddOutLine(Text t)
        {
            var i = UObject.Instantiate(t.gameObject, t.transform) as GameObject;
            var i2 = i.GetComponent<Text>();
            if (i2.alignment == TextAnchor.UpperLeft)
            {
                i2.alignment = TextAnchor.LowerLeft;
            }
            else if (i2.alignment == TextAnchor.UpperCenter)
            {
                i2.alignment = TextAnchor.LowerCenter;
            }
            else if (i2.alignment == TextAnchor.UpperRight)
            {
                i2.alignment = TextAnchor.LowerRight;
            }
            else if (i2.alignment == TextAnchor.MiddleLeft)
            {
                i2.alignment = TextAnchor.LowerLeft;
            }
            else if (i2.alignment == TextAnchor.MiddleCenter)
            {
                i2.alignment = TextAnchor.LowerCenter;
            }
            else if (i2.alignment == TextAnchor.MiddleRight)
            {
                i2.alignment = TextAnchor.LowerRight;
            }
            i2.raycastTarget = false;
            i2.text = "_";
            int i3 = (int)(t.preferredWidth / i2.preferredWidth);
            for (int j = 1; j < i3; j++)
            {
                i2.text += "_";
            }

        }

        public static bool ScreenToWorldRaycast(BoxCollider collider, Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (collider.bounds.IntersectRay(ray))
            {
                return true;
            }
            return false;
        }

        public static bool ScreenToWorldRaycast(int transformId, int colliderId)
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (ObjectManager.GetComponent<BoxCollider>(transformId, colliderId).bounds.IntersectRay(ray))
            {
                return true;
            }
            return false;
        }

        //设置颜色
        public static void SetColor(Renderer renderer, Color color)
        {
            renderer.material.color = color;
        }

        public static void SetColor(int transformId, int rendererId, float r, float g, float b, float a)
        {
            tempColor.r = r;
            tempColor.g = g;
            tempColor.b = b;
            tempColor.a = a;
            ObjectManager.GetComponent<Renderer>(transformId, rendererId).material.color = tempColor;
        }

        //设置Image颜色
        public static void SetColor(Image image, Color color)
        {
            image.color = color;
        }

        //设置Image颜色
        public static void SetImageAlpha(int transformId, int imgId, float a)
        {
            Image img = ObjectManager.GetComponent<Image>(transformId, imgId);
            img.color = new Color(img.color.r, img.color.g, img.color.b, a);
        }

        //设置Sprite
        public static void SetSprite(SpriteRenderer renderer, Sprite sprite)
        {
            renderer.sprite = sprite;
        }

        public static void SetSprite(int transformId, int rendererId, Sprite sprite)
        {
            ObjectManager.GetComponent<SpriteRenderer>(transformId, rendererId).sprite = sprite;
        }

        //设置Sprite
        public static void SetSpriteObj(SpriteRenderer renderer, UnityEngine.Object sprite)
        {
            renderer.sprite = Texture2DToSprite(sprite as Texture2D);
        }

        public static void SetSpriteObji(int transformId, int rendererId, UnityEngine.Object sprite)
        {
            ObjectManager.GetComponent<SpriteRenderer>(transformId, rendererId).sprite = Texture2DToSprite(sprite as Texture2D);
        }

        //设置文字
        public static void SetText(TextMesh mesh, string text)
        {
            mesh.text = text;
        }

        public static void SetText(int transformId, int textMeshId, string text)
        {
            ObjectManager.GetComponent<TextMesh>(transformId, textMeshId).text = text;
        }

        public static void SetUIText(int transformId, int textMeshId, string text)
        {
            ObjectManager.GetComponent<Text>(transformId, textMeshId).text = text;
        }

        public static void SetUITextColor(int transformId, int textMeshId, float r, float g, float b, float a)
        {
            ObjectManager.GetComponent<Text>(transformId, textMeshId).color = new Color(r, g, b, a);
        }

        //设置字體
        public static void SetFont(TextMesh mesh, Font font)
        {
            mesh.font = font;
        }

        public static void SetFont(int transformId, int textMeshId, Font font)
        {
            ObjectManager.GetComponent<TextMesh>(transformId, textMeshId).font = font;
        }

        public static void SetLayer(GameObject go, int layer)
        {
            DoSetLayer(go, layer);
        }

        private static void DoSetLayer(GameObject go, int layer)
        {
            Transform trans = go.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                DoSetLayer(trans.GetChild(i).gameObject, layer);
            }
            go.layer = layer;
        }

        //播放特效
        public static void PlayEffect(GameObject effectGo)
        {
            Animator anim = effectGo.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Play");
        }

        public static void PlayEffect(int transformId)
        {
            int id = 0;
            if ((id = ObjectManager.GetComponent(transformId, "Animator")) != 0)
            {
                Animator anim = ObjectManager.GetComponent<Animator>(transformId, id);
                if (anim != null)
                    anim.SetTrigger("Play");
            }
        }

        public static void PlayAnimation(Animator a, string name)
        {
            a.Play(name);
        }

        //向量绕Y轴旋转
        public static Vector3 RotateAsY(Vector3 v, float angle)
        {
            angle = angle * Mathf.Deg2Rad;
            Vector3 temp = new Vector3();
            temp.x = v.x * Mathf.Cos(angle) - v.z * Mathf.Sin(angle);
            temp.z = v.x * Mathf.Sin(angle) + v.z * Mathf.Cos(angle);
            return temp;
        }

        //向量绕Z轴旋转
        public static Vector3 RotateAsZ(Vector3 v, float angle)
        {
            angle = angle * Mathf.Deg2Rad;
            Vector3 temp = new Vector3();
            temp.x = v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle);
            temp.y = v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle);
            return temp;
        }

        //背景自适应
        public static void AdjustBg(int standardWidth, int standardHeight, Transform bg, Camera camera)
        {
            if (Screen.width == standardWidth && Screen.height == standardHeight)
                return;
            float aspect = standardWidth / (float)standardHeight;
            float aspect2 = Screen.width / (float)Screen.height;
            if (aspect < aspect2)
            {
                float w = aspect2 * camera.orthographicSize * 2;
                float h = w / aspect;
                bg.localScale = new Vector3(w, h, 1);
            }
        }

        //背景自适应
        public static void AdjustBgWEqualH(int standardWidth, int standardHeight, Transform bg)
        {
            float aspect2 = Screen.width / (float)Screen.height;
            float w = aspect2 * MainCamera.orthographicSize * 2;
            float h = w;
            bg.localScale = new Vector3(w, h, 1);
        }



        public static void AdjustBgWEqualH(int standardWidth, int standardHeight, int transformId)
        {
            AdjustBgWEqualH(standardWidth, standardHeight, ObjectManager.Get(transformId));
        }

        //背景自适应
        public static void AdjustYPlane(int standardWidth, int standardHeight, Transform bg, Camera camera)
        {
            float aspect = standardWidth / (float)standardHeight;
            float aspect2 = Screen.width / (float)Screen.height;
            if (aspect <= aspect2)
            {
                float w = aspect2 * camera.orthographicSize * 2;
                float h = w / aspect;
                bg.localScale = new Vector3(w, 1, h);
            }
        }

        //背景自适应
        public static void AdjustImage(int standardWidth, int standardHeight, RectTransform bg, Canvas canvas)
        {
            float imgwidth = bg.sizeDelta.x;
            float imgheight = bg.sizeDelta.y;
            float aspect = imgwidth / imgheight;
            float aspect2 = Screen.width / (float)Screen.height;

            if (aspect > aspect2)
            {
                float h = canvas.pixelRect.height / canvas.scaleFactor;
                float w = h * aspect;
                bg.sizeDelta = new Vector2(w, h);
            }
            else if (aspect <= aspect2)
            {
                float w = canvas.pixelRect.width / canvas.scaleFactor;
                float h = w / aspect;
                bg.sizeDelta = new Vector2(w, h);
            }
        }

        //背景自适应
        public static void AdjustImageWEqualH(int standardWidth, int standardHeight, RectTransform bg, Canvas canvas)
        {
            bg.sizeDelta = new Vector2(canvas.pixelRect.width / canvas.scaleFactor, canvas.pixelRect.width / canvas.scaleFactor);
        }

        //移除button所有事件
        public static void ButtonRemoveAllEvent(UnityEngine.UI.Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        public static void ToggleRemoveAllEvent(Toggle to)
        {


            to.onValueChanged.RemoveAllListeners();
        }

        // Texture2D转Sprite
        public static Sprite Texture2DToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public static void SetGreyShader(Image image)
        {
            if (image == null)
            {
                return;

            }
            image.raycastTarget = false;
            image.material = GreyMaterial;
        }

        public static void SetGreyText(Text t, bool flag)
        {
            if (flag)
            {
                t.material = GreyTextMaterial;
            }
            else
            {
                t.material = null;
            }
        }

        public static void ResetImageShader(Image image)
        {
            if (image == null)
            {
                return;

            }
            image.raycastTarget = true;
            image.material = null;
        }

        public static void SetButtonGrey(Button button)
        {
            foreach (Image img in button.gameObject.GetComponentsInChildren<Image>())
            {
                SetGreyShader(img);
            }
            button.GetComponent<Image>().raycastTarget = false;
        }

        public static void ResetButtonGrey(Button button)
        {
            foreach (Image img in button.gameObject.GetComponentsInChildren<Image>())
            {
                ResetImageShader(img);
            }
            button.GetComponent<Image>().raycastTarget = true;
        }

        //设置阴影类型 0-无 1-硬 2-软
        public static void SetShadow(Light light, int shadowType)
        {
            light.shadows = (LightShadows)shadowType;
        }

        public static void ApplicatoinQuit()
        {
            isPrepareQuit = true;
            GameServer.Instance.LogOut();
            Application.Quit();
        }

        private static List<GameObject> goList = new List<GameObject>();

        public static GameObject DebugGo(Vector3 pos, Color color)
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Cube")) as GameObject;
            go.transform.position = pos;
            go.GetComponent<Renderer>().material.color = color;
            goList.Add(go);
            return go;
        }

        public static void ClearDebugGo()
        {
            for (int i = 0; i < goList.Count; i++)
            {
                GameObject.Destroy(goList[i]);
            }
            goList.Clear();
        }

        public static void PlayAllParticle(int transformId)
        {
            foreach (ParticleSystem ps in ObjectManager.Get(transformId).GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play();
            }
        }

        public static void PauseAllParticle(int transformId)
        {
            foreach (ParticleSystem ps in ObjectManager.Get(transformId).GetComponentsInChildren<ParticleSystem>())
            {
                ps.Pause();
            }
        }

        public static void CallGC()
        {
            Resources.UnloadUnusedAssets();
            //System.GC.Collect();
        }

        public static void SetTimeScale(float i)
        {
            Time.timeScale = i;
        }

        public static bool IsWindowsClient()
        {

            return Application.platform == RuntimePlatform.WindowsPlayer;
        }


        public static string Trim(string inn)
        {
            if (inn != null)
            {
                inn = inn.Trim();
                return inn;
            }
            else
            {
                return "";
            }

        }

        public static void LoadFishingAssets(LuaFunction callback)
        {
            if (!GameConst.DebugMode)
            {
                Launcher.DoCoroutine(GameApp.Instance.DoLoadFishingSceneAssetBundle(() =>
                {
                    if (callback != null)
                    {
                        callback.Call();
                        callback.Dispose();
                    }
                }));
            }
            else
            {
                LoadEmpty(callback);
            }
        }

        public static void LoadNewbieGuideFishingAssets(LuaFunction callback)
        {
            if (!GameConst.DebugMode)
            {
                Launcher.DoCoroutine(GameApp.Instance.DoLoadNewbieGuideFishingSceneAssetBundle(() =>
                {
                    if (callback != null)
                    {
                        callback.Call();
                        callback.Dispose();
                    }
                }));
            }
            else
            {
                LoadEmpty(callback);
            }
        }

        public static void LoadEmpty(LuaFunction callback)
        {
            Launcher.DoCoroutine(GameApp.Instance.DoLoadEmpty(() =>
            {
                if (callback != null)
                {
                    callback.Call();
                    callback.Dispose();
                }
            }));
        }

        //是否是新安装的客户端
        public static bool IsNewClient()
        {
            return PlayerPrefs.GetInt("NewClient", 0) == 0;
        }

        //设置为非新安装的客户端
        public static void SetToOldClient()
        {
            PlayerPrefs.SetInt("NewClient", 1);

        }

        static string iexplore = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe";
        public static void OpenURL(string url)
        {
            url = url.Replace("%20", " ")
                .Replace("%22", "\"")
                .Replace("%23", "#")
                .Replace("%25", "%")
                .Replace("%26", "&")
                .Replace("%28", "(")
                .Replace("%29", ")")
                .Replace("%2B", "+")
                .Replace("%2C", ",")
                .Replace("%2F", "/")
                .Replace("%3A", ":")
                .Replace("%3B", ";")
                .Replace("%3C", "<")
                .Replace("%3D", "=")
                .Replace("%3E", ">")
                .Replace("%3F", "?")
                .Replace("%40", "@")
                .Replace("%5C", "\\")
                .Replace("%7C", "|");
#if UNITY_STANDALONE
            if (File.Exists(iexplore))
                System.Diagnostics.Process.Start(iexplore, url);
            else
                Application.OpenURL(url);
#else
            Application.OpenURL(url);
#endif
        }

        public static string GetFilterName(string name)
        {
            if (name == null) return null;
            if (name.IndexOf((char)31) == -1)
            {
                return name;
            }
            return name.Split((char)31)[1];
        }

        public static float GetWidth(Transform go)
        {
            return go.GetComponent<RectTransform>().rect.width;
        }

        public static void ShowFPS(bool show)
        {
            UIFPS.instance.gameObject.SetActive(show);
        }

        public static void FPSSetExtraInfo(string text)
        {
            UIFPS.instance.SetExtraInfo(text);
        }
    }
}
