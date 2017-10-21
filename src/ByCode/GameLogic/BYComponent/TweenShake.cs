using GameCommon;
using UnityEngine;

namespace UnityDLL.GameLogic.BYComponent
{
    public class TweenShake : MonoBehaviour
    {
        public bool isStart = false;
        public float duration = 0.12f;
        public float radius = 0.3f;
        public float speed = 160;
        public Vector3 direction;
        public Type type = Type.Circle;

        private Vector3 orgPos;
        private Vector3 pos;
        private float timer = 0;
        private Vector3 dir;

        public static TweenShake Create(GameObject go, float duration, float radius, float speed, Type type, bool isStart)
        {
            TweenShake tween = null;
            if ((tween = go.GetComponent<TweenShake>()) == null)
            {
                tween = go.AddComponent<TweenShake>();
            }
            tween.duration = duration;
            tween.radius = radius;
            tween.speed = speed;
            tween.type = type;
            tween.isStart = isStart;
            return tween;
        }

        public void Play()
        {
            this.isStart = true;
        }

        public void PlayWithDirection(Vector3 dir, float duration)
        {
            type = Type.Direction;
            this.direction = -dir;
            this.duration = duration;
            isStart = true;
        }

        void Awake()
        {
            orgPos = transform.position;
            if (type == Type.Circle)
            {
                dir = Vector3.up * radius;
            }
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
                isStart = false;
                transform.position = orgPos;
                timer = 0;
            }
            else
            {
                if (type == Type.Random)
                {
                    pos = Random.insideUnitCircle * radius;
                }
                else if (type == Type.Circle)
                {
                    dir = CommonLib.RotateAsZ(dir, speed);
                    pos = orgPos + dir;
                }
                else if (type == Type.Direction)
                {
                    pos = orgPos + direction * (Random.value * 0.8f + 0.2f);
                }
                pos.z = orgPos.z;
                transform.position = pos;
            }
        }

        public enum Type
        {
            Random = 1,
            Circle = 2,
            Direction = 3,
        }

        public static Type GetShakeType(int type)
        {
            return (Type)type;
        }
    }
}
