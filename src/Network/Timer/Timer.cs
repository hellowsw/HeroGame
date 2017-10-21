using Network.Pool;
using System;

namespace Network.Timer
{
    public delegate void ReAddTimerHandler(Timer timer);
    public class Timer
    {
        public static ReAddTimerHandler ReAddHandler;

        public Timer() { }
        public delegate void TimerHandler(object[] parameters);
        
		public uint seqID;
		public bool running;
        public TimerHandler handler;

        public object[] parameters;
		public UInt64 runTick;
        public int delay;
        public bool loop;
        public Timer next;

        // 2016.5.3
        // For Lua timer key
        public ulong timerID;
        public unsafe IntPtr GetIDPtr()
        {
            fixed(ulong* idPtr = &timerID)
            {
                return (IntPtr)idPtr;
            }
        }
        public static unsafe ulong IDFromPtr(IntPtr idPtr)
        {
            return *((ulong*)idPtr);
        }

        internal void Reset()
        {
            seqID = 0;
            running = false;
            handler = null;
            parameters = null;
            next = null;
            loop = false;
        }
    }

    // Timer列表
    public class TimerList
	{
		public Timer head;
        public Timer tail;

        public bool Execute(UInt64 now, ref int maxCount)
		{
            Timer runningTimer = null;
			while(head != null && maxCount > 0)
			{
                runningTimer = head;
				maxCount --;

                runningTimer.running = true;
                
                runningTimer.handler(runningTimer.parameters);

                runningTimer.running = false;

                head = runningTimer.next;

                if (runningTimer.loop)
                    Timer.ReAddHandler(runningTimer);
                else
                {
                    runningTimer.Reset();
                    TimerPool.Instance.Free(runningTimer);
                }
            }
			if(head == null)
			{
				tail = null;
				return false;
			}
			return true;
		}

        public void Clear()
        {
            Timer currTimer = null;
            while (head != null)
            {
                currTimer = head;
                head = currTimer.next;
                currTimer.Reset();
                TimerPool.Instance.Free(currTimer);
            }
            head = null;
			tail = null;
		}

        public void AddTimer(Timer timer)
		{
			timer.next = null;
			if(tail == null)
			{
				head = tail = timer;
				return;
			}

			tail.next = timer;
			tail = timer;
		}

        public Timer GetTimer(uint timerSeqID)
		{
			Timer currTimer = head;
			while(currTimer != null)
			{
				if(currTimer.seqID == timerSeqID)
                    return currTimer;
				currTimer = currTimer.next;
			}
			return null;
		}

        public bool DeleteTimer(uint timerSeqID)
		{
			Timer prevTimer = null;
			Timer currTimer = head;
			Timer nextTimer = null;
			while(currTimer != null)
			{
				nextTimer = currTimer.next;
                if (currTimer.seqID != timerSeqID)
				{
					prevTimer = currTimer;
					currTimer = nextTimer;
					continue;
                }

                // 避免执行中的重复添加
                currTimer.loop = false;

                // 正在执行中,忽略删除操作
                if (currTimer.running)
					return true;

				if(prevTimer == null)
					head = nextTimer;
				else
					prevTimer.next = nextTimer;

				if(nextTimer == null)
					tail = prevTimer;

                currTimer.Reset();
                TimerPool.Instance.Free(currTimer);
				return true;
			}
			return false;
		}
	}

    public class TimerPool
    {
        private static TimerPool instance;
        public static TimerPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new TimerPool();
                return instance;
            }
        }

        LookAsideList timerList = new LookAsideList();
        public Timer Get()
        {
            return timerList.Pop<Timer>();
        }
        public void Free(Timer timer)
        {
            TimerParamPool.Instance.Free(timer.parameters);
            timer.parameters = null;
            timerList.Push(timer);
        }
    }

    public class TimerParamPool
    {
        private static TimerParamPool instance;
        public static TimerParamPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new TimerParamPool();
                return instance;
            }
        }

        Map<int, LookAsideArrayList> pool = new Map<int, LookAsideArrayList>();//Key: paramCount
        public object[] Get(int paramCount)
        {
            LookAsideArrayList lst = null;
            if (pool.TryGetValue(paramCount, out lst))
                return lst.Pop<object>();

            lst = new LookAsideArrayList(paramCount, true);
            pool.Add(paramCount, lst);
            return lst.Pop<object>();
        }
        public void Free(object[] parameters)
        {
            if (parameters == null)
                return;

            var paramCount = parameters.Length;
            LookAsideArrayList lst = null;
            if (!pool.TryGetValue(paramCount, out lst))
            {
                lst = new LookAsideArrayList(paramCount, true);
                pool.Add(paramCount, lst);
            }
            lst.Push(parameters);
        }
    }
}
