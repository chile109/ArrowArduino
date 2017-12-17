using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Xml;
using System.IO;
using System;

public class BowController : MonoBehaviour
{

    private static SerialPort sp;
    static Dictionary<string, int> data = new Dictionary<string, int>();
    public int pre_val;
    public int test_val;
    public Transform Arrow;
    public bool IsReloading = false;

    public void Start()
    {
        pre_val = Tonometer.InitPow;
    }
    // Use this for initialization
    public static void OpenPort()
    {

        sp = new SerialPort(Port.portname, Port.baudrate);

        if (Port.portname.Length > 1)
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        ArrowVertivcal(test_val);
        if (sp.IsOpen)
        {
            try
            {
                DistinguishSignal(sp.ReadLine());

                foreach (var OneItem in data)
                {
                    Debug.Log("Key = " + OneItem.Key + ", Value = " + OneItem.Value);

                    switch (OneItem.Key)
                    {
                        case "Compass":
                            ArrowHorizental(OneItem.Value);
                            break;
                        case "Tonometer":
                            ArrowVertivcal(OneItem.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }

    static Dictionary<string, int> DistinguishSignal(string msg)
    {
        data.Clear();

        Char delimiter = ':';
        String[] substrings = msg.Split(delimiter);

        string _device = substrings[0];
        int _val = int.Parse(substrings[1]);

        data.Add(_device, _val);

        return data;
    }

    void ArrowVertivcal(int _val)
    {
        if (Math.Abs(_val - pre_val) > Tonometer.Threshold)
        {
            Debug.Log("_val:" + _val + "pre:" + pre_val + " Shoot!!");
            pre_val = _val;
            IsReloading = true;
            return;
        }
        else
            ForceView(_val);

    }

    private int VerticID;
    private void ForceView(int _val)
    {
        //Debug.Log("force");
        pre_val = _val;

        if (_val < Tonometer.InitPow && _val >= Tonometer.Pow2)
        {
            VerticID = 0;
            Arrow.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        else if (_val < Tonometer.Pow2 && _val >= Tonometer.Pow3)
        {
            VerticID = 1;
            Arrow.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }

        else if (_val < Tonometer.Pow3)
        {
            VerticID = 2;
            Arrow.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        else
        {
            VerticID = -1;
            Arrow.localScale = Vector3.one;
        }
    }

    private int HorizID;
    void ArrowHorizental(int _val)
    {
        Vector3 m_rot = Arrow.rotation.eulerAngles;
        if (_val > Compass.LeftMin && _val <= Compass.Left2)
        {
            HorizID = 0;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 60);
        }
        else if (_val > Compass.Left2 && _val <= Compass.Left3)
        {
            HorizID = 1;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 30);
        }

        else if (_val > Compass.Left3 && _val <= Compass.Right3)
        {
            HorizID = 2;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
        }
        else if (_val > Compass.Right3 && _val <= Compass.Right2)
        {
            HorizID = 3;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -30);
        }
        else if (_val > Compass.Right2 && _val <= Compass.RightMax)
        {
            HorizID = 4;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -60);
        }
        else
        {
            HorizID = -1;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
        }
    }

    void OnGUI()
    {
        //InitPos = Camera.main.WorldToViewportPoint(this.transform.position);

        for (int i = 0; i < 6; i++)
        {
            GUI.Box(new Rect( 160 + 1600/5 * i, 0, 10, Screen.height), "");
        }

        for (int i = 0; i < 4; i++)
        {
            GUI.Box(new Rect(0, 100 + 250 * i, Screen.width, 10), "");
        }

        //for (int k = 0; k < 5; k++)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        GUI.Box(new Rect(160 + 1600 / 5 * k, 100 + 250 * i, 320, 250), "");
        //    }
        //}

        if(IsReloading && HorizID >= 0 && VerticID >= 0)
        {
            GUI.Box(new Rect(160 + 1600 / 5 * HorizID, 100 + 250 * VerticID, 320, 250), "");
        }
    }
}
