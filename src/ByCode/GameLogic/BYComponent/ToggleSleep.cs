using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[RequireComponent(typeof(Toggle))]
public class ToggleSleep : MonoBehaviour, IPointerClickHandler
{
    public float duration = 0.5f;
    private Toggle button;
    private bool sleeping = false;


    void Awake()
    {
        button = GetComponentInChildren<Toggle>();
    }

    void OnDisable()
    {
        button.interactable = true;
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
            button.interactable = false;
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
        button.interactable = true;
        sleeping = false;
    }
}
