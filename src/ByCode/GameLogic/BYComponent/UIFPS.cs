using GameCommon;
using System;
using UnityDLL;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BYComponent
{
    public class UIFPS : MonoBehaviour
    {
        private Text txtFPS;
        private Text txtPing;

        public float updateInterval = 0.1F;
        private float lastInterval;
        private float frames = 0;
        private static float fps;
        private string extraInfo;
        public int targetFrameRate = GameConst.GameFrameRate;
        public static UIFPS instance;

        void Awake()
        {
            instance = this;
            if (!GameConst.ShowFPS)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                gameObject.SetActive(false);
            }
            txtFPS = transform.FindChild("frame/txtFPS").GetComponent<Text>();
            txtPing = transform.FindChild("frame/txtPing").GetComponent<Text>();
        }

        void Start()
        {
            lastInterval = Time.realtimeSinceStartup;
            frames = 0;
        }

        void Update()
        {
            ++frames;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > lastInterval + updateInterval)
            {
                fps = frames / (timeNow - lastInterval);
                frames = 0;
                lastInterval = timeNow;
                txtFPS.text = string.Format("FPS:{0}/{1}/{2}", Mathf.FloorToInt(fps), extraInfo, GameServer._GetPing());
                //txtPing.text = string.Format("ping:{0}", GameServer._GetPing());
            }
        }

        public void SetExtraInfo(string text)
        {
            extraInfo = text;
        }
    }
}
