using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GameCommon;
using System;

namespace ToLuaEditor.Editor
{
#if true
    public class Packager
    {
        public static string srcDicName = "Resources";
        public static string platform = string.Empty;
        static List<string> paths = new List<string>();
        static List<string> files = new List<string>();
        static List<AssetBundleBuild> maps = new List<AssetBundleBuild>();

        [MenuItem("Packager/SignBundleName")]
        public static void AssignBundleName()
        {
            string path = Application.dataPath + "/" + srcDicName;
            if (!Directory.Exists(path)) return;
            HandleAllAssetBundleName(new DirectoryInfo(path));
            AssetDatabase.Refresh();
        }

        //[MenuItem("Packager/Set")]
        //public static void Add()
        //{
        //    PlayerPrefs.SetInt("VersionNum", 400);
        //}

        [MenuItem("Packager/SignAtlasBundleName")]
        public static void AssignAtlasBundleName()
        {
            string path = Application.dataPath + "/Atlas";
            if (!Directory.Exists(path)) return;
            HandleAtlasAssetBundleName(new DirectoryInfo(path));
            AssetDatabase.Refresh();
        }

        static void HandleAllAssetBundleName(DirectoryInfo dirInfo)
        {
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.FullName.EndsWith(".meta"))
                {
                    continue;
                }
                string path = file.FullName.Replace('\\', '/').Replace(Application.dataPath, "");
                string bundleFullName = path.Replace("/" + srcDicName + "/", "");
                path = "Assets" + path;
                AssetImporter importer = AssetImporter.GetAtPath(path);

                if (importer != null)
                {
                    string name = bundleFullName.Replace(Path.GetExtension(file.Name), "").Replace("/", "&") + GameConst.ExtName;
                    string md5 = Util.md5(name);
                    //UnityEngine.Debug.Log(name + "\t" + md5);
                    map.Add(name, md5);
                    //使用路径+名字作包名
                    importer.assetBundleName = md5;
                    //UnityEngine.Debug.Log("AssetName:" + file.FullName);
                }
            }
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                HandleAllAssetBundleName(dir);
            }
        }

        static void HandleAtlasAssetBundleName(DirectoryInfo dirInfo)
        {
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (dir.FullName.EndsWith(".meta"))
                {
                    continue;
                }
                string path = dir.FullName.Replace('\\', '/').Replace(Application.dataPath, "");
                string bundleFullName = path.Replace("/" + srcDicName + "/", "");
                path = "Assets" + path;
                AssetImporter importer = AssetImporter.GetAtPath(path);

                if (importer != null)
                {
                    //使用路径+名字作包名
                    string name = (bundleFullName.Replace("/", "&") + GameConst.ExtName).Remove(0, 1);
                    string md5 = Util.md5(name);
                    map.Add(name, md5);
                    importer.assetBundleName = md5;
                    //UnityEngine.Debug.Log("AssetName:" + file.FullName);
                }
            }
            UnityEngine.Debug.Log("atlas package complate!!!");
        }

        [MenuItem("Packager/ClearBundleName")]
        public static void ClearBundleName()
        {
            string path = Application.dataPath + "/" + srcDicName;
            HandleClearAllAssetBundleName(new DirectoryInfo(path));
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
        }

        [MenuItem("Packager/ClearAtlasBundleName")]
        public static void ClearAtlasBundleName()
        {
            string path = Application.dataPath + "/Atlas";
            HandleClearAtlasAllAssetBundleName(new DirectoryInfo(path));
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
        }

        static void HandleClearAllAssetBundleName(DirectoryInfo dirInfo)
        {
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.FullName.EndsWith(".meta"))
                {
                    continue;
                }
                string path = file.FullName.Replace('\\', '/').Replace(Application.dataPath, "");
                string bundleFullName = path.Replace("/" + srcDicName + "/", "");
                path = "Assets" + path;
                AssetImporter importer = AssetImporter.GetAtPath(path);

                if (importer != null)
                {
                    //使用路径+名字作包名
                    importer.assetBundleName = null;
                    //UnityEngine.Debug.Log("AssetName:" + file.FullName);
                }
            }
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                HandleClearAllAssetBundleName(dir);
            }
        }

        static void HandleClearAtlasAllAssetBundleName(DirectoryInfo dirInfo)
        {
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                if (dir.FullName.EndsWith(".meta"))
                {
                    continue;
                }
                string path = dir.FullName.Replace('\\', '/').Replace(Application.dataPath, "");
                string bundleFullName = path.Replace("/" + srcDicName + "/", "");
                path = "Assets" + path;
                AssetImporter importer = AssetImporter.GetAtPath(path);

                if (importer != null)
                {
                    //使用路径+名字作包名
                    importer.assetBundleName = null;
                    //UnityEngine.Debug.Log("AssetName:" + file.FullName);
                }
            }
            UnityEngine.Debug.Log("clear atlas assetbundle name complate!!!");
        }

        [MenuItem("Packager/Build iPhone Resource", false, 100)]
        public static void BuildiPhoneResource()
        {
            BuildTarget target;
#if UNITY_5
            target = BuildTarget.iOS;
#else
            target = BuildTarget.iPhone;
#endif
            BuildAssetResource(target);
        }

        [MenuItem("Packager/Build Android Resource", false, 101)]
        public static void BuildAndroidResource()
        {
            BuildAssetResource(BuildTarget.Android);
        }

        [MenuItem("Packager/Build Windows Resource", false, 102)]
        public static void BuildWindowsResource()
        {
            BuildAssetResource(BuildTarget.StandaloneWindows);
        }

        [MenuItem("Packager/Build iPhone Lua", false, 100)]
        public static void BuildiPhoneLua()
        {
            BuildTarget target;
#if UNITY_5
            target = BuildTarget.iOS;
#else
            target = BuildTarget.iPhone;
#endif
            BuildLua(target);
        }

        [MenuItem("Packager/Build Android Lua", false, 101)]
        public static void BuildAndroidLua()
        {
            BuildLua(BuildTarget.Android);
        }

        [MenuItem("Packager/Build Windows Lua", false, 102)]
        public static void BuildWindowsLua()
        {
            BuildLua(BuildTarget.StandaloneWindows);
        }

        /// <summary>
        /// 生成绑定素材
        /// </summary>
        public static void BuildAssetResource(BuildTarget target)
        {
            map.Clear();
            if (!GameConst.DebugMode)
            {
                AssignBundleName();
                AssignAtlasBundleName();
            }
            if (Directory.Exists(GameConst.StreamingPath))
            {
                //Directory.Delete(Util.DataPath, true);
            }
            string streamPath = Application.streamingAssetsPath;
            if (Directory.Exists(streamPath))
            {
                // Directory.Delete(streamPath, true);
            }
            Directory.CreateDirectory(streamPath);
            AssetDatabase.Refresh();
            maps.Clear();
            HandleLuaBundle();

            BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle |
                                            BuildAssetBundleOptions.UncompressedAssetBundle;
            BuildPipeline.BuildAssetBundles(streamPath, maps.ToArray(), options, target);

            string streamDir = Application.dataPath + "/" + GameConst.LuaTempDir;
            if (Directory.Exists(streamDir)) Directory.Delete(streamDir, true);

            if (!GameConst.DebugMode)
            {
                UnityEngine.Debug.Log(GameConst.DebugMode);
                streamPath = Application.streamingAssetsPath + "/source";
                if (Directory.Exists(streamPath))
                {
                    Directory.Delete(streamPath);
                }
                Directory.CreateDirectory(streamPath);
                BuildPipeline.BuildAssetBundles(streamPath, BuildAssetBundleOptions.None, target);
                streamDir = Application.dataPath + "/" + GameConst.LuaTempDir;

                //重命名source文件，加上扩展名
                Util.Rename(Application.streamingAssetsPath + "/source/source", Application.streamingAssetsPath + "/" + Util.GetFileName("source") + GameConst.ExtName);

                //重命名source.manifest文件
                Util.Rename(Application.streamingAssetsPath + "/source/source.manifest", Application.streamingAssetsPath + "/" + Util.GetFileName("source.manifest"));

                //重命名StreamingAssets文件
                Util.Rename(Application.streamingAssetsPath + "/StreamingAssets", Application.streamingAssetsPath + "/" + Util.GetFileName("StreamingAssets"));

                //重命名StreamingAssets.manifest文件
                Util.Rename(Application.streamingAssetsPath + "/StreamingAssets.manifest", Application.streamingAssetsPath + "/" + Util.GetFileName("StreamingAssets.manifest"));
            }

            LuaRename();
            BuildFileIndex();
            BuildCS();
            DeleteManifestFiles(new DirectoryInfo(streamPath));
            UnityEngine.Debug.Log("删除Manifest文件");
            AssetDatabase.Refresh();
        }

        public static void BuildLua(BuildTarget target)
        {
            map.Clear();
            AssignBundleName();
            AssignAtlasBundleName();
            maps.Clear();
            HandleLuaBundle();

            string streamPath = Application.streamingAssetsPath;

            BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle |
                                            BuildAssetBundleOptions.UncompressedAssetBundle;
            BuildPipeline.BuildAssetBundles(streamPath, maps.ToArray(), options, target);

            string streamDir = Application.dataPath + "/" + GameConst.LuaTempDir;
            if (Directory.Exists(streamDir)) Directory.Delete(streamDir, true);
            AssetDatabase.Refresh();
            //重命名StreamingAssets文件
            string name = Application.streamingAssetsPath + "/" + Util.GetFileName("StreamingAssets");
            if (File.Exists(name))
                File.Delete(name);
            Util.Rename(Application.streamingAssetsPath + "/StreamingAssets", name);

            name = Application.streamingAssetsPath + "/" + Util.GetFileName("StreamingAssets.manifest");
            if (File.Exists(name))
                File.Delete(name);
            //重命名StreamingAssets.manifest文件
            Util.Rename(Application.streamingAssetsPath + "/StreamingAssets.manifest", name);

            LuaRename();
            BuildFileIndex();
            BuildCS();
            AssetDatabase.Refresh();
        }

        static void AddBuildMap(string bundleName, string pattern, string path)
        {
            string[] files = Directory.GetFiles(path, pattern);
            if (files.Length == 0) return;

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetNames = files;
            maps.Add(build);
        }

        static void AddBuildMapLua()
        {
            string[] files = Directory.GetFiles("Assets/" + GameConst.LuaTempDir, "*.bytes");
            string[] platfiles = Directory.GetFiles("Assets/" + GameConst.LuaTempDir + "/platform", "*.bytes");
            List<string> list = new List<string>(files);
            list.AddRange(platfiles);
            files = list.ToArray();
            string bundleName = "lua/lua" + GameConst.ExtName;
            if (files.Length == 0) return;

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace('\\', '/');
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName;
            build.assetNames = files;
            maps.Add(build);
        }

        /// <summary>
        /// 处理Lua代码包
        /// </summary>
        static void HandleLuaBundle()
        {
            string streamDir = Application.dataPath + "/" + GameConst.LuaTempDir;
            if (!Directory.Exists(streamDir)) Directory.CreateDirectory(streamDir);

            string[] srcDirs = { LuaConst.luaDir, LuaConst.toluaDir, LuaConst.platformDir };
            for (int i = 0; i < srcDirs.Length; i++)
            {
                ToLuaMenu.CopyLuaBytesFiles(srcDirs[i], streamDir);
            }
            string[] dirs = Directory.GetDirectories(streamDir, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirs.Length; i++)
            {
                string name = dirs[i].Replace(streamDir, string.Empty);
                name = name.Replace('\\', '_').Replace('/', '_');
                name = "lua/lua_" + name.ToLower() + GameConst.ExtName;

                string path = "Assets" + dirs[i].Replace(Application.dataPath, "");
                AddBuildMap(name, "*.bytes", path);
            }
            AddBuildMap("lua/lua" + GameConst.ExtName, "*.bytes", "Assets/" + GameConst.LuaTempDir);

            AssetDatabase.Refresh();
        }

        static void BuildFileIndex()
        {
            string resPath = AppDataPath + "/StreamingAssets/";
            ///----------------------创建文件列表-----------------------
            string newFilePath = resPath + "/" + Util.md5("files.txt");
            if (File.Exists(newFilePath)) File.Delete(newFilePath);

            paths.Clear(); files.Clear();
            Recursive(resPath);

            //FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
            //StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine("version-" + GetVersionNum());
            //VersionNumAdd();
            //for (int i = 0; i < files.Count; i++)
            //{
            //    string file = files[i];
            //    string ext = Path.GetExtension(file);
            //    if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.Contains(".manifest")) continue;

            //    string md5 = Util.md5file(file);

            //    string value = file.Replace(resPath, string.Empty);
            //    sw.WriteLine(value + "|" + md5);
            //}
            //sw.Close(); fs.Close();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("version-" + GetVersionNum());
            VersionNumAdd();
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string ext = Path.GetExtension(file);
                if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.Contains(".manifest")) continue;

                string md5 = Util.md5file(file);

                string value = file.Replace(resPath, string.Empty);
                sb.AppendLine(value + "|" + md5);
            }
            Util.SaveToBinaryFile(sb.ToString(), newFilePath);
        }

        /// <summary>
        /// 数据目录
        /// </summary>
        static string AppDataPath
        {
            get { return Application.dataPath.ToLower(); }
        }

        /// <summary>
        /// 遍历目录及其子目录
        /// </summary>
        static void Recursive(string path)
        {
            string[] names = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string filename in names)
            {
                string ext = Path.GetExtension(filename);
                if (ext.Equals(".meta")) continue;
                files.Add(filename.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                paths.Add(dir.Replace('\\', '/'));
                Recursive(dir);
            }
        }

        static int GetVersionNum()
        {
            return PlayerPrefs.GetInt("VersionNum", 1);
        }

        static void VersionNumAdd()
        {
            PlayerPrefs.SetInt("VersionNum", GetVersionNum() + 1);
        }

        [MenuItem("Packager/UploadAssetsToServer")]
        static void UploadAssetsToServer()
        {
            Process pr1 = Process.Start(Application.dataPath.Replace("Assets", "") + "upload.bat");
        }

        [MenuItem("Packager/显示files.txt")]
        static void ShowFilesMD5()
        {
            string resPath = AppDataPath + "/StreamingAssets/" + Util.md5("files.txt");
            string content = Util.ReadFromBinaryFile(resPath);
            string[] strs = Util.UnpackTextContent(content);
            for (int i = 0; i < strs.Length; i++)
            {
                UnityEngine.Debug.Log(strs[i]);
            }
        }

        [MenuItem("Packager/显示filemd5.txt")]
        static void ShowFilemd5MD5()
        {
            string resPath = AppDataPath + "/StreamingAssets/" + Util.md5("filemd5.txt");
            Dictionary<string, string> dic = Util.ReadDicFromBinaryFile(resPath);
            string content = Util.ReadFromBinaryFile(resPath);
            string[] strs = Util.UnpackTextContent(content);
            for (int i = 0; i < strs.Length; i++)
            {
                UnityEngine.Debug.Log(strs[i]);
            }
        }

        [MenuItem("Packager/删除Manifest文件")]
        static void DeleteAllManifest()
        {
            DeleteManifestFiles(new DirectoryInfo(Application.streamingAssetsPath));
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("删除Manifest文件");
        }

        static Dictionary<string, string> map = new Dictionary<string, string>();
        public static void BuildCS()
        {
            string resPath = AppDataPath + "/StreamingAssets/";
            string newFilePath = resPath + "/" + Util.md5("filemd5.txt");

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in map)
            {
                sb.AppendLine(string.Format("{0}|{1}", kv.Key, kv.Value));
            }
            Util.SaveToBinaryFile(sb.ToString(), newFilePath);
        }

        public static void LuaRename()
        {
            string resPath = AppDataPath + "/StreamingAssets/lua/";
            DirectoryInfo dirInfo = new DirectoryInfo(resPath);
            foreach (FileInfo info in dirInfo.GetFiles())
            {
                string name = info.Name;
                string extname = Path.GetExtension(name);
                if (extname != ".meta" && extname != ".manifest")
                    extname = string.Empty;
                string md5 = Util.md5(name);
                map.Add(name, md5);
                info.MoveTo(resPath + md5 + extname);
            }
        }

        public static void DeleteManifestFiles(DirectoryInfo dir)
        {
            foreach (FileInfo fileInfo in dir.GetFiles())
            {
                if (Path.GetExtension(fileInfo.FullName) == ".manifest")
                {
                    File.Delete(fileInfo.FullName);
                }
            }

            foreach (DirectoryInfo dirInfo in dir.GetDirectories())
            {
                DeleteManifestFiles(dirInfo);
            }
        }
    }
#endif

}
