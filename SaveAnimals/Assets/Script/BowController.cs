using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Xml;
using System.IO;
using System;

public class BowController : MonoBehaviour {

    private static SerialPort sp;
    static Dictionary<string, int> data = new Dictionary<string, int>();
    public int test_val;
    public Transform Arrow;

	// Use this for initialization
	public static void OpenPort() {
        
        sp = new SerialPort(Port.portname, Port.baudrate);

        if (Port.portname.Length > 1)
        {
            sp.Open();
            sp.ReadTimeout = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {

        ArrowVertivcal(test_val);
        if(sp.IsOpen)
        {
            try
            {
                DistinguishSignal(sp.ReadLine());

                foreach (var OneItem in data)
                {
                    Debug.Log("Key = " + OneItem.Key + ", Value = " + OneItem.Value);

                    switch(OneItem.Key)
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
            catch(Exception ex)
            {
                Debug.Log(ex);
            }
        }
	}

    static Dictionary<string,int> DistinguishSignal(string msg)
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

        if (_val < Tonometer.InitPow && _val >= Tonometer.Pow2)
            Arrow.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        else if (_val < Tonometer.Pow2 && _val >= Tonometer.Pow3)
            Arrow.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        
        else if (_val < Tonometer.Pow3)
            Arrow.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        else
            Arrow.localScale = Vector3.one;
            
    }

    void ArrowHorizental(int _val)
    {
        Vector3 m_rot = Arrow.rotation.eulerAngles;
        if (_val > Compass.LeftMin && _val <= Compass.Left2)
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 60);

        else if (_val > Compass.Left2 && _val <= Compass.Left3)
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 30);

        else if (_val > Compass.Left3 && _val <= Compass.Right3)
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
        
        else if (_val > Compass.Right3 && _val <= Compass.Right2)
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -30);
        
        else if (_val > Compass.Right2 && _val <= Compass.RightMax)
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, -60);
        
        else
            Arrow.eulerAngles = new Vector3(m_rot.x, m_rot.y, 0);
    }
}
