using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Manager
{
    public class UpdateManager : SingletonObject<UpdateManager>
    {
        private int streamingVersion;
        private int rwVersion;
        Dictionary<string, string> localNewDic = new Dictionary<string, string>();
        Dictionary<string, string> localOldDic = new Dictionary<string, string>();

        public UpdateManager()
        {

        }

        #region public method

        public void DeleteRWFiles()
        {
            if (Directory.Exists(GameConst.RWPath + "lua"))
            {
                Debug.Log("删除:" + GameConst.RWPath + "lua");
                Directory.Delete(GameConst.RWPath + "lua", true);
            }
            if (Directory.Exists(GameConst.RWPath + "source"))
            {
                Debug.Log("删除:" + GameConst.RWPath + "source");
                Directory.Delete(GameConst.RWPath + "source", true);
            }
            if (File.Exists(GameConst.RWPath + ResourceManager.files_md5))
            {
                Debug.Log("删除files_md5:" + GameConst.RWPath + ResourceManager.files_md5);
                File.Delete(GameConst.RWPath + ResourceManager.files_md5);
            }
            if (File.Exists(GameConst.RWPath + ResourceManager.source_md5 + GameConst.ExtName))
            {
                Debug.Log("删除source_md5:" + GameConst.RWPath + ResourceManager.source_md5 + GameConst.ExtName);
                File.Delete(GameConst.RWPath + ResourceManager.source_md5 + GameConst.ExtName);
            }
            if (File.Exists(GameConst.RWPath + Util.md5("StreamingAssets")))
            {
                Debug.Log("删除streamingAssets:" + GameConst.RWPath + Util.md5("StreamingAssets"));
                File.Delete(GameConst.RWPath + Util.md5("StreamingAssets"));
            }
        }

        //得到本地资源最新版本的路径
        public string GetNewestFilePath()
        {
            return rwVersion > streamingVersion ? GameConst.RWPath : GameConst.StreamingPath;
        }

        //得到本地资源较老版本的路径
        public string GetOlderFilePath()
        {
            return GetLocalNewestVersion() == streamingVersion ? GameConst.RWPath : GameConst.StreamingPath;
        }

        //得到本地资源最新版本号
        public int GetLocalNewestVersion()
        {
            return rwVersion > streamingVersion ? rwVersion : streamingVersion;
        }

        //得到本地资源较老版本号
        public int GetLocalOlderVersion()
        {
            return GetLocalNewestVersion() == streamingVersion ? rwVersion : streamingVersion;
        }

        //得到资源所在最新路径
        public string GetRelPath(string sourceName)
        {
            if (GetNewestFilePath().Equals(GameConst.RWPath) && File.Exists(GameConst.RWPath + sourceName))
                return GetNewestFilePath() + sourceName;
            else
                return GameConst.StreamingPath + sourceName;
        }

        /// <summary>
        /// 启动更新下载
        /// </summary>
        public IEnumerator DoUpdateResource(Action OnUpdateComplate, Action<float> OnProgress)
        {
            if (!GameConst.UpdateMode)
            {
                if (OnUpdateComplate != null)
                    OnUpdateComplate();
                yield break;
            }

            yield return InitVersion();

            string listUrl = GameConst.DownloadUrl + ResourceManager.files_md5;

            //1.下载服务器的file.txt文件，对比版本号
            WWW www = new WWW(listUrl);
            Debug.LogError("开始下载files.txt");
            yield return www;
            Debug.LogError("下载完成files.txt");
            if (www.error != null)
            {
                Debug.LogError("下载资源列表失败!!! url:" + listUrl);
                if (OnUpdateComplate != null)
                    OnUpdateComplate();
                yield break;
            }

            //2.取得本地最新的file.txt文件，获得版本号
            Debug.LogError("使用目录:" + GetNewestFilePath());
            string[] server_list = Util.UnpackTextContent(Util.ReadFromMemory(www.bytes));//.Split('\r');
            int local_version = GetLocalNewestVersion();
            int server_version = GetVersionNum(server_list[0]);
            Debug.LogError("本地版本号:" + local_version);
            Debug.LogError("服务器版本号:" + server_version);
            if (local_version < server_version)
            {
                //3.如果本地版本号低于下载的版本号，对比md5码，下载文件
                Debug.LogError("开始更新...");
                string[] temp = null;
                string file_name = null;
                string full_name = null;
                string dir = null;
                string md5 = null;
                bool needUpate = false;
                if (File.Exists(GameConst.RWPath + ResourceManager.files_md5))
                {
                    File.Delete(GameConst.RWPath + ResourceManager.files_md5);
                }
                File.WriteAllBytes(GameConst.RWPath + ResourceManager.files_md5, www.bytes);
                List<string> downloadList = new List<string>();
                for (int i = 1; i < server_list.Length; i++)
                {
                    needUpate = false;
                    temp = server_list[i].Split('|');
                    if (temp.Length != 2) continue;
                    full_name = temp[0].Replace("\n", "");
                    md5 = temp[1];
                    if (localNewDic.ContainsKey(full_name))
                    {
                        if (localNewDic[full_name] != md5)
                        {
                            //Debug.LogError("更新列表md5对比:" + localNewDic[full_name] + "\t" + md5);
                            if (File.Exists(GameConst.RWPath + full_name))
                                File.Delete(GameConst.RWPath + full_name);
                            needUpate = true;
                        }
                        //Warning 这里还有问题，因为Android下面File.Exists会一直返回false
                        //如果可读写目录的md5码与服务器一致，但是可读写目录没有这个文件，应该再判断只读目录有没有该文件，并且判断m5d码是否一致
                        else if (!File.Exists(GameConst.RWPath + full_name))
                        {
                            //bool exists = File.Exists(GameConst.StreamingPath + full_name);
                            if (/*!exists || */(localOldDic.ContainsKey(full_name) && localOldDic[full_name] != md5))
                                needUpate = true;
                            //Debug.LogError("更新列表md5对比:" + localNewDic[full_name] + "\t" + md5 + "\texits:" + exists);
                        }
                    }
                    else
                    {
                        needUpate = true;
                        //Debug.LogError("更新列表md5对比:" + localNewDic[full_name] + "\t" + md5 + "\tneed:" + needUpate);
                    }
                    if (needUpate)
                    {
                        downloadList.Add(full_name);
                    }
                }
                for (int j = 0; j < downloadList.Count; j++)
                {
                    full_name = downloadList[j];
                    file_name = Path.GetFileName(full_name);
                    dir = Path.GetDirectoryName(full_name);
                    Debug.LogError("更新:" + full_name);
                    WWW download = new WWW(GameConst.DownloadUrl + full_name);
                    yield return download;
                    if (download.error != null)
                    {
                        Debug.LogError("下载 " + file_name + " 失败!!!");
                    }
                    else
                    {
                        Debug.LogError("下载成功，保存至:" + GameConst.RWPath + dir + "/" + file_name + "\tsize:" + download.bytes.Length);
                        if (!Directory.Exists(GameConst.RWPath + dir))
                        {
                            Directory.CreateDirectory(GameConst.RWPath + dir);
                        }
                        File.WriteAllBytes(GameConst.RWPath + dir + "/" + file_name, download.bytes);
                    }
                    if (OnProgress != null)
                        OnProgress(j / (float)downloadList.Count);
                }
                if (OnProgress != null)
                    OnProgress(1);
                Debug.LogError("更新资源完成!!!");
            }
            else
            {
                Debug.LogError("服务器资源过时");
            }

            rwVersion = server_version;

            if (OnUpdateComplate != null)
                OnUpdateComplate();

            localNewDic = null;
            localOldDic = null;
        }

        #endregion

        #region private method

        private IEnumerator InitVersion()
        {
            string[] rwlist = null;
            string[] streaminglist = null;
            if (File.Exists(GameConst.RWPath + ResourceManager.files_md5))
            {
                rwlist = Util.ReadArrFromBinaryFile(GameConst.RWPath + ResourceManager.files_md5);
                rwVersion = GetVersionNum(rwlist[0]);
            }

            string fpath = null;
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                fpath = "file://" + GameConst.StreamingPath + ResourceManager.files_md5;
            else
                fpath = GameConst.StreamingPath + ResourceManager.files_md5;
            WWW www = new WWW(fpath);
            yield return www;
            if (www.error == null)
            {
                streaminglist = Util.UnpackTextContent(Util.ReadFromMemory(www.bytes));
                streamingVersion = GetVersionNum(streaminglist[0]);
                Debug.LogError("用www加载streaming files:" + streaminglist.Length);
            }
            else
            {
                Debug.LogError("streaming files.txt is nil");
            }

            string[] furthest = rwVersion > streamingVersion ? rwlist : streaminglist;
            AddToDic(furthest, localNewDic);

            if (GetLocalOlderVersion() != 0)
            {
                furthest = GetLocalOlderVersion() == rwVersion ? rwlist : streaminglist;
                AddToDic(furthest, localOldDic);
            }

            //如果不可写目录的版本号大于可读写目录的版本号时，说明可读写目录的资源已经没有价值，直接删除
            if (streamingVersion >= rwVersion)
            {
                try
                {
                    Debug.LogError("不可写目录的版本号大于可读写目录的版本号，直接删除");
                    DeleteRWFiles();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            yield return null;
        }

        private void AddToDic(string[] arr, Dictionary<string, string> dic)
        {
            if (arr == null)
            {
                Debug.LogError("arr is null");
                return;
            }
            string[] furthest = arr;
            string[] temp = null;
            for (int i = 1; i < furthest.Length; i++)
            {
                temp = furthest[i].Split('|');
                if (temp != null && temp.Length == 2)
                    dic.Add(temp[0].Replace("\n", ""), temp[1]);
            }
        }

        private int GetVersionNum(string version)
        {
            return int.Parse(version.Split('-')[1]);
        }

        #endregion
    }
}
