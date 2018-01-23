using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Xml;
using System.IO;
using System;
using System.Threading;

public class BowController2 : MonoBehaviour
{
    bool bkd = true;
    private Thread resolveThread;  //用于接收消息的线程

    Vector3 InitPosition;
    public GameObject Warning;
    public int pre_val; //前訊號值

    public int test_val;
    public Transform Arrow; //  弓箭

    public static bool IsReloading = false;

    public static int VerticID;   //Ｙ軸編號
    public static int HorizID;    //Ｘ軸編號

    public void Start()
    {
        pre_val = Tonometer.InitPow;
        InitPosition = Arrow.position;

        startThread();
    }

    void startThread()
    {
        resolveThread = new Thread(ResolveThread);
        resolveThread.IsBackground = true;
        resolveThread.Start();
    }

    private void ResolveThread()
    {
        while (bkd)
        {
            try
            {
                //String strRec = sp.ReadLine();            //SerialPort读取数据有多种方法，我这里根据需要使用了ReadLine()
                //Debug.Log("Receive From Serial: " + strRec);

                MainTask.Singleton.AddTask(delegate
                {
                    foreach (var OneItem in ForurParameterDevide.data)
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
                            case "OffsetX":
                                Xoffset(OneItem.Value);
                                break;
                            case "OffsetZ":
                                Zoffset(OneItem.Value);
                                break;
                            default:
                                break;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }

        }
    }

    private void OnApplicationQuit()
    {
        bkd = false;//thread沒結束abort不會砍掉thread
        resolveThread.Abort();
    }

    void Update()
    {
        if (Error1 || Error2 || Error3 || Error4)
            Warning.SetActive(true);
        else
            Warning.SetActive(false);
    }

    public void ArrowVertivcal(int _val)
    {
        TargetSystem.VerticalLog = _val;
        if (_val - pre_val > Tonometer.Threshold && VerticID >= 0 && !IsReloading)
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

    private bool Error1 = false;
    private void ForceView(int _val)
    {
        //Debug.Log("force");
        pre_val = _val;

        if (_val <= Tonometer.InitPow && _val >= Tonometer.Pow2)
        {
            VerticID = 0;
            Arrow.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Error1 = false;
        }

        else if (_val < Tonometer.Pow2 && _val >= Tonometer.Pow3)
        {
            VerticID = 1;
            Arrow.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            Error1 = false;
        }

        else if (_val < Tonometer.Pow3)
        {
            VerticID = 2;
            Arrow.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Error1 = false;
        }
        else
        {
            Error1 = true;
        }
    }

    private bool Error2 = false;
    public void ArrowHorizental(int _val)
    {
        TargetSystem.HorizentalLog = _val;
        Vector3 m_rot = Arrow.rotation.eulerAngles;
        if (_val >= Compass.LeftMin && _val <= Compass.Left2)
        {
            HorizID = 0;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 60);
            Error2 = false;
        }
        else if (_val > Compass.Left2 && _val <= Compass.Left3)
        {
            HorizID = 1;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 40);
            Error2 = false;
        }

        else if (_val > Compass.Left3 && _val <= Compass.Middle)
        {
            HorizID = 2;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 20);
            Error2 = false;
        }
        else if (_val > Compass.Middle && _val <= Compass.Right3)
        {
            HorizID = 3;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -20);
            Error2 = false;
        }
        else if (_val > Compass.Right3 && _val <= Compass.Right2)
        {
            HorizID = 4;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -40);
            Error2 = false;
        }
        else if (_val > Compass.Right2 && _val <= Compass.RightMax)
        {
            HorizID = 5;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -60);
            Error2 = false;
        }
        else
        {
            Error2 = true;
        }
    }
    private bool Error3 = false;
    public void Xoffset(int x)
    {
        if (x > OffsetX.Max || x < OffsetX.Min)
            Error3 = true;
        else
            Error3 = false;
    }

    private bool Error4 = false;
    public void Zoffset(int z)
    {
        if (z > OffsetZ.Max || z < OffsetZ.Min)
            Error4 = true;
        else
            Error4 = false;
    }

}
