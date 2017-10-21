using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class SpriteBoundlePackerEditor
{
    static string bundle_path = Application.dataPath + "/Bundle/sprite/";
    static string resource_path = Application.dataPath + "/Resources/sprite/";

    [MenuItem("Tools/Bundle CraeteAtlasPrefabs")]
    static private void CraeteBundleAtlasPrefabs()
    {
        if (Directory.Exists(bundle_path))
            Directory.Delete(bundle_path, true);
        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
        HandleDirectory(rootDirInfo, "/Bundle/");
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Resources CraeteSelectAtlasPrefabs")]
    static private void CraeteBundleAtlasPrefabs2()
    {
        string path = Application.dataPath + "/Resources/sprite/" + Selection.activeObject.name;
        //string assetPath = "/Assets/Atlas/" + Selection.activeObject.name;
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas/" + Selection.activeObject.name);
        HandleDirectory(rootDirInfo, "/Resources/");
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Resources CraeteAtlasPrefabs")]
    static private void CraeteResourcesAtlasPrefabs()
    {
        if (Directory.Exists(resource_path))
            Directory.Delete(resource_path, true);
        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
        HandleDirectory(rootDirInfo, "/Resources/");
        AssetDatabase.Refresh();
    }

    static void HandleDirectory(DirectoryInfo dir, string buildRoot)
    {
        if (dir.FullName.Contains("package"))
        {
            foreach (FileInfo pngFile in dir.GetFiles("*.png", SearchOption.AllDirectories))
            {
                string allPath = pngFile.FullName;
                string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                string targetDir = dir.FullName.Replace("\\", "/").Replace(Application.dataPath, "").Replace("Atlas/", "sprite/").Replace("/package", "");
                allPath = Application.dataPath + buildRoot + targetDir;
                if (!Directory.Exists(allPath))
                {
                    Directory.CreateDirectory(allPath);
                }
                string prefabName = "Assets/" + buildRoot + targetDir + "/" + sprite.name + ".prefab";
                PrefabUtility.CreatePrefab(prefabName, go);
                GameObject.DestroyImmediate(go);
            }
        }

        foreach (DirectoryInfo dirInfo in dir.GetDirectories())
        {
            HandleDirectory(dirInfo, buildRoot);
        }
    }
}