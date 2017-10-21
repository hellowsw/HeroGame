using UnityEngine;
using System.Collections;
using System.IO;
using LuaInterface;
using GameCommon;
using GameLogic;
using Manager;

namespace UnityDLL.LuaCustom
{
    /// <summary>
    /// 集成自LuaFileUtils，重写里面的ReadFile，
    /// </summary>
    public class LuaLoader : LuaFileUtils
    {
        private ResourceManager m_resMgr;

        ResourceManager resMgr
        {
            get
            {
                return ResourceManager.Instance;
            }
        }

        // Use this for initialization
        public LuaLoader()
        {
            instance = this;
            beZip = !GameConst.DebugMode;
        }

        /// <summary>
        /// 添加打入Lua代码的AssetBundle
        /// </summary>
        /// <param name="bundle"></param>
        public void AddBundle(string bundleName)
        {
            string url = UpdateManager.Instance.GetRelPath(bundleName);
            //Debug.LogError("lua文件 " + bundleName + " 使用url:" + url);
            AssetBundle bundle = AssetBundle.LoadFromFile(url);
            if (bundle != null)
            {
                if (GameConst.ExtName != string.Empty)
                    bundleName = bundleName.Replace("lua/", "").Replace(GameConst.ExtName, "").Replace(".bytes", "");
                else
                    bundleName = bundleName.Replace("lua/", "").Replace(".bytes", "");
                //Debug.LogError("成功 bundleName:" + bundleName);
                base.AddSearchBundle(bundleName.ToLower(), bundle);
            }
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName)
        {
            return base.ReadFile(fileName);
        }
    }
}