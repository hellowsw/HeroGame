using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    private Material mat;

	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	void Update () {
        mat.SetFloat("t", Time.time % 1000);
	}
}
