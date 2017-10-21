using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityDLL.GameLogic.BYComponent;

namespace GameLogic.BYComponent
{
    public class UILoadingRedpaper : MonoBehaviour, IPointerExitHandler
    {
        public UILoading uiloading;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            uiloading.HideRedpaper();
        }
    }
}
