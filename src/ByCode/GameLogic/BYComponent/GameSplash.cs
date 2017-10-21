using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityDLL.GameLogic.BYComponent
{
    public class GameSplash : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Launcher());
        }

        IEnumerator Launcher()
        {
            for (int i = 1; i <= 100; i++)
            {
                UILoading.Show(i / 100f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
