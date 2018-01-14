using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Xml;
using System.IO;
using System;

public class BowController : MonoBehaviour
{
    public TargetSystem TarSystem;

    private static SerialPort sp;
    static Dictionary<string, int> data = new Dictionary<string, int>();    //訊號資訊
    Vector3 InitPosition;

    public int pre_val; //前訊號值

    public int test_val;
    public Transform Arrow; //  弓箭

    public static bool IsReloading = false;

    public static int  VerticID;   //Ｙ軸編號
    public static int HorizID;    //Ｘ軸編號

    public void Start()
    {
        pre_val = Tonometer.InitPow;
        InitPosition = Arrow.position;

    }

    /// <summary>
    /// 開啟ＵＳＢ port
    /// </summary>
    public static void OpenPort()
    {

        sp = new SerialPort(Port.portname, Port.baudrate);

        if (Port.portname.Length > 1)
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }
    }

    void Update()
    {
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

    /// <summary>
    /// 訊號斷行處理
    /// </summary>
    /// <returns>The signal.</returns>
    /// <param name="msg">Message.</param>
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


    public void ArrowVertivcal(int _val)
    {
        TargetSystem.VerticalLog = _val;
        if (_val - pre_val > Tonometer.Threshold && VerticID >= 0)
        {
            //Debug.Log("_val:" + _val + "pre:" + pre_val + " Shoot!!");
            //Debug.Log("HorizID:" + HorizID + "VerticID:" + VerticID);
            pre_val = _val;
            IsReloading = true;

            Vector3 _target = Camera.main.ScreenToWorldPoint(TargetSystem.ShootPoint[HorizID, VerticID]);

            shootArrow(_target);
            return;
        }
        else
            ForceView(_val);

    }
    void shootArrow(Vector3 goal)
    {
        Vector2Int m_target = new Vector2Int(HorizID, VerticID);
        LeanTween.move(Arrow.gameObject, goal, 0.3f).setEase(LeanTweenType.easeInQuad)
                 .setOnComplete(_ =>
        {
            //Debug.Log(m_target.x + "\\" + m_target.y);
            ObserverSystem.share.HitNotify(m_target.x, m_target.y); 
            Arrow.position = InitPosition;
            IsReloading = false;
        });
    }

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



    public void ArrowHorizental(int _val)
    {
        TargetSystem.HorizentalLog = _val;
        Vector3 m_rot = Arrow.rotation.eulerAngles;
        if (_val >= Compass.LeftMin && _val <= Compass.Left2)
        {
            HorizID = 0;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 60);
        }
        else if (_val > Compass.Left2 && _val <= Compass.Left3)
        {
            HorizID = 1;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 40);
        }

        else if (_val > Compass.Left3 && _val <= Compass.Middle)
        {
            HorizID = 2;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 20);
        }
        else if (_val > Compass.Middle && _val <= Compass.Right3)
        {
            HorizID = 3;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -20);
        }
        else if (_val > Compass.Right3 && _val <= Compass.Right2)
        {
            HorizID = 4;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -40);
        }
        else if (_val > Compass.Right2 && _val <= Compass.RightMax)
        {
            HorizID = 5;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -60);
        }
        else
        {
            HorizID = 2;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
        }
    }

}
