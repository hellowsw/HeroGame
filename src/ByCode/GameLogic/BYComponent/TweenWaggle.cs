using UnityEngine;
using System.Collections;

public class TweenWaggle : MonoBehaviour {

    public float rate;
    public float length;
    public float range;

    private Transform trans;
    private Vector3 euler;

	void Start () {
        trans = transform;
	}
	
	void Update () {
        euler.z = Mathf.Sin(Time.time * rate + length) * range;
        trans.eulerAngles = euler;
    }
}
