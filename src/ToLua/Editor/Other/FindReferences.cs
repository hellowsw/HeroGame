using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

#if true
public class FindReferences
{

    [MenuItem("Assets/查找资源引用", false, 10)]
    static private void Find()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = Application.dataPath + AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets", string.Empty);
        if (File.Exists(path))
            HandleFile(path, false);
        else
            HandleDirectory(path, false);
    }

    [MenuItem("Assets/删除没用的图(仅限Atlas下面)", false, 10)]
    static private void Delete()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = Application.dataPath + AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets", string.Empty);
        if (File.Exists(path))
            HandleFile(path, true);
        else
            HandleDirectory(path, true);
        AssetDatabase.Refresh();
    }

    static void HandleFile(string path, bool delete)
    {
        string fullPath = path;
        path = "Assets" + path.Replace(@"\", "/").Replace(Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid))
                return;
            List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

            int count = 0;

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];

                if (Regex.IsMatch(File.ReadAllText(file), guid))
                {
                    count++;
                    Debug.Log(file, AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(GetRelativeAssetsPath(file)));
                }
            }

            if (count > 0)
                Debug.Log("匹配结束");
            else
            {
                string p = Application.dataPath + path.Replace("Assets", string.Empty);
                if (delete && File.Exists(p))
                {
                    File.Delete(p);
                    Debug.LogError("已删除:" + p);
                }
                else
                    Debug.LogError("没有匹配到任何引用:" + p);
            }
        }
    }

    static void HandleDirectory(string dir, bool delete)
    {
        DirectoryInfo info = new DirectoryInfo(dir);
        foreach (FileInfo fileInfo in info.GetFiles())
        {
            HandleFile(fileInfo.FullName, delete);
        }
        foreach (DirectoryInfo dirInfo in info.GetDirectories())
        {
            if (dirInfo.FullName.IndexOf("package") == -1)
                HandleDirectory(dirInfo.FullName, delete);
        }
    }

    [MenuItem("Assets/Find References", true)]
    static private bool VFind()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }

    static private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }
}

#endif