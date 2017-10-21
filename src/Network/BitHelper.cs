namespace Network
{
    public class BitHelper
    {
        public static int Get(int data, int index, int count)
        {
            int mask = 0x1;
            for (int i = 1; i < count; i++)
                mask = (mask << 1) | 0x1;

            return (data >> (8 - index - count)) & mask;
        }

        public static int Set(int data, int index, int count, int value)
        {
            value = value << (8 - index);

            int mask = 0x1;
            for (int i = 1; i < count; i++)
                mask = (mask << 1) | 0x1;
            mask = mask << (8 - index);
            int clearMask = ~mask;
            return (data & clearMask) | (value & mask);
        }
    }
}
