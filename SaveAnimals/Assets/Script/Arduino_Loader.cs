using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;

public class Arduino_Loader : MonoBehaviour
{
    private string filepath;
    public static Arduino_Loader inst;

    public static int InitPow = 0;
    public static int MaxPow = 0;
    public static int T_Threshold = 0;
    public static int LeftMin = 0;
    public static int RightMax = 0;
    public static int C_Threshold = 0;


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
                if (X.Name == "Tonometer")
                {
                    foreach (XmlElement Pow in X.ChildNodes)
                    {

                        if (Pow.Name == "InitPow")
                        {
                            InitPow = int.Parse(Pow.InnerText);
                            Debug.Log("initPow:" + InitPow);
                        }
                        if (Pow.Name == "MaxPow")
                        {
                            MaxPow = int.Parse(Pow.InnerText);
                        }
                        if (Pow.Name == "Threshold")
                        {
                            T_Threshold = int.Parse(Pow.InnerText);
                        }
                    }

                }
                if (X.Name == "Compass")
                {
                    foreach (XmlElement Direc in X.ChildNodes)
                    {

                        if (Direc.Name == "LeftMin")
                        {
                            LeftMin = int.Parse(Direc.InnerText);
                        }
                        if (Direc.Name == "RightMax")
                        {
                            RightMax = int.Parse(Direc.InnerText);
                        }
                        if (Direc.Name == "Threshold")
                        {
                            C_Threshold = int.Parse(Direc.InnerText);
                        }
                    }
                }

            }
        }
        else
            return;
    }

}
