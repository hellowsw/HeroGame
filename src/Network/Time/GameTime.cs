using System;

namespace Network.Time
{
    public class GameTime 
    {
        public DateTime baseDate;

        public GameTime()
        {
            baseDate = new DateTime();
        }

        public GameTime(Int64 utc)
        {

            baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            baseDate = baseDate.AddSeconds(utc);
            baseDate = baseDate.ToLocalTime();
            //LogManager.Instance.LogDebug(curDate.ToString());
        }
    }
}
