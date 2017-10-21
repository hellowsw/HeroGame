using UnityEngine;

namespace GameLogic.BYComponent
{
    public class MaterialFadeColor : MonoBehaviour
    {
        public Color from;
        public Color to;
        public float duration = 2;
        public EColor colorType = EColor.AddTexColor;
        public AnimationCurve animationCurve;
        public ELoopType loopType = ELoopType.Pingpong;
        public bool isStart = false;

        private string paramName;
        private Renderer render;
        private float timer;
        private int dir = 0;

        void Start()
        {
            render = GetComponentInChildren<Renderer>();
            switch (colorType)
            {
                case EColor.MainTexColor:
                    paramName = "_Color";
                    break;
                case EColor.AddTexColor:
                    paramName = "_AddTexColor";
                    break;
            }
        }

        void Update()
        {
            if (!isStart)
                return;

            if (loopType == ELoopType.Normal)
            {
                timer += Time.deltaTime;
                if (timer > duration)
                {
                    timer = 0;
                }
            }
            else if (loopType == ELoopType.Pingpong)
            {
                if (dir == 0)
                {
                    timer += Time.deltaTime;
                    if (timer > duration)
                    {
                        dir = 1;
                    }
                }
                else if (dir == 1)
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        dir = 0;
                    }
                }
            }

            render.material.SetColor(paramName, Color.Lerp(from, to, animationCurve.Evaluate(timer / duration)));
        }

        public enum ELoopType
        {
            Normal,
            Pingpong,
        }

        public enum EColor
        {
            MainTexColor = 1,
            AddTexColor = 3,
        }
    }
}
