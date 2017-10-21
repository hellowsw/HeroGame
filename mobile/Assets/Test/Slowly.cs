using UnityEngine;
using System.Collections;

public class Slowly : MonoBehaviour {

    public AnimationCurve curve;

    public float timer;
    public float scale = 0.5f;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        Time.timeScale = curve.Evaluate(timer * scale);
    }

    //void OnGUI()
    //{
    //    if (GUILayout.Button("biu"))
    //    {
    //        timer = 0;
    //    }
    //}
}
