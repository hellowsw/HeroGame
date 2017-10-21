using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using Network.Net;
using UnityDLL.NetMsgHandler;
using UnityDLL;
using LuaInterface;
using System.Collections.Generic;
using System.IO;
using UnityDLL.LuaCustom;
using GameCommon;
using Network.Timer;
using Manager;
using System;
using GameNet;

namespace GameLogic
{
    public class GameApp : App
    {
        string systemInfo;
        bool showInfo = false;
        private static GameApp instance;
        private ResourceManager resManager { get { return ResourceManager.Instance; } }

        public GameApp() { instance = this; }

        public static GameApp Instance { get { return instance; } }

        public Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                    mainCamera = Camera.main;
                return mainCamera;
            }
        }

        private bool fishingAssetsLoaded = false;
        private bool fishingAssetsLoadedNewbieGuide = false;
        private Camera mainCamera;

        public override IEnumerator OnInitialize()
        {
            Application.targetFrameRate = GameConst.GameFrameRate;
            QualitySettings.vSyncCount = 0;

            if (!GameConst.DebugMode)
            {
                yield return Launcher.DoCoroutine(UpdateManager.Instance.DoUpdateResource(null, null));
                yield return resManager.Initialize();
                yield return DoLoadMainSceneAssetBundle();
            }
            else if (GameConst.LuaBundleMode)
            {
                yield return Launcher.DoCoroutine(UpdateManager.Instance.DoUpdateResource(null, null));
                yield return resManager.Initialize();
            }

            DOTweenComponent doTween = DOTween.Init() as DOTweenComponent;
            if (doTween != null)
            {
                doTween.gameObject.name = "DOTween";
                GameObject.DontDestroyOnLoad(doTween.gameObject);
            }

            mainCamera = Camera.main;

            OnBundleLoad();

            SysMsgHandler.Init();
            LoginMsgHandler.Init();
            ScriptMsgHandler.Init();
            LuaSerialize.Init(Lua.lua);
            LYBMsgDispatcher.Instance.UnhandledMsgHandler = ProcessMsgInLua;

            yield return null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            TimerManager.Instance.Execute();
            AccountServer.Instance.Execute();
            GameServer.Instance.Execute();
            resManager.OnUpdate();

            if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftControl))
            {
                showInfo = !showInfo;
            }
        }

        private void OnBundleLoad()
        {
            Lua.Prepare();
            Lua.Initialize();
            LuaBinder.Bind(Lua.lua);
            LuaCoroutine.Register(Lua.lua, Launcher.Instance);
            Lua.Load("platform.lua");
            Lua.Load("Main.lua");

            LuaFunction func = Lua.GetFunction("Main");
            func.Call();
            func.Dispose();
            LuaLooper looper = Launcher.Instance.GameObject.AddComponent<LuaLooper>();
            looper.luaState = Lua.lua;
            luaFuncOnNetMessage = Lua.GetFunction("OnNetMessage");
            LuaRpc.Init(Lua.lua);
        }

        static LYBMsgSerializerOut arOut = new LYBMsgSerializerOut();
        static LuaFunction luaFuncOnNetMessage = null;
        void ProcessMsgInLua(byte firstID, byte secondID, TMemoryBufferEx onePacketBuff)
        {
            luaFuncOnNetMessage.BeginPCall();

            luaFuncOnNetMessage.Push(firstID);
            luaFuncOnNetMessage.Push(secondID);

            arOut.ResetStream();
            arOut.Reset(onePacketBuff);
            arOut.Skip(2);  //跳过消息头

            luaFuncOnNetMessage.Push(arOut);

            luaFuncOnNetMessage.PCall();
            luaFuncOnNetMessage.EndPCall();
        }

        private IEnumerator DoLoadMainSceneAssetBundle()
        {
            string[] file_list = resManager.GetFileList(new string[] { "atlas&start", "atlas&selectLevel" });
            yield return Launcher.DoCoroutine(DoLoadAssetBundle(file_list, 1f));
        }

        public IEnumerator DoLoadFishingSceneAssetBundle(Action onloaded)
        {
            if (!fishingAssetsLoaded)
            {
                //Debug.LogError("DoLoadFishingSceneAssetBundle Get");
                string[] file_list = resManager.GetFileList(new string[] { "prefabs&fish", "prefabs&weapon", "atlas&fishing" });
                //Debug.LogError("DoLoadFishingSceneAssetBundle Get End");
                yield return Launcher.DoCoroutine(DoLoadAssetBundle(file_list, 1f));
                if (onloaded != null)
                {
                    onloaded();
                }
                fishingAssetsLoaded = true;
            }
            else
            {

                yield return Launcher.DoCoroutine(DoLoadEmpty());
                if (onloaded != null)
                {
                    onloaded();
                }
            }
        }

        public IEnumerator DoLoadNewbieGuideFishingSceneAssetBundle(Action onloaded)
        {
            LuaTable table = Lua.lua.GetTable("preload_list.newbie_guide");
            object[] objs = table.ToArray();
            string[] pathes = new string[objs.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                pathes[i] = objs[i].ToString();
                //Debug.LogError("新手场景预加载：" + pathes[i]);
            }
            if (!fishingAssetsLoadedNewbieGuide && !fishingAssetsLoaded)
            {
                string[] file_list = resManager.GetFileList(pathes);
                yield return Launcher.DoCoroutine(DoLoadAssetBundle(file_list, 1f));
                if (onloaded != null)
                {
                    onloaded();
                }
                fishingAssetsLoadedNewbieGuide = true;
            }
            else
            {
                //Debug.Log("DoLoadNewbieGuideFishingSceneAssetBundle");
                yield return Launcher.DoCoroutine(DoLoadEmpty());

                if (onloaded != null)
                {
                    onloaded();
                }
            }
        }

        public IEnumerator DoLoadEmpty(Action onloaded)
        {
            yield return Launcher.DoCoroutine(DoLoadEmpty());

            if (onloaded != null)
            {
                onloaded();
            }
        }

        private IEnumerator DoLoadAssetBundle(string[] file_list, float ratio)
        {
            string path = null;
            int index = -1;
            float startTime = Time.time;
            string dirTemp = null;
            for (int i = 0; i < file_list.Length; i++)
            {
                if (Path.GetExtension(file_list[i]) == GameConst.ExtName || GameConst.ExtName == string.Empty)
                {
                    //path = file_list[i].Replace("source/", "");
                    path = ResourceManager.GetRealBundleName(file_list[i]);
                    yield return resManager.LoadAssetBundleAsync("source/" + path, (objs) =>
                    {
                        //Debug.LogError("预加载:" + file_list[i] + "\t" + path);
                        index = file_list[i].LastIndexOf("&");
                        if (index != -1)
                        {
                            path = path.Substring(0, index).Replace("&", "/");
                            //图集和字体资源只载入不需要保存
                            if (path.IndexOf("atlas/") == -1 && path.IndexOf("font/") == -1)
                            {
                                //加入到缓存
                                dirTemp = file_list[i].Replace("&", "/");
                                dirTemp = dirTemp.Substring(0, dirTemp.LastIndexOf('/'));
                                ResourceManager.AddSourceInLocal(dirTemp, objs[0]);
                            }
                        }
                        else
                        {
                            ResourceManager.AddSourceInLocal("", objs[0]);
                        }
                    }, ResourceManager.NeedRelease(path));

                }
            }

        }

        private IEnumerator DoLoadEmpty(float time = 0.3f)
        {
            float f = time / 10;
            for (int i = 1; i <= 10; i++)
            {
                yield return new WaitForSeconds(f);
            }
            yield return new WaitForSeconds(f);
        }

    }


}
