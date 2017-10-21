using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.IO;

public class FontAssigner : Editor {

    public static string fontDir = "font/msyh";

    [MenuItem("Tools/FontAssigner")]
    public static void Assigner()
    {
        Font font = Resources.Load<Font>(fontDir);
        string dir = Application.dataPath + "/Resources/UI/";
        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        foreach (FileInfo fileInfo in dirInfo.GetFiles())
        {
            if (Path.GetExtension(fileInfo.Name) == ".prefab")
            {
                string pName = fileInfo.Name.Replace(Path.GetExtension(".prefab"), "");
                GameObject uiPrefab = (GameObject)Resources.Load("UI/" + pName);
                foreach (Text text in uiPrefab.GetComponentsInChildren<Text>())
                {
                    if (text.font == null)
                    {
                        text.font = font;
                    }
                }
                EditorUtility.SetDirty(uiPrefab);
            }
        }
    }
}
