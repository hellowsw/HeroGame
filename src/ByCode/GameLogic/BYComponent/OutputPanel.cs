using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCommon;

namespace GameLogic.BYComponent
{
    public class OutputPanel : MonoBehaviour
    {
        private GameObject window;
        private Text txtContent;
        private float update_interval = 0.5f;
        private float last_update_time = 0;
        private Dictionary<OutputType, string> contents = new Dictionary<OutputType, string>();
        public static OutputPanel instance;
        public static OutputPanel CreateInstance()
        {
            if (instance == null)
            {
                instance = GameObject.Find("Launcher/UIManager/OutputPanel").AddComponent<OutputPanel>();
                instance.Init();
            }
            return instance;
        }

        public static void Destroy()
        {
            GameObject go = GameObject.Find("Launcher/UIManager/OutputPanel");
            if (go != null)
                Destroy(go);
        }

        public void Init()
        {
            txtContent = transform.GetComponentInChildren<Text>();
            window = transform.FindChild("window").gameObject;
        }

        public void SetContent(string content, OutputType type)
        {
            if (contents.ContainsKey(type))
            {
                contents[type] = content;
            }
            else
            {
                contents.Add(type, content);
            }
        }

        public static void Output(string content, int type)
        {
            if (instance == null)
                return;
            instance.SetContent(content, (OutputType)type);
        }

        public static void Output(string content, OutputType type)
        {
            if (instance == null)
                return;
            instance.SetContent(content, type);
        }

        void Update()
        {
            if (Time.time - last_update_time >= update_interval)
            {
                last_update_time = Time.time;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("F1键开关此面板");
                foreach (KeyValuePair<OutputType, string> kv in contents)
                {
                    sb.AppendLine(kv.Key.ToString() + ":" + kv.Value.ToString());
                }
                txtContent.text = sb.ToString();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                window.SetActive(!window.activeInHierarchy);
                if (window.activeInHierarchy)
                    transform.SetAsLastSibling();
            }
        }

        public enum OutputType
        {
            鱼数量 = 1,
            鱼阵数量 = 2,
            特效数量 = 3,
            子弹数量 = 4,
            消息数量 = 5,
            当前消息数量 = 6,
        }
    }
}
