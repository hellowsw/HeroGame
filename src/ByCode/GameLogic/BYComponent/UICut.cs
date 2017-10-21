using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BYComponent
{
    public class UICut : MonoBehaviour
    {
        private static UICut instance;

        private Material mat;
        private float timer = 0;
        private float time = 1;
        private bool isStart = false;
        private bool autoHide = false;
        private GameObject imgGo;
        private GameObject go;
        private float fromValue;
        private float toValue;
        private float delayTime;

        public void Init()
        {
            go = gameObject;
            instance = this;
            imgGo = transform.FindChild("image").gameObject;
            mat = imgGo.GetComponent<Image>().material;
            Hide();
        }

        public void Tween(float from, float to, bool autoHide, float delay)
        {
            delayTime = delay;
            fromValue = from;
            toValue = to;
            isStart = true;
            timer = 0;
            this.autoHide = autoHide;
            mat.SetFloat("_T", fromValue);
            go.SetActive(true);
        }

        void Update()
        {
            if (!isStart) return;

            timer += Time.deltaTime;
            if (timer < delayTime)
                return;
            if (timer - delayTime < time)
            {
                mat.SetFloat("_T", Mathf.Lerp(fromValue, toValue, (timer - delayTime) / time));
            }
            else
            {
                mat.SetFloat("_T", 1);
                if (autoHide)
                    Hide();
            }
        }

        public static void CutIn(bool autoHide, float delay)
        {
            Show();
            instance.Tween(1, 0, autoHide, delay);
        }

        public static void CutOut(bool autoHide, float delay)
        {
            Show();
            instance.Tween(0, 1, autoHide, delay);
        }

        public static void Show()
        {
            instance.go.SetActive(true);
        }

        public static void Hide()
        {
            instance.go.SetActive(false);
        }

        public static void CreateInstance()
        {
            GameObject go = GameObject.Find("Launcher/UIManager/CutPanel");
            UICut cut = go.AddComponent<UICut>();
            cut.Init();
        }
    }
}
