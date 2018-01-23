using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSystem : MonoBehaviour {
   
    public static Vector3[,] ShootPoint = new Vector3[6, 3]; //15宮格矩陣中心點
    public static bool showLog = false;

    public static int VerticalLog = 0;
    public static int HorizentalLog = 0;

    bool initPos = false;
    Vector2 tempTar = new Vector2(3, 1);  //目標反灰區域

    void OnGUI()
    {
        if (!initPos)
        {
            SetTarget();
        }

		if (showLog) 
		{
			//畫出15宮格
			for (int i = 0; i < 6; i++) { //直線
				GUI.Box (new Rect (120 + 1040 / 5 * i, 0, 10, Screen.height), "");
			}

			for (int i = 0; i < 4; i++) { //橫線
				GUI.Box (new Rect (0, 50 + 150 * i, Screen.width, 10), "");
			}

			if (BowController.IsReloading && BowController.VerticID >= 0) {
				tempTar = new Vector2 (BowController.HorizID, BowController.VerticID);
				BowController.IsReloading = false;
			}

        
			var sk = GUI.skin.textArea.fontSize = 40;
			GUI.TextArea (new Rect (100, 100, 400, 50), "VerticalLog:" + VerticalLog, sk);
			GUI.TextArea (new Rect (100, 200, 400, 50), "HorizentalLog:" + HorizentalLog, sk);
        
			//畫出瞄準區域
			GUI.Box (new Rect (16 + 1040 / 5 * tempTar.x, 350 - 150 * tempTar.y, 208, 150), "");
		}
    }



    void SetTarget()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 gPos = new Vector3(120 + 1040 / 5 * i, 275 + 150 * j, 0);
                ShootPoint[i, j] = new Vector3(GUIUtility.GUIToScreenPoint(gPos).x, GUIUtility.GUIToScreenPoint(gPos).y, 10);
                //Debug.Log(Camera.main.ScreenToWorldPoint(ShootPoint[i, j]));
            }
        }

        initPos = true;
    }

}
