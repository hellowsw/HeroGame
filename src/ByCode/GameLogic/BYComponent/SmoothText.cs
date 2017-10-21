using UnityEngine;
using UnityEngine.UI;

namespace UnityDLL.GameLogic.BYComponent
{
    public class SmoothText : MonoBehaviour
    {
        public int to;
        public float duration;
        public bool isStart = false;

        private float timer = 0;
        private int cur;
        private Text text;

        public static SmoothText Create(GameObject go, float duration, int cur)
        {
            SmoothText smoothText = null;
            if ((smoothText = go.GetComponent<SmoothText>()) == null)
            {
                smoothText = go.AddComponent<SmoothText>();
                smoothText.Init();
            }
            smoothText.cur = cur;
            smoothText.duration = duration;
            smoothText.text.text = cur.ToString();
            return smoothText;
        }

        public void Set(int value)
        {
            cur = value;
            text.text = cur.ToString();
        }

        public void Smooth(int value)
        {
            isStart = true;
            cur = System.Convert.ToInt32(text.text);
            to = value;
        }

        public void Init()
        {
            text = GetComponentInChildren<Text>();
            text.text = cur.ToString();
        }

        void Update()
        {
            if (!isStart)
            {
                return;
            }
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                timer = 0;
                isStart = false;
                text.text = to.ToString();
                cur = to;
            }
            else
            {
                text.text = Mathf.FloorToInt(Mathf.Lerp(cur, to, timer / duration)).ToString();
            }
        }
    }
}