using System;
using GameCommon;
using UnityEngine;

namespace GameLogic.BYComponent
{
    public class YouCantChangeResolution : MonoBehaviour
    {
        private const float CHECK_INTERVAL = 2;
        private const float SCREEN_RATIO = (float)GameConst.STANDARD_WIDTH / (float)GameConst.STANDARD_HEIGHT;

        private float m_lastCheckTime;

        void Update()
        {
            if (Time.time - m_lastCheckTime > CHECK_INTERVAL)
            {
                m_lastCheckTime = Time.time;
                Check();
            }
        }

        void Check()
        {
            if (Mathf.Abs((float)Screen.width / (float)Screen.height - SCREEN_RATIO) > 0.05f)
            {
                //Debug.Log(string.Format("分辨率 {0} {1} {2} {3}", Screen.width, Screen.height, (float)Screen.width / (float)Screen.height, SCREEN_RATIO));
                //CommonLib.QuitAndOpenLogin();
                ComputeResolution();
            }
        }

        public void ComputeResolution()
        {
            Resolution resolution = new Resolution();
            resolution.width = Screen.width;
            resolution.height = Screen.height;
            float ratio = (float)resolution.height / (float)resolution.width;
            float width = 0, height = 0;
            width = ratio >= 0.625f ? (resolution.width * 0.85f) : (resolution.height * 0.85f / 0.625f);
            height = width * 0.625f;
            Screen.SetResolution((int)width, (int)height, false);
            Debug.Log(string.Format("窗口大小:{0} {1}   窗口大小:{2} {3}", resolution.width, resolution.height, width, height));
        }
    }
}
