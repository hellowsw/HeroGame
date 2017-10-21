using UnityEngine;
using System.Collections;

public class AnimationEventScript : MonoBehaviour {

    public GameObject go;

    void Show(float autoHideTime)
    {
        go.SetActive(true);
        if (autoHideTime != -1)
        {
            StartCoroutine(DoEnable(false, autoHideTime));
        }
    }

    void Hide(float autoShowTime)
    {
        go.SetActive(false);
        if (autoShowTime != -1)
        {
            StartCoroutine(DoEnable(true, autoShowTime));
        }
    }

    IEnumerator DoEnable(bool enable, float time)
    {
        yield return new WaitForSeconds(time);
        go.SetActive(enable);
    }
}
