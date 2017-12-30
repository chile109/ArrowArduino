using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class Arduino_Loader : MonoBehaviour
{
    private string filepath;
    public static Arduino_Loader inst;

    private void Awake()
    {
        inst = this;
        filepath = Application.streamingAssetsPath + @"/ArduinoSettings.xml";
        SearchXml();
    }

    public void SearchXml()
    {

        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
            Debug.Log("search");

            foreach (XmlElement X in nodeList)
            {
                if (X.Name == "Port")
                {
                    foreach (XmlElement Set in X.ChildNodes)
                    {

                        if (Set.Name == "Baudrate")
                        {
                            Port.baudrate = int.Parse(Set.InnerText);
                        }
                        if (Set.Name == "Portname")
                        {
                            Port.portname = Set.InnerText;
                            Debug.Log("port:" + Port.portname);
                        }
                    }

                }
                if (X.Name == "Tonometer")
                {
                    foreach (XmlElement Pow in X.ChildNodes)
                    {

                        if (Pow.Name == "InitPow")
                        {
                            Tonometer.InitPow = int.Parse(Pow.InnerText);
                            Debug.Log("initPow:" + Tonometer.InitPow);
                        }
                        if (Pow.Name == "MaxPow")
                        {
                            Tonometer.MaxPow = int.Parse(Pow.InnerText);
                        }
                        if (Pow.Name == "Threshold")
                        {
                            Tonometer.Threshold = int.Parse(Pow.InnerText);
                        }
                    }

                }
                if (X.Name == "Compass")
                {
                    foreach (XmlElement Direc in X.ChildNodes)
                    {

                        if (Direc.Name == "LeftMin")
                        {
                            Compass.LeftMin = int.Parse(Direc.InnerText);
                        }
                        if (Direc.Name == "RightMax")
                        {
                            Compass.RightMax = int.Parse(Direc.InnerText);
                        }
                        if (Direc.Name == "Threshold")
                        {
                            Compass.Threshold = int.Parse(Direc.InnerText);
                        }


                    }
                }

            }
        }
        else
            return;

        DevideTonometerParts(Tonometer.InitPow, Tonometer.MaxPow, 3);
        DevideCompassParts(Compass.LeftMin, Compass.RightMax, 5);

        BowController.OpenPort();
    }

    void DevideTonometerParts(int Min, int Max, int times)
    {
        int[] points = new int[times];

        float gap;
        gap = (Max - Min) / times;

        for (int i = 1; i < times; i++)
        {
            points[i] = Min + (int)Math.Round(gap * i);
        }

        Tonometer.Pow2 = points[1];
        Tonometer.Pow3 = points[2];

        Debug.Log("pow2:" + Tonometer.Pow2 + "  pow3:" + Tonometer.Pow3);
    }

    void DevideCompassParts(int Min, int Max, int times)
    {
        int[] points = new int[times];

        float gap;
        gap = (Max - Min) / times;

        for (int i = 1; i < times; i++)
        {
            points[i] = Min + (int)Math.Round(gap * i);
        }

        Compass.Left2  = points[1];
        Compass.Left3  = points[2];
        Compass.Right3 = points[3];
        Compass.Right2 = points[4];

        Debug.Log("left2:" + Compass.Left2 + "  left3:" + Compass.Left3 + "  right3:" + Compass.Right3 + "  right2:" + Compass.Right2);
    }
}

public static class Port
{
    public static int baudrate;
    public static string portname;
}

public static class Tonometer
{
    public static int InitPow = 0;
    public static int Pow2 = 0;
    public static int Pow3 = 0;
    public static int MaxPow = 0;
    public static int Threshold = 0;
}

public static class Compass
{
    public static int LeftMin = 0;
    public static int Left2 = 0;
    public static int Left3 = 0;
    public static int Right2 = 0;
    public static int Right3 = 0;
    public static int RightMax = 0;
    public static int Threshold = 0;
}