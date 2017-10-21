using System;

namespace Network
{
    // 放置需要使用Unsafe来实现的有用方法
    public static class UnsafeHelper
    {
        // 动态替换字符串指定字符
        //---------------------------------------------------------------------
        public static unsafe bool SetChar(string str, int index, char c)
        {
            if (String.IsNullOrEmpty(str) || index >= str.Length)
            {
                return false;
            }

            fixed (char* p = str)
            {
                p[index] = c;
            }

            return true;
        }

        // 动态替换字符串指定区域的字符串
        //---------------------------------------------------------------------
        public static unsafe bool SetStr(string str, int start, string s)
        {
            if (String.IsNullOrEmpty(str) || start >= str.Length)
            {
                return false;
            }

            fixed (char* p = str)
            {
                var strLength = str.Length;
                for (int i = start; i < strLength; ++i)
                {
                    p[i] = s[i - start];
                }
            }

            return true;
        }

        // 转换时间为字符串，格式为：00:00:00
        //---------------------------------------------------------------------
        public static unsafe bool ToFullTime(string str, long time)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 8)
            {
                return false;
            }

            short hour = (short)(time / 3600);
            short minute = (short)(time / 60 % 60);
            short second = (short)(time % 60);

            fixed (char* p = str)
            {
                p[0] = (char)(48 + (hour / 10));
                p[1] = (char)(48 + (hour % 10));

                p[3] = (char)(48 + (minute / 10));
                p[4] = (char)(48 + (minute % 10));

                p[6] = (char)(48 + (second / 10));
                p[7] = (char)(48 + (second % 10));
            }

            return true;
        }


        // 转换时间为分秒字符串，格式为：00:00
        //---------------------------------------------------------------------
        public static unsafe bool ToMinuteSecond(string str, long time)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 5)
            {
                return false;
            }

            short minute = (short)(time / 60 % 60);
            short second = (short)(time % 60);

            fixed (char* p = str)
            {
                p[0] = (char)(48 + (minute / 10));
                p[1] = (char)(48 + (minute % 10));

                p[3] = (char)(48 + (second / 10));
                p[4] = (char)(48 + (second % 10));
            }

            return true;
        }

        // 转换时间为时分字符串，格式为：00:00
        //---------------------------------------------------------------------
        public static unsafe bool ToHourMinute(string str, long time)
        {
            if (String.IsNullOrEmpty(str) || str.Length < 5)
            {
                return false;
            }

            short hour = (short)(time / 3600);
            short minute = (short)(time / 60 % 60);

            fixed (char* p = str)
            {
                p[0] = (char)(48 + (hour / 10));
                p[1] = (char)(48 + (hour % 10));

                p[3] = (char)(48 + (minute / 10));
                p[4] = (char)(48 + (minute % 10));
            }

            return true;
        }
    }
}
