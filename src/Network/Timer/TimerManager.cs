using Network.Log;
using System;

namespace Network.Timer
{
    // 时间轮
    public class TimerListWheel
	{
        const ushort arraySize = 256;
        TimerList[] array = new TimerList[arraySize];
		byte index = 0;
		int timerCount = 0;

		int bitSize = 0;
		UInt64 msUint = 0;
		UInt64 baseTick = 0;

        TimerListWheel smallerWheel = null;
        TimerListWheel biggerWheel = null;

	    public TimerListWheel()
        {
            for(int i = 0; i < arraySize; i++)
                array[i] = new TimerList();
        }

        public void Init(
			UInt64 msUnit,
            int bitSize,
            TimerListWheel smallerWheel,
            TimerListWheel biggerWheel,
			UInt64 now)
		{
			this.msUint = msUnit;
            this.bitSize = bitSize;
            this.smallerWheel = smallerWheel;
            this.biggerWheel = biggerWheel;

            if (msUnit == 1)
                baseTick = now;
            else
                baseTick = now + msUnit;
		}

		byte TickIdxDelta(UInt64 runTick)
		{
            Int64 idxDelta = (Int64)(runTick - baseTick);
			idxDelta = idxDelta >> bitSize;
			return (byte)idxDelta;
		}

        public bool Execute(UInt64 now, ref int maxCount)
        {
            // 只允许最小刻度的轮子执行
            if (smallerWheel != null)
                return false;

                // 获取循环次数(根据流逝时间)
		    UInt64 elapsedTime = now - baseTick;
		    UInt64 loopTimes = 1 + elapsedTime;

		    // 更改基准时间(根据循环次数),更改索引值
		    byte nowIndex = index;
		    index += (byte)(elapsedTime);
		    baseTick += elapsedTime;

		    int tempCount;
		    bool ret = false;
		    while(maxCount > 0)
		    {
			    loopTimes--;

			    TimerList nowList = array[nowIndex];//不可使用index - loopTimes
			    tempCount = maxCount;
			    ret = ret | nowList.Execute(now, ref maxCount);
			    timerCount -= tempCount - maxCount;

			    if(loopTimes <= 0)
				    break;
			    if((++nowIndex) == 0)
				    biggerWheel.Turn();
		    }
		    return ret;
		}

		// 转轮
        public void Turn()
		{
            if (smallerWheel == null)
                return;

            TimerList nowList = array[index++];//不可使用index - loop_times

            Timer head = nowList.head;
            Timer nextTimer;
            while (head != null)
            {
                nextTimer = head.next;
                smallerWheel.AddTimer(head);//内部会更改head.next为空
                head = nextTimer;
                timerCount--;
            }

            nowList.head = null;
            nowList.tail = null;
            baseTick += msUint;

            if (index == 0 && biggerWheel != null)
            {
                biggerWheel.Turn();
            }
		}

        public void AddTimer(Timer timer)
		{
			if(smallerWheel != null && (timer.runTick < baseTick))
			{
				smallerWheel.AddTimer(timer);
				return;
			}

            byte idx = (byte)(index + TickIdxDelta(timer.runTick));
			TimerList currList = array[idx];
			currList.AddTimer(timer);
			timerCount ++;
		}

        public Timer GetTimer(UInt64 runTick, uint timerSeqID)
		{
			if(smallerWheel != null && (runTick < baseTick))
				return smallerWheel.GetTimer(runTick,timerSeqID);

            byte idx = (byte)(index + TickIdxDelta(runTick));
			TimerList currList = array[idx];
			return currList.GetTimer(timerSeqID);
		}

        public bool DeleteTimer(UInt64 runTick, uint timerSeqID)
		{
			if(smallerWheel != null && (runTick < baseTick))
                return smallerWheel.DeleteTimer(runTick, timerSeqID);

            byte idx = (byte)(index + TickIdxDelta(runTick));
            TimerList currList = array[idx];
            if (!currList.DeleteTimer(timerSeqID))
			{
                LogManager.Instance.LogError((new System.Diagnostics.StackTrace()).ToString());
                LogManager.Instance.LogError("DeleteTimer Error : runTick:" + runTick + ",seqID:" + timerSeqID + ",index:" + index + ",idx:" + idx);
				return false;
			}
			timerCount --;
			return true;
		}

        public void Clear()
		{
			for(int i = 0; i < 256; ++i)
			{
				array[i].Clear();
			}
		}
	}

    public class TimerManager
    {
        private static TimerManager instance;
        public static TimerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TimerManager();
                return instance;
            }
        }

        uint seqIDBase = 0;// SEQID 取值范围(1,0xffffff),让AddTimer生成的TimerID为不可能0
        TimerListWheel[] timerWheels = new TimerListWheel[5];
        public delegate UInt64 TickProvider();
        public event TickProvider Tick;
        uint GetSeqID()
        {
            seqIDBase++;
            if(seqIDBase == 0 || seqIDBase >= 0xffffff)
                seqIDBase = 1;
            return seqIDBase;
        }

        public UInt64 GetTick()
        {
            if (Tick != null)
                return Tick();
            return TickerPolicy.Ticker.GetTick();
        }

		public TimerManager()
		{
            for (int i = 0; i < 5; i++)
                timerWheels[i] = new TimerListWheel();

			UInt64 now = GetTick();
            timerWheels[0].Init(1, 0, null, timerWheels[1], now);
            timerWheels[1].Init(0xff + 1, 8, timerWheels[0], timerWheels[2], now);
            timerWheels[2].Init(0xffff + 1, 16, timerWheels[1], timerWheels[3], now);
            timerWheels[3].Init(0xffffff + 1, 24, timerWheels[2], timerWheels[4], now);
            timerWheels[4].Init((UInt64)0xffffffff + 1, 32, timerWheels[3], null, now);

            Timer.ReAddHandler = ReAddTimer;
        }

        public Timer CreateTimer(double delay, Timer.TimerHandler op, object[] parameters)
        {
            return CreateTimer((int)delay, op, false, parameters);
        }
        public Timer CreateLoopTimer(double delay, Timer.TimerHandler op, object[] parameters)
        {
            return CreateTimer((int)delay, op, true, parameters);
        }

        public UInt64 AddTimer(double delay, Timer.TimerHandler op, object[] parameters)
        {
            Timer newTimer = CreateTimer(delay, op, parameters);
            return AddTimer(newTimer);
        }

        public bool DeleteTimer(UInt64 id)
        {
            // AddTimer生成的TimerID为不可能0
            if (id == 0)
                return true;
            return timerWheels[4].DeleteTimer(id >> 24, (uint)(id & 0xffffff));
        }

        public UInt64 AddTimer(Timer new_timer)
        {
            timerWheels[4].AddTimer(new_timer);
            return new_timer.timerID;
        }

        public IntPtr AddLoopTimer(double delay, Timer.TimerHandler op, object[] parameters)
        {
            Timer newTimer = CreateLoopTimer(delay, op, parameters);
            AddTimer(newTimer);
            return newTimer.GetIDPtr();
        }

        public bool DeleteTimer(IntPtr idPtr)
        {
            return DeleteTimer(Timer.IDFromPtr(idPtr));
        }

        public void SetTimerOp(UInt64 id, Timer.TimerHandler op, object[] parameters)
		{
            Timer timer = timerWheels[4].GetTimer(id >> 24, (uint)(id & 0xffffff));
			if(timer == null)
				return;
			timer.handler = op;
            timer.parameters = parameters;
		}

		// 返回值:
		// true  还有需执行的TIMER
		// false 没有需执行的TIMER

        public bool Execute()
        {
            return Execute(1000);
        }
        public bool Execute(int maxCount)
		{
			UInt64 now = GetTick();
            return timerWheels[0].Execute(now, ref maxCount);
		}

        public void Clear()
		{
			for(int i = 0;i < 4;++i)
			{
				timerWheels[i].Clear();
			}
		}

        #region Protected

        protected Timer CreateTimer(int delay, Timer.TimerHandler op, bool loop, object[] parameters)
        {
            uint seqID = GetSeqID();
            delay = delay < 0 ? 0 : delay;
            UInt64 run_tick = GetTick() + (UInt64)delay;
            if (run_tick > 0x000000ffffffffff)
            {
                run_tick = run_tick & 0x000000ffffffffff;
            }

            Timer newTimer = TimerPool.Instance.Get();
            newTimer.seqID = seqID;
            newTimer.runTick = run_tick;
            newTimer.delay = delay;
            newTimer.loop = loop;
            newTimer.handler = op;
            newTimer.parameters = parameters;
            newTimer.timerID = (run_tick << 24) | (seqID & 0xffffff);
            return newTimer;
        }

        protected void ReAddTimer(Timer timer)
        {
            UInt64 run_tick = GetTick() + (UInt64)timer.delay;
            if (run_tick > 0x000000ffffffffff)
            {
                run_tick = run_tick & 0x000000ffffffffff;
            }
            timer.runTick = run_tick;
            timer.timerID = (run_tick << 24) | (timer.seqID & 0xffffff);
            timerWheels[4].AddTimer(timer);
        }
        #endregion
    }
}
