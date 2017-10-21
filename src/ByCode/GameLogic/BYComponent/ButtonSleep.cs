using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[RequireComponent(typeof(Button))]
public class ButtonSleep : MonoBehaviour, IPointerClickHandler
{
    public float duration = 0.5f;
    private Button button;
    private bool sleeping = false;
    

    void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    void OnDisable()
    {
        button.enabled = true;
        sleeping = false;
     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_STANDALONE_WIN
        if (eventData.pointerId == -1)
        {
#endif
            if (button.interactable)
                Sleep(duration);
#if UNITY_STANDALONE_WIN
        }
#endif
    }

    public void Sleep(float time)
    {
        if (gameObject.activeInHierarchy)
        {
            button.enabled = false;
            if (sleeping)
            {
                return;
            }
            sleeping = true;
            StartCoroutine(DoSleep(time));
        }
    }

    IEnumerator DoSleep(float time)
    {
        yield return new WaitForSeconds(time);
        button.enabled = true;
        sleeping = false;
    }
}
