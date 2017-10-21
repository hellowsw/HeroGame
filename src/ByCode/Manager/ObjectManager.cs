using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

#if true

namespace Manager
{
    public class ObjectManager
    {
        private static Dictionary<int, ObjectData> m_objects = new Dictionary<int, ObjectData>();

        public static int Instantiate(string path, string sourceName)
        {
            GameObject obj = ResourceManager.InstantiateFromResources(path, sourceName) as GameObject;
            if (obj != null)
                return Add(obj.transform);
            return 0;
        }

        public static int NewGameObject(string name)
        {
            GameObject obj = new GameObject(name);
            return Add(obj.transform);
        }

        public static int Find(string name)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
                return Add(obj.transform);
            return 0;
        }

        public static int FindWithTag(string tag)
        {
            GameObject obj = GameObject.FindWithTag(tag);
            if (obj != null)
                return Add(obj.transform);
            return 0;
        }

        public static int Add(Transform transform)
        {
            if (!m_objects.ContainsKey(transform.GetInstanceID()))
            {
                m_objects.Add(transform.GetInstanceID(), new ObjectData(transform));
            }
            return transform.GetInstanceID();
        }

        public static int AddFromChild(int transformId, string childName)
        {
            Transform transform = Get(transformId).FindChild(childName);
            if (!m_objects.ContainsKey(transform.GetInstanceID()))
            {
                m_objects.Add(transform.GetInstanceID(), new ObjectData(transform));
            }
            return transform.GetInstanceID();
        }

        public static Transform Get(int id)
        {
            if (m_objects.ContainsKey(id))
            {
                return m_objects[id].Get();
            }
            return null;
        }

        public static Transform Get(int id, int childId)
        {
            if (childId != 0)
                return GetChild(id, childId);
            else
                return Get(id);
        }

        public static Transform GetChild(int parentId, int childId)
        {
            if (m_objects.ContainsKey(parentId))
            {
                return m_objects[parentId].GetChild(childId);
            }
            return null;
        }

        public static void Delete(int id)
        {
            if (m_objects.ContainsKey(id))
            {
                m_objects[id].Release();
                m_objects.Remove(id);
            }
        }

        public static void SetPosition(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                trans.position = new Vector3(x, y, z);
        }

        public static void Move(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                trans.position += new Vector3(x, y, z);
        }

        public static void GetPosition(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.position.x;
                y = trans.position.y;
                z = trans.position.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetPositionX(int transformId, int childId, out float x)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.position.x;
            }
            else
            {
                x = 0;
            }
        }

        public static void GetPositionY(int transformId, int childId, out float y)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                y = trans.position.y;
            }
            else
            {
                y = 0;
            }
        }

        public static void GetPositionZ(int transformId, int childId, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                z = trans.position.z;
            }
            else
            {
                z = 0;
            }
        }

        public static void SetLocalPosition(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                trans.localPosition = new Vector3(x, y, z);
        }

        public static void GetLocalPosition(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.localPosition.x;
                y = trans.localPosition.y;
                z = trans.localPosition.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetLocalPositionX(int transformId, int childId, out float x)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.localPosition.x;
            }
            else
            {
                x = 0;
            }
        }

        public static void GetLocalPositionY(int transformId, int childId, out float y)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                y = trans.localPosition.y;
            }
            else
            {
                y = 0;
            }
        }

        public static void GetLocalPositionZ(int transformId, int childId, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                z = trans.localPosition.z;
            }
            else
            {
                z = 0;
            }
        }

        public static void GetForward(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.forward.x;
                y = trans.forward.y;
                z = trans.forward.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetRight(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.right.x;
                y = trans.right.y;
                z = trans.right.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetUp(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.up.x;
                y = trans.up.y;
                z = trans.up.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void SetLocalScale(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                trans.localScale = new Vector3(x, y, z);
        }

        public static void GetLocalScale(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.localScale.x;
                y = trans.localScale.y;
                z = trans.localScale.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetLocalScaleX(int transformId, int childId, out float x)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.localScale.x;
            }
            else
            {
                x = 0;
            }
        }

        public static void GetLocalScaleY(int transformId, int childId, out float y)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                y = trans.localScale.y;
            }
            else
            {
                y = 0;
            }
        }

        public static void GetLocalScaleZ(int transformId, int childId, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                z = trans.localScale.z;
            }
            else
            {
                z = 0;
            }
        }

        public static void SetEulerAngles(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.eulerAngles = new Vector3(x, y, z);
            }
        }

        public static void GetEulerAngles(int transformId, int childId, out float x, out float y, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.eulerAngles.x;
                y = trans.eulerAngles.y;
                z = trans.eulerAngles.z;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
            }
        }

        public static void GetEulerAnglesX(int transformId, int childId, out float x)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.eulerAngles.x;
            }
            else
            {
                x = 0;
            }
        }

        public static void GetEulerAnglesY(int transformId, int childId, out float y)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                y = trans.eulerAngles.y;
            }
            else
            {
                y = 0;
            }
        }

        public static void GetEulerAnglesZ(int transformId, int childId, out float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                z = trans.eulerAngles.z;
            }
            else
            {
                z = 0;
            }
        }

        public static void SetRotation(int transformId, int childId, float x, float y, float z, float w)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.rotation = new Quaternion(x, y, z, w);
            }
        }

        public static void GetRotation(int transformId, int childId, out float x, out float y, out float z, out float w)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.rotation.x;
                y = trans.rotation.y;
                z = trans.rotation.z;
                w = trans.rotation.w;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
                w = 0;
            }
        }

        public static void SetLocalRotation(int transformId, int childId, float x, float y, float z, float w)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.localRotation = new Quaternion(x, y, z, w);
            }
        }

        public static void GetLocalRotation(int transformId, int childId, out float x, out float y, out float z, out float w)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                x = trans.localRotation.x;
                y = trans.localRotation.y;
                z = trans.localRotation.z;
                w = trans.localRotation.w;
            }
            else
            {
                x = 0;
                y = 0;
                z = 0;
                w = 0;
            }
        }

        public static void Rotate(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.Rotate(new Vector3(x, y, z));
            }
        }

        public static void Rotate(int transformId, int childId, float axisX, float axisY, float axisZ, float angle)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.Rotate(new Vector3(axisX, axisY, axisZ), angle);
            }
        }

        public static void LookAt(int transformId, int childId, float x, float y, float z)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.LookAt(new Vector3(x, y, z));
            }
        }

        public static void SetAsFirstSibling(int transformId, int childId)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.SetAsFirstSibling();
            }
        }

        public static void SetAsLastSibling(int transformId, int childId)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
            {
                trans.SetAsLastSibling();
            }
        }

        public static void SetParent(int transformId, int parentId)
        {
            Transform trans = Get(transformId);
            Transform parent = Get(parentId);
            if (trans != null && parent != null)
                trans.SetParent(parent);
        }

        public static int FindChild(int transformId, string name)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.FindChild(name);
            }
            return 0;
        }

        public static void SetActive(int transformId, int childId, int active)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                trans.gameObject.SetActive(active == 1);
        }

        public static int IsActive(int transformId, int childId)
        {
            Transform trans = Get(transformId, childId);
            if (trans != null)
                return trans.gameObject.activeInHierarchy ? 1 : 0;
            else
                return 0;
        }

        public static int AddComponent(int transformId, Type type)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.AddComponent(type);
            }
            return 0;
        }

        public static int AddComponent(int transformId, Component component)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.AddComponent(component);
            }
            return 0;
        }

        public static void DestroyComponent(int transformId, int componentId)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                data.DestroyComponent(componentId);
            }
        }

        public static T GetComponent<T>(int transformId, int componentId) where T : Component
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.GetComponent<T>(componentId);
            }
            return default(T);
        }

        public static int GetComponent(int transformId, string componentName)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.GetComponent(componentName);
            }
            return 0;
        }

        public static int GetComponentInChildren(int transformId, string componentName)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.GetComponentInChildren(componentName);
            }
            return 0;
        }

        public static int GetComponentInChildren(int transformId, string childName, string componentName)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.GetComponentInChildren(childName, componentName);
            }
            return 0;
        }

        public static void SetName(int transformId, string name)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                data.SetName(name);
            }
        }

        public static string GetName(int transformId)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                return data.GetName();
            }
            return null;
        }

        public static void SetLayer(int transformId, int layer)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                data.SetLayer(layer);
            }
        }

        public static int GetLayer(int transformId)
        {
            ObjectData data = null;
            m_objects.TryGetValue(transformId, out data);
            if (data != null)
            {
                data.GetLayer();
            }
            return 0;
        }

        public static void Clear()
        {
            foreach (ObjectData objData in m_objects.Values)
            {
                if (objData != null)
                    objData.Release();
            }
            m_objects.Clear();
        }
    }

    public class ObjectData
    {
        private Transform transform;
        private GameObject gameObject;
        private Dictionary<int, Transform> childs = new Dictionary<int, Transform>();
        private Dictionary<int, Component> components = new Dictionary<int, Component>();

        public ObjectData(Transform transform)
        {
            this.transform = transform;
            this.gameObject = transform.gameObject;
        }

        public Transform Get()
        {
            return transform;
        }

        public Transform GetChild(int childId)
        {
            if (childs.ContainsKey(childId))
            {
                return childs[childId];
            }
            return null;
        }

        public T GetComponent<T>(int componentId) where T : Component
        {
            if (components.ContainsKey(componentId))
            {
                return components[componentId] as T;
            }
            return default(T);
        }

        public int GetComponent(string componentName)
        {
            Component comp = transform.GetComponent(componentName);
            if (comp == null)
                return 0;
            if (!components.ContainsKey(comp.GetInstanceID()))
            {
                components.Add(comp.GetInstanceID(), comp);
            }
            return comp.GetInstanceID();
        }

        public int GetComponentInChildren(string componentName)
        {
            Component comp = GetComponentInChildren(transform, componentName);
            if (!components.ContainsKey(comp.GetInstanceID()))
            {
                components.Add(comp.GetInstanceID(), comp);
            }
            return comp.GetInstanceID();
        }


        //得到子物体中的组件
        public static Component GetComponentInChildren(Transform trans, string name)
        {
            Component component = null;
            foreach (Transform t in trans.GetComponentsInChildren<Transform>())
            {
                if ((component = t.GetComponent(name)) != null)
                    return component;
            }
            return null;
        }

        public int GetComponentInChildren(string childName, string componentName)
        {
            Component comp = transform.FindChild(childName).GetComponent(componentName);
            if (!components.ContainsKey(comp.GetInstanceID()))
            {
                components.Add(comp.GetInstanceID(), comp);
            }
            return comp.GetInstanceID();
        }

        public int AddComponent(Type type)
        {
            Component comp = gameObject.AddComponent(type);
            if (!components.ContainsKey(comp.GetInstanceID()))
            {
                components.Add(comp.GetInstanceID(), comp);
            }
            return comp.GetInstanceID();
        }

        public int AddComponent(Component component)
        {
            if (!components.ContainsKey(component.GetInstanceID()))
            {
                components.Add(component.GetInstanceID(), component);
            }
            return component.GetInstanceID();
        }

        public void DestroyComponent(int componentId)
        {
            if (components.ContainsKey(componentId))
                GameObject.Destroy(components[componentId]);
        }

        public void Release()
        {
            GameObject.Destroy(gameObject);
            childs.Clear();
        }

        public int FindChild(string name)
        {
            Transform child = transform.FindChild(name);
            if (child != null)
            {
                if (!childs.ContainsKey(child.GetInstanceID()))
                {
                    childs.Add(child.GetInstanceID(), child);
                }
                return child.GetInstanceID();
            }
            return 0;
        }

        public Transform GetChildTransform(int childId)
        {
            if (childs.ContainsKey(childId))
            {
                return childs[childId];
            }
            return null;
        }

        public GameObject GetChildGameObject(int childId)
        {
            if (childs.ContainsKey(childId))
            {
                return childs[childId].gameObject;
            }
            return null;
        }

        public void SetName(string name)
        {
            gameObject.name = name;
        }

        public string GetName()
        {
            return gameObject.name;
        }

        public void SetLayer(int layer)
        {
            SetLayer(gameObject, layer);
        }

        private void SetLayer(GameObject go, int layer)
        {
            Transform trans = go.transform;
            for (int i = 0; i < trans.childCount; i++)
            {
                SetLayer(trans.GetChild(i).gameObject, layer);
            }
            go.layer = layer;
        }

        public int GetLayer()
        {
            return gameObject.layer;
        }
    }

}

#endif