using UnityEngine;
using UnityEditor;
using System.IO;

public class FindMissingScriptsRecursively : EditorWindow
{
    static int go_count = 0, components_count = 0, missing_count = 0;

    static string dataPath = null;

    static string DataPath {
        get {
            if (dataPath == null)
            {
                dataPath = Application.dataPath.Replace("\\", "/") + "/";
            }
            return dataPath;
        }
    }

    [MenuItem("Window/脚本丢失")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
    }

    [MenuItem("GameObject/查找丢失脚本", false, 12)]
    public static void FindSelectInGameObject()
    {
        FindInSelected();
    }

    [MenuItem("Assets/查找丢失脚本")]
    public static void FindSelectInAssets()
    {
        FindInSelected();
    }

    [MenuItem("Tools/查找丢失脚本")]
    public static void FindToolsMenu()
    {
        FindInResources();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("查找选中物体"))
        {
            FindInSelected();
        }
        if (GUILayout.Button("查找Resources"))
        {
            FindInResources();
        }
    }

    [MenuItem("Tools/查找Resources脚本丢失")]
    public static void FindInResources()
    {
        FindInDir(new DirectoryInfo(Application.dataPath + "/Resources"));
    }

    public static void FindInDir(DirectoryInfo dir)
    {
        string path = null;
        foreach (FileInfo fileInfo in dir.GetFiles())
        {
            if (Path.GetExtension(fileInfo.FullName) == ".prefab")
            {
                path = fileInfo.FullName.Replace("\\", "/").Replace(DataPath + "Resources/", string.Empty).Replace(".prefab", string.Empty);
                GameObject go = Resources.Load(path) as GameObject;
                FindInGO(go);
            }
        }
        foreach (DirectoryInfo dirInfo in dir.GetDirectories())
        {
            FindInDir(dirInfo);
        }
    }

    private static void FindInSelected()
    {
        GameObject[] Go = Selection.gameObjects;
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        foreach (GameObject g in Go)
        {
            FindInGO(g);
        }
        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }


    private static void FindInGO(GameObject g)
    {
        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Object.DestroyImmediate(components[i]);
                Debug.Log(s + " has an empty script attached in position: " + i, g);
            }
        }

        foreach (Transform childT in g.transform)
        {
            FindInGO(childT.gameObject);
        }
    }
}