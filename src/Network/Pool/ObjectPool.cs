using System;
using System.Collections.Generic;

namespace Network.Pool
{
    public class LookAsideList
    {
        public LookAsideList() { }
        public LookAsideList(bool autoGenerate)
        {
            this.autoGenerate = autoGenerate;
        }

        public int Capacity = 0;
        public int Count
        {
            get { return stackTop + 1; }
        }

        bool autoGenerate = true;
        List<object> lst = new List<object>();
        int stackTop = -1;

        public T Pop<T>() where T : new()
        {
            if (stackTop < 0)
            {
                if (!autoGenerate)
                    return default(T);
                Push(new T());
            }

            T ret = (T)lst[stackTop];
            lst[stackTop] = default(T);
            --stackTop;
            return ret;
        }

        public bool Push(object t)
        {
            if (Capacity > 0 && Count >= Capacity)
                return false;
            ++stackTop;
            if (stackTop >= lst.Count)
            {
                lst.Add(t);
            }
            else
            {
                lst[stackTop] = t;
            }
            return true;
        }

        public void Flush(FlushHandler handler)
        {
            if (handler != null)
            {
                for (int i = 0; i <= stackTop; ++i)
                {
                    handler(lst[i]);
                }
            }
            lst.Clear();
            stackTop = -1;
        }
    }

    public class LookAsideArrayList
    {
        public LookAsideArrayList(int arrayCount, bool autoGenerate)
        {
            this.arrayCount = arrayCount;
            this.autoGenerate = autoGenerate;
        }

        public int Capacity = 0;
        public int Count
        {
            get { return stackTop + 1; }
        }

        int arrayCount = 0;
        bool autoGenerate = true;
        List<object> lst = new List<object>();
        int stackTop = -1;

        public T[] Pop<T>() where T : new()
        {
            if (stackTop < 0)
            {
                if (!autoGenerate)
                    return null;
                Push(new T[arrayCount]);
            }

            T[] ret = (T[])lst[stackTop];
            lst[stackTop] = default(T);
            --stackTop;
            return ret;
        }

        public bool Push(object t)
        {
            if (Capacity > 0 && Count >= Capacity)
                return false;
            ++stackTop;
            if (stackTop >= lst.Count)
            {
                lst.Add(t);
            }
            else
            {
                lst[stackTop] = t;
            }
            return true;
        }

        public void Flush(FlushHandler handler)
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                if (handler != null)
                    handler(lst[i]);
            }
            lst.Clear();
            stackTop = -1;
        }
    }

    public delegate void FlushHandler(object obj);
    public class ObjectPool
    {
        public ObjectPool() { }
        public ObjectPool(bool autoGenerate)
        {
            this.autoGenerate = autoGenerate;
        }

        bool autoGenerate = true;
        Map<Type, LookAsideList> pool = new Map<Type, LookAsideList>();

        public T Get<T>() where T : new()
        {
            LookAsideList lst = null;
            var type = typeof(T);
            if (pool.TryGetValue(type, out lst))
                return lst.Pop<T>();
            if (!autoGenerate)
                return default(T);

            lst = new LookAsideList(autoGenerate);
            pool.Add(type, lst);
            return lst.Pop<T>();
        }

        public void Free(object t)
        {
            LookAsideList lst = null;
            var type = t.GetType();
            if (pool.TryGetValue(type, out lst))
            {
                lst.Push(t);
                return;
            }

            lst = new LookAsideList(autoGenerate);
            pool.Add(type, lst);
            lst.Push(t);
        }

        public void Flush(FlushHandler handler)
        {
            var iter = pool.GetEnumerator();
            for (; iter.MoveNext(); )
            {
                iter.Current.Value.Flush(handler);
            }
            pool.Clear();
        }

        public void Flush<T>(FlushHandler handler)
        {
            LookAsideList lst = null;
            var type = typeof(T);
            if (pool.TryGetValue(type, out lst))
            {
                lst.Flush(handler);
                pool.Remove(type);
            }
        }
    }
}
