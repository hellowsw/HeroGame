#define _ShowLog_

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UnityEngine.SceneManagement;
using GameLogic;
using System;
using UObject = UnityEngine.Object;
using GameCommon;
using Manager;


public class ResourceManager : SingletonObject<ResourceManager>
{
    //所有已加载的bundle缓存，[key = bundle完整路径] [value = bundle]
    static Dictionary<string, AssetBundle> m_Bundles = new Dictionary<string, AssetBundle>();

    //所有加载中的bundle
    static HashSet<string> m_LoadingBundles = new HashSet<string>();

    //本地资源 Resource下的资源,AssetBundle的资源在会保存在这里
    static Dictionary<string, List<UObject>> m_LocalResources = new Dictionary<string, List<UObject>>();

    //所有引用
    static Dictionary<string, byte> m_Dependences = new Dictionary<string, byte>();

    //文件名对应的md5
    static Dictionary<string, string> m_SourceNameMD5 = new Dictionary<string, string>();

    private string[] m_fileList = null; //files.txt的所有行

    private AssetBundleManifest m_Manifest;

    public string[] FileList { get { return m_fileList; } }

    private static bool isAsyncLoading = false;

    public static string files_md5 = Util.md5("files.txt");
    public static string filemd5_md5 = Util.md5("filemd5.txt");
    public static string source_md5 = Util.md5("source");

    static bool DebugLog = false;

    #region LoadAssetBundle

    public void OnUpdate()
    {
        
    }

    public IEnumerator Initialize()
    {
        string fpath = null;
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
            fpath = "file://" + GameConst.StreamingPath + files_md5;
        else
            fpath = GameConst.StreamingPath + files_md5;
        WWW www = new WWW(fpath);
        yield return www;
        if (www.error == null)
        {
            m_fileList = Util.UnpackTextContent(Util.ReadFromMemory(www.bytes));
#if _ShowLog_
            Debug.LogError("Resources 用www加载streaming files:" + m_fileList.Length);
#endif
            //Debug.LogError(Util.ReadFromMemory(www.bytes));
        }
        else
        {
#if _ShowLog_
            Debug.LogError("Resources 用www加载streaming files 失败 " + www.error + "\tpath:" + fpath);
#endif
        }

        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.IPhonePlayer)
            fpath = "file://" + GameConst.StreamingPath + filemd5_md5;
        else
            fpath = GameConst.StreamingPath + filemd5_md5;
        www = new WWW(fpath);
        yield return www;
        if (www.error == null)
        {
            m_SourceNameMD5 = Util.ReadDicFromMemory(www.bytes, true);
            Debug.LogError("Resources 用www加载streaming files2:" + m_SourceNameMD5.Count);
            //Debug.LogError(Util.ReadFromMemory(www.bytes));
        }
        else
        {
            Debug.LogError("Resources 用www加载streaming files2 失败:" + fpath);
        }

        string url = UpdateManager.Instance.GetRelPath(source_md5 + GameConst.ExtName);
        AssetBundle main = AssetBundle.LoadFromFile(url);
        //加载Manifest文件
        if (main != null)
            m_Manifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        yield return null;
    }

    public void FileForeach(string filter, Action<string> callback)
    {
        if (callback == null) return;

        foreach (KeyValuePair<string, string> kv in m_SourceNameMD5)
        {
            if (Util.EqualsWithHead(kv.Key, filter) && Path.GetExtension(kv.Key) == GameConst.ExtName)
            {
                callback(GetRealBundleName(kv.Key));
            }
        }
    }

    public string[] GetFileList(string[] filter)
    {
        List<string> fileList = new List<string>();
        bool contains = false;
        foreach (KeyValuePair<string, string> kv in m_SourceNameMD5)
        {
            contains = false;
            for (int i = 0; i < filter.Length; i++)
            {
                if (Util.EqualsWithHead(kv.Key, filter[i]) && Path.GetExtension(kv.Key) == GameConst.ExtName)
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                //Debug.Log("GetFileList:" + kv.Key + "\t" + kv.Value);
                fileList.Add(kv.Key);
            }
        }
        return fileList.ToArray();
    }

    public Dictionary<string, string> GetFileDic(string[] filter)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        bool contains = false;
        foreach (KeyValuePair<string, string> kv in m_SourceNameMD5)
        {
            contains = false;
            for (int i = 0; i < filter.Length; i++)
            {
                if (Util.EqualsWithHead(kv.Key, filter[i]) && Path.GetExtension(kv.Key) == GameConst.ExtName)
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                //Debug.Log("GetFileList:" + kv.Key + "\t" + kv.Value);
                dic.Add(kv.Key, kv.Value);
            }
        }
        return dic;
    }

    public string[] GetFileListIgnore(string[] ignore)
    {
        List<string> fileList = new List<string>();
        bool contains = false;
        foreach (KeyValuePair<string, string> kv in m_SourceNameMD5)
        {
            contains = false;
            for (int i = 0; i < ignore.Length; i++)
            {
                if (Util.EqualsWithHead(kv.Key, ignore[i]) && Path.GetExtension(kv.Key) == GameConst.ExtName)
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
                fileList.Add(kv.Key);
        }
        return fileList.ToArray();
    }


    public IEnumerator LoadAssetBundleAsync(string abName, Action<UObject[]> onLoaded, bool unload)
    {

        string url = UpdateManager.Instance.GetRelPath(abName);
        Log("LoadAssetBundleAsync abName:" + abName + "\turl:" + url);
        if (m_Bundles.ContainsKey(url))
        {
            if (onLoaded != null)
                onLoaded(m_Bundles[url].LoadAllAssets());
            yield break;
        }

        if (string.IsNullOrEmpty(url))
        {
            LogErr("找不到资源:" + url);
        }
        else if (m_LoadingBundles.Contains(url))
        {
            LogErr("资源正在加载中:" + url);
        }
        else
        {
            m_LoadingBundles.Add(url);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(url);
            yield return request;
            m_LoadingBundles.Remove(url);

            if (onLoaded != null && request.assetBundle)
            {
                onLoaded(request.assetBundle.LoadAllAssets());
            }
            if (!unload)
            {
                m_Bundles.Add(url, request.assetBundle);
            }
            else if (unload)
            {
                request.assetBundle.Unload(false);
            }
        }
    }

    public AssetBundle LoadBundleAndAllDependence(string assetbundleName)
    {
        string path = UpdateManager.Instance.GetRelPath("source/" + assetbundleName);
        if (m_Dependences.ContainsKey(path))
        {
            return LoadAssetBundle(path);
        }
        string[] dependence = m_Manifest.GetAllDependencies(assetbundleName);

        foreach (string bundleName in dependence)
        {
            Log("LoadBundleAndAllDependence assetbundleName:" + assetbundleName + "\tdependence:" + bundleName);
            LoadBundleAndAllDependence(bundleName);
        }

        m_Dependences.Add(path, 1);
        return LoadAssetBundle(path);
    }

    public IEnumerator LoadBundleAndAllDependenceAsync(string assetbundleName, Action<UObject[]> callback, bool unload = false)
    {
        while (isAsyncLoading)
            yield return null;

        string url = UpdateManager.Instance.GetRelPath(assetbundleName);
        Log("LoadBundleAndAllDependenceAsync url:" + url + "\tname:" + assetbundleName);
        if (m_Bundles.ContainsKey(url))
        {
            if (callback != null)
                callback(m_Bundles[url].LoadAllAssets());
            yield break;
        }

        string[] dependence = m_Manifest.GetAllDependencies(assetbundleName.Replace("source/", string.Empty));

        foreach (string bundleName in dependence)
        {
            yield return LoadBundleAndAllDependenceAsync("source/" + bundleName, (objs)=> {
                Log("LoadBundleAndAllDependenceAsync AddToLocal");
            });
        }
        isAsyncLoading = true;
        yield return LoadAssetBundleAsync(assetbundleName, callback, ResourceManager.NeedRelease(assetbundleName));
        isAsyncLoading = false;
    }

    AssetBundle LoadAssetBundle(string assetbundleName)
    {
        if (m_LoadingBundles.Contains(assetbundleName))
        {
            LogErr("资源正在加载中:" + assetbundleName);
        }
        else if (!m_Bundles.ContainsKey(assetbundleName))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(assetbundleName);
            m_Bundles.Add(assetbundleName, bundle);
            LogErr("不存在资源:" + bundle.LoadAllAssets<UObject>()[0].name + "\t" + bundle.LoadAllAssets<UObject>()[0].ToString() + "\t" + assetbundleName);
            return bundle;
        }
        LogErr("存在资源:" + m_Bundles[assetbundleName].LoadAllAssets<UObject>()[0].name + "\t" + m_Bundles[assetbundleName].LoadAllAssets<UObject>()[0].ToString() + "\t" + assetbundleName);
        return m_Bundles[assetbundleName];
    }

    public static bool NeedRelease(string sourceName)
    {
        return sourceName.IndexOf("atlas&") == -1 && sourceName.IndexOf("font&") == -1;
    }

    #endregion

    #region loadFromResources

    public static UObject LoadResourceFromLocal(string path, string name)
    {
        path = path.ToLower();
        name = name.ToLower();
        string full_path = UpdateManager.Instance.GetRelPath("source/" + GetBundleNameByLocalPath(path + "/" + name));

        if (!m_LocalResources.ContainsKey(path))
        {
            m_LocalResources.Add(path, new List<UObject>() { });
        }

        List<UObject> gos = null;
        UObject go = null;
        if (m_LocalResources.TryGetValue(path, out gos))
        {

            go = gos.Find(s => s.name == name);
            if (go != null)
            {
                Log("找到已存在的资源:" + path + "\tname:" + name);
            }
            else
            {
                Log("没有找到已存在的资源:" + path + "\tname:" + name);
            }
        }
        if (go == null)
        {
            go = Resources.Load<UObject>(path + "/" + name);
            if (go != null)
                m_LocalResources[path].Add(go);
        }
        if (go == null)
        {
            AssetBundle bundle = Instance.LoadBundleAndAllDependence(GetBundleNameByLocalPath(path + "/" + name));
            Log("LoadResourceFromLocal:" + path + "/" + name);
            if (bundle != null)
                go = bundle.LoadAllAssets<UObject>()[0];
        }
        return go;
    }

    public static void LoadResourceAsync(string path, string name, Action<UObject> callback)
    {
        Launcher.DoCoroutine(DoLoadResourceAsync(path, name, callback));
    }

    public static void LoadResourceAsync(string path, string name, LuaFunction callback)
    {
        Launcher.DoCoroutine(DoLoadResourceAsync(path, name, (obj) =>
        {
            if (callback != null)
            {
                callback.Call(obj);
                callback.Dispose();
            }
        }));
    }

    static IEnumerator DoLoadResourceAsync(string path, string name, Action<UObject> callback)
    {
        path = path.ToLower();

        if (!m_LocalResources.ContainsKey(path))
        {
            m_LocalResources.Add(path, new List<UObject>() { });
        }

        List<UObject> gos = null;
        UObject go = null;
        if (m_LocalResources.TryGetValue(path, out gos))
        {
            go = gos.Find(s => s.name == name);
        }
        if (go == null)
        {
            ResourceRequest request = Resources.LoadAsync<UObject>(path + "/" + name);
            yield return request;
            go = request.asset;
            if (go != null)
                m_LocalResources[path].Add(go);
        }
        if (go == null)
        {
            Log("source/" + GetBundleNameByLocalPath(path + "/" + name));
            yield return Instance.LoadBundleAndAllDependenceAsync("source/" + GetBundleNameByLocalPath(path + "/" + name), (objs) =>
            {
                go = objs[0];
            });
        }
        if (callback != null)
            callback(go);
    }

    public static UObject GetResourcesFromLocal(string path, string name)
    {
        return LoadResourceFromLocal(path, name);
    }
    public static bool ExitsFromResources(string path, string name)
    {
        List<UObject> gos = null;
        UObject go = null;
        if (m_LocalResources.TryGetValue(path, out gos))
        {
            go = gos.Find(s => s.name == name);
        }
        return go != null;
    }

    public static UObject InstantiateFromResources(string path, string name)
    {
        return GameObject.Instantiate(GetResourcesFromLocal(path, name)) as UObject;
    }

    public static Sprite LoadSpriteFromResources(string path, string name)
    {
        UObject obj = GetResourcesFromLocal("sprite/" + path, name);
        if (obj == null)
        {
            return null;
        }
        SpriteRenderer renderer = (obj as GameObject).GetComponent<SpriteRenderer>();
        return renderer.sprite;
    }

    public static void AddSourceInLocal(string path, UObject obj)
    {
        Log("AddSourceInLocal:" + path + "\tname:" + obj.name);
        if (!m_LocalResources.ContainsKey(path))
        {
            m_LocalResources.Add(path, new List<UObject>() { });
        }

        if (obj != null && !m_LocalResources[path].Contains(obj))
        {
            m_LocalResources[path].Add(obj);
        }
    }

    public static string GetBundleNameByLocalPath(string path)
    {
        path = path.Replace("/", "&").ToLower() + GameConst.ExtName;
        Log("GetBundleNameByLocalPath:" + path + "\tmd5:" + GetRealBundleName(path));
        return GetRealBundleName(path);
    }

    public static string GetRealBundleName(string path)
    {
        if (!m_SourceNameMD5.ContainsKey(path))
        {
            LogErr("资源不存在:" + path);
            return null;
        }
        return m_SourceNameMD5[path];
    }

    #endregion

    #region LoadScene

    public static void LoadScene(string sceneName, LuaFunction func)
    {
        Launcher.DoCoroutine(DoLoadScene(sceneName, func));
    }

    public static void UnLoadScene(string sceneName)
    {
        SceneManager.UnloadScene(sceneName);
    }

    static IEnumerator DoLoadScene(string sceneName, LuaFunction func)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone && operation.progress < 0.9f)
        {
            yield return operation;
        }
        if (func != null)
        {
            func.Call();
            func.Dispose();
        }
    }

    #endregion

    static void Log(string log)
    {
        if (DebugLog)
            Debug.Log(log);
    }

    static void LogErr(string log)
    {
        if (DebugLog)
            Debug.LogError(log);
    }
}
