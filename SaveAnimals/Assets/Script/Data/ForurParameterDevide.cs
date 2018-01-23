using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Xml;
using System.IO;
using System;
using System.Threading;

public class ForurParameterDevide : MonoBehaviour
{
    private SerialPort sp;
    private Thread receiveThread;  //用于接收消息的线程
    public static Dictionary<string, int> data = new Dictionary<string, int>();    //訊號資訊
    string Signal = "";
    bool bkd = true;
    // Use this for initialization
    //void Start()
    //{
    //    startThread();
    //}

    //private void Update()
    //{
    //    Signal = "Tonometer:30+Compass:30+OffsetX:10+OffsetZ:10";
    //}

    /// <summary>
    /// 開啟ＵＳＢ port
    /// </summary>
    public void OpenPort()
    {
        sp = new SerialPort(Port.portname, Port.baudrate);

        if (Port.portname.Length > 1)
        {
            sp.ReadTimeout = 1;
            sp.Open();
            startThread();
        }
    }

    void startThread()
    {
        receiveThread = new Thread(ReceiveThread);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveThread()
    {
        while (bkd)
        {

            try
            {
                String strRec = Signal;

                DistinguishSignal(strRec);

                foreach (var d in data)
                {
                    Debug.Log(d.Key + "/" + d.Value);
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

        }
    }

    /// <summary>
    /// 訊號斷行處理
    /// </summary>
    /// <returns>The signal.</returns>
    /// <param name="msg">Message.</param>
    void DistinguishSignal(string msg)
    {
        data.Clear();

        Char delimiter = '+';
        String[] substrings = msg.Split(delimiter);

        for (int i = 0; i < substrings.Length; i++)
        {
            Char subdeli = ':';
            String[] KandV = substrings[i].Split(subdeli);

            string _key = KandV[0];
            int _val = int.Parse(KandV[1]);

            addOrUpdate(data, _key, _val);

        }
    }

    void addOrUpdate(Dictionary<string, int> dic, string key, int newValue)
    {
        int val;
        if (dic.TryGetValue(key, out val))
        {
            dic[key] = newValue;
        }
        else
        {
            dic.Add(key, newValue);
        }
    }

    private void OnApplicationQuit()
    {
        bkd = false;//thread沒結束abort不會砍掉thread
        if (receiveThread != null)
            receiveThread.Abort();
    }


}
