using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityDLL.GameLogic.BYComponent;

namespace GameLogic.BYComponent
{
    public class UILoadingPopup : MonoBehaviour, IPointerEnterHandler
    {
        public UILoading uiloading;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            uiloading.ShowRedpaper();
        }
    }
}
