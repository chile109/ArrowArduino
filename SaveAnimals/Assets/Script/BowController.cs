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
    Vector3 InitPosition;
    public int pre_val;
    public int test_val;
    public Transform Arrow;
    public bool IsReloading = false;

    public bool showLog = false;

    public void Start()
    {
        pre_val = Tonometer.InitPow;
        InitPosition = Arrow.position;
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

    private int VerticalLog = 0;
    void ArrowVertivcal(int _val)
    {
        VerticalLog = _val;
        if (_val - pre_val > Tonometer.Threshold && VerticID >= 0)
        {
            Debug.Log("_val:" + _val + "pre:" + pre_val + " Shoot!!");
            Debug.Log("HorizID:" + HorizID + "VerticID:" + VerticID);
            pre_val = _val;
            IsReloading = true;

            Vector3 _target = Camera.main.ScreenToWorldPoint(ShootPoint[HorizID, VerticID]);
            Debug.Log(_target);
            shootArrow(_target);
            return;
        }
        else
            ForceView(_val);

    }
    void shootArrow(Vector3 goal)
    {
        LeanTween.move(Arrow.gameObject, goal, 2f).setEase(LeanTweenType.easeInQuad)
                 .setOnComplete(_ =>
        {
            Debug.Log("goal");
            Arrow.position = InitPosition;
            IsReloading = false;
        });
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

    private int HorizentalLog = 0;
    private int HorizID;
    void ArrowHorizental(int _val)
    {
        HorizentalLog = _val;
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
            HorizID = 2;
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
        }
    }
    bool initPos = false;
    Vector2 tempTar = new Vector2(2, 1);
    void OnGUI()
    {
        if (!initPos)
        {
            SetTarget();
        }

        for (int i = 0; i < 6; i++)
        {
            GUI.Box(new Rect(160 + 1600 / 5 * i, 0, 10, Screen.height), "");
        }

        for (int i = 0; i < 4; i++)
        {
            GUI.Box(new Rect(0, 100 + 250 * i, Screen.width, 10), "");
        }

        if (IsReloading && VerticID >= 0)
        {
            tempTar = new Vector2(HorizID, VerticID);
            IsReloading = false;
        }

        if(showLog)
        {
            var sk = GUI.skin.textArea.fontSize = 40;
            GUI.TextArea(new Rect(100 , 100, 400, 50), "VerticalLog:" + VerticalLog, sk);
            GUI.TextArea(new Rect(100, 200, 400, 50), "HorizentalLog:" + HorizentalLog, sk);
        }
        GUI.Box(new Rect(160 + 1600 / 5 * tempTar.x, 600 - 250 * tempTar.y, 320, 250), "");
    }

    Vector3[,] ShootPoint = new Vector3[5, 3];

    void SetTarget()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 gPos = new Vector3(320 + 1600 / 5 * i, 225 + 250 * j, 0);
                ShootPoint[i, j] = new Vector3(GUIUtility.GUIToScreenPoint(gPos).x, GUIUtility.GUIToScreenPoint(gPos).y, 10);
                //Debug.Log(Camera.main.ScreenToWorldPoint(ShootPoint[i, j]));
            }
        }

        initPos = true;
    }
}
