using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test : MonoBehaviour {

    public AnimationCurve curve;

    public float timer;

	void Start () {

    }
	
	void Update () {
        timer += Time.deltaTime;
        Time.timeScale = curve.Evaluate(timer);
	}

    void OnGUI()
    {
        if (GUILayout.Button("biu"))
        {
            timer = 0;
        }
    }
}
