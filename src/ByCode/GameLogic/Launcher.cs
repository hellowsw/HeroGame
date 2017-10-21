using GameCommon;
using GameNet;
using LuaInterface;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityDLL;
using UnityDLL.LuaCustom;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class Launcher : MonoBehaviour
    {
        private static Launcher instance;
        public static Launcher Instance { get { return instance; } }

        private App app;
        private GameObject go;
        [HideInInspector]
        public GameObject GameObject { get { return go; } }
        private Transform trans;
        [HideInInspector]
        public Transform Transform { get { return trans; } }

        public static Coroutine DoCoroutine(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
        }

        void Awake()
        {
            ComputeResolution();
            if (instance == null)
            {
                instance = this;
                go = gameObject;
                trans = transform;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            app = new GameApp();
            StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return app.OnInitialize();
            app.OnLoaded();
        }

        void Update()
        {
            app.OnUpdate();
        }

        void FixedUpdate()
        {
            app.OnFixedUpdate();
        }

        void LateUpdate()
        {
            app.OnLateUpdate();
        }

        void OnEnable()
        {
            app.OnUnpause();
        }

        void OnDisable()
        {
            app.OnPause();
        }

        void OnDestroy()
        {
            app.OnQuit();
            instance = null;
        }

        void OnGUI()
        {
            app.OnGUI();
        }

        public void ComputeResolution()
        {
            Resolution resolution = Screen.currentResolution;
            float ratio = (float)GameConst.STANDARD_WIDTH / (float)GameConst.STANDARD_HEIGHT;
            float width = 0, height = 0;
            height = resolution.height * 0.84f;
            width = height * ratio;
            Screen.SetResolution((int)width, (int)height, false);
            Debug.Log(string.Format("屏幕大小:{0} {1}   窗口大小:{2} {3}", resolution.width, resolution.height, width, height));
        }
    }

    public abstract class App
    {
        public abstract IEnumerator OnInitialize();
        public virtual void OnLoaded() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnPause() { }
        public virtual void OnUnpause() { }
        public virtual void OnQuit() { }
        public virtual void OnGUI() { }
    }
}
