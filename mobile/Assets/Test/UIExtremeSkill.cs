using UnityEngine;
using System.Collections;
using UnityDLL.GameLogic.Common;

public class UIExtremeSkill : MonoBehaviour {

    public Material mat;
    public Transform _name;
    public Transform face;
    public Transform bg;
    public Slowly slowly;

    public float delay = 0.1f;

	void Start () {
        DOTweenUtil.DOMoveX(face, 0, 0.6f, 0 + delay);
        DOTweenUtil.DOMoveX(face, 10, 0.6f, 1.8f + delay);
        DOTweenUtil.DOMoveX(_name, 0, 0.6f, 0 + delay);
        DOTweenUtil.DOMoveX(_name, -10, 0.6f, 1.8f + delay);
        DOTweenUtil.DOScale(bg, new Vector3(1, 0, 0), 0.1f, 2.1f + delay);
        slowly.timer = 0;
    }

	void Update () {
        mat.SetFloat("_time", Time.unscaledTime);
    }
}
