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
        if (_val > Tonometer.InitPow && _val <= Tonometer.Pow2)
            Debug.Log("Power1");
        else if (_val > Tonometer.Pow2 && _val <= Tonometer.Pow3)
            Debug.Log("power2");
        else if (_val > Tonometer.Pow3)
            Debug.Log("MAxPower");
        else
            Debug.Log("NoPower");
            
    }

    void ArrowHorizental(int _val)
    {
        if (_val > Compass.LeftMin && _val <= Compass.Left2)
            Debug.Log("left1");
        else if (_val > Compass.Left2 && _val <= Compass.Left3)
            Debug.Log("left2");
        else if (_val > Compass.Left3 && _val <= Compass.Right3)
            Debug.Log("Middle");
        else if (_val > Compass.Right3 && _val <= Compass.Right2)
            Debug.Log("right2");
        else if (_val > Compass.Right2 && _val <= Compass.RightMax)
            Debug.Log("right1");
        else
            Debug.Log("reset");
    }
}
