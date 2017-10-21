using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Timer
{
    public delegate uint TickFunc();
	public class TickerBase
	{
        UInt64 preHiTick = 0;
        uint preTick = 0;
        uint baseTick = 0;
        TickFunc tickFunc;
        public TickerBase()
        {
            this.tickFunc = GetTick32;
            this.baseTick = tickFunc();
        }

        public TickerBase(TickFunc tickFunc)
        {
            this.tickFunc = tickFunc;
            this.baseTick = tickFunc();
        }

        public uint GetTick32()
        {
            return (uint)System.Environment.TickCount;
        }

	    public UInt64 GetTick()
		{
            lock(tickFunc)
            {
                UInt64 ret;
                uint now = tickFunc();

                if (now < preTick)
                    ret = ((++preHiTick) << 32) + now;
                else
                    ret = (preHiTick << 32) + now;

                preTick = now;
                return ret - baseTick;
            }
		}
    };

    public interface ITicker
    {
        uint GetTick32();
        UInt64 GetTick();
    }

    public class SysTicker : ITicker
    {
        static TickerBase ticker = new TickerBase();
        public uint GetTick32()
        {
            return ticker.GetTick32();
        }
        public UInt64 GetTick()
        {
            return ticker.GetTick();
        }
    };

    public class Ticker : ITicker
    {
        static TickerBase ticker = new TickerBase(GetTick32Impl);
        static uint GetTick32Impl()
        {
            return (uint)(UnityEngine.Time.time * 1000);
        }
        public uint GetTick32()
        {
            return ticker.GetTick32();
        }

        public UInt64 GetTick()
        {
            return ticker.GetTick();
        }
    };

    public class RealTicker : ITicker
    {
        static TickerBase ticker = new TickerBase(GetTick32Impl);
        static uint GetTick32Impl()
        {
            return (uint)(UnityEngine.Time.realtimeSinceStartup * 1000);
        }
        public uint GetTick32()
        {
            return ticker.GetTick32();
        }
        public UInt64 GetTick()
        {
            return ticker.GetTick();
        }
    };

    public static class TickerPolicy
    {
        static ITicker defaultTicker = null;
        static ITicker sysTicker = null;
        static ITicker ticker = null;
        static ITicker realTicker = null;

        public static ITicker Default
        {
            get
            {
                if (defaultTicker == null)
                    defaultTicker = SysTicker;
                return defaultTicker;
            }
            set { defaultTicker = value; }
        }

        public static ITicker SysTicker
        {
            get
            {
                if (sysTicker == null)
                    sysTicker = new SysTicker();
                return sysTicker;
            }
            set { sysTicker = value; }
        }
        public static ITicker Ticker
        {
            get
            {
                if (ticker == null)
                    ticker = new Ticker();
                return ticker;
            }
            set { ticker = value; }
        }
        public static ITicker RealTicker
        {
            get
            {
                if (realTicker == null)
                    realTicker = new RealTicker();
                return realTicker;
            }
            set { realTicker = value; }
        }
    }
}
