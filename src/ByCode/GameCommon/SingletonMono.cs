using UnityEngine;
using System.Collections;

/// <summary>
/// unity MonoBehaviour单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    protected static T _this = null;
    private static GameObject _gameObj = null;
    private static string gobjName = "SingletonGameObject";
    /// <summary>
    /// 取得当前对象的一个单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_this == null)
            {
                if (_gameObj == null)
                {
                    _gameObj = GameObject.Find(gobjName);
                    if (_gameObj == null)
                    {
                        _gameObj = new GameObject();
                        _gameObj.name = gobjName;
                        GameObject.DontDestroyOnLoad(_gameObj);
                    }
                }
                _this = _gameObj.AddComponent<T>();
                _this.SendMessage("OnCreated", SendMessageOptions.DontRequireReceiver);
                //_gameObj.name = _this.GetType().Name;
            }
            return _this;
        }
    }
    protected virtual void OnCreated()
    {

    }
}
/// <summary>
/// C#单例模板
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonObject<T> where T : class,new()
{
    protected static T _this = null;
    /// <summary>
    /// 取得当前对象的一个单例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_this == null)
            {
                _this = new T();
            }
            return _this;
        }
    }

}
 