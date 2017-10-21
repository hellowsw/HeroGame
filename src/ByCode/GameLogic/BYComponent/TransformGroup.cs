using UnityEngine;

namespace GameLogic.BYComponent
{
    public class TransformGroup : MonoBehaviour
    {
        public Transform[] transforms;

        public bool inUpdate = false;

        private Vector3[] scales;

        private Transform trans;

        private float orgx;

        private float orgy;

        private float ratex;

        private float ratey;

        private Vector3 temp;

        void Awake()
        {
            trans = transform;
            orgx = trans.localScale.x;
            orgy = trans.localScale.y;
            temp.z = 1;

            scales = new Vector3[transforms.Length];
            for (int i = 0; i < scales.Length; i++)
            {
                scales[i] = transforms[i].localScale;
            }
        }

        void Update()
        {
            if (inUpdate && (trans.localScale.x != orgx || trans.localScale.y != orgy))
            {
                ratex = (trans.localScale.x - orgx) / orgx;
                ratey = (trans.localScale.y - orgy) / orgy;
                for (int i = 0; i < scales.Length; i++)
                {
                    temp.x = scales[i].x + scales[i].x * ratex;
                    temp.y = scales[i].y + scales[i].y * ratey;
                    transforms[i].localScale = temp;
                }
            }
        }

        public void Scale(float factor)
        {
            for (int i = 0; i < scales.Length; i++)
            {
                transforms[i].localScale *= factor;
            }
        }

        public void ResetScale()
        {
            for (int i = 0; i < scales.Length; i++)
            {
                transforms[i].localScale = scales[i];
            }
        }

        public void SetActive(bool active)
        {
            for (int i = 0; i < scales.Length; i++)
            {
                transforms[i].gameObject.SetActive(active);
            }
        }
    }
}
