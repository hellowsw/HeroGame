using UnityEngine;
using System;

public class YouCantSpeedUp : MonoBehaviour
{
    public event Action OnGuilty;
    public const double MAX_OFFSET = 2;
    public const int FRAME_COUNT = 180;

    private double ticks;
    private double startOffset;
    private bool guilty = false;
    private bool print = false;
    private int count = 0;

    bool[] status = new bool[3];

    void Start()
    {
        ticks = (double)DateTime.Now.Ticks * 0.0000001f;
        startOffset = ticks - Time.realtimeSinceStartup;
    }

    double Abs(double a)
    {
        return a >= 0 ? a : -a;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                status[0] = true;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                status[1] = true;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                status[2] = true;
            }
            print = Check();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < status.Length; i++)
            {
                status[i] = false;
            }
            print = false;
        }

        if (guilty)
            return;
        ticks = (double)DateTime.Now.Ticks * 0.0000001f;

        if (Abs((ticks - Time.realtimeSinceStartup) - startOffset) > MAX_OFFSET)
        {
            count++;
            if (count >= FRAME_COUNT)
            {
                guilty = true;
                if (OnGuilty != null)
                    OnGuilty();
            }
        }
    }

    void OnGUI()
    {
        if (!print) return;

        GUILayout.Box("时间:" + Time.realtimeSinceStartup + "\nTicks:" + ticks + "\n偏移:" + (ticks - Time.realtimeSinceStartup).ToString("0.000000")
            + "\n程序运行时偏移:" + startOffset.ToString("0.000000") + "\n误差:" + (Abs((ticks - Time.realtimeSinceStartup) - startOffset)).ToString("0.000000") + "\n出现误差的累计数:" + count);
    }

    bool Check()
    {
        for (int i = 0; i < status.Length; i++)
        {
            if (status[i] == false)
                return false;
        }
        return true;
    }
}
