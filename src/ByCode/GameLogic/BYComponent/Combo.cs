using UnityEngine;
using System.Collections;
using Manager;

namespace UnityDLL.GameLogic.BYComponent
{
    public class Combo : MonoBehaviour
    {

        private MeshRenderer meshRenderer;
        private float t = 0;

        void Awake()
        {
            meshRenderer = transform.FindChild("text_high").GetComponent<MeshRenderer>();
        }

        /*float value;
        void OnGUI()
        {
            value = GUILayout.HorizontalSlider(value, 0, 1, GUILayout.Width(300));
            T(value);
        }*/

        void Update()
        {
            meshRenderer.material.SetFloat("_Slice", t);
            meshRenderer.material.SetFloat("scale", meshRenderer.bounds.size.x);
            meshRenderer.material.SetFloat("max_x", meshRenderer.bounds.center.x + meshRenderer.bounds.extents.x);
            meshRenderer.material.SetFloat("max_z", meshRenderer.bounds.center.z + meshRenderer.bounds.extents.z);
        }

        public void T(float value)
        {
            t = Mathf.Lerp(-0.1f, 1.1f, value);
        }

        public static int Attach(int transformId)
        {
            return ObjectManager.AddComponent(transformId, typeof(Combo));
        }

        public static void T(int transformId, int componentId, float value)
        {
            ObjectManager.GetComponent<Combo>(transformId, componentId).T(value);
        }
    }
}