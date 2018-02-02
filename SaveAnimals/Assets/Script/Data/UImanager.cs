using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour {

    public Image _cursor;
	public Text GameTime;
	public Text Score;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorTexture;
	public static int sec;
	public static int point;

	// Use this for initialization
	void Start () {
		sec = 90;
		point = 0;
		InvokeRepeating ("CountDown", 0, 1);

        //Cursor.visible = true;
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode );


	}

	void Update()
	{
        _cursor.transform.position = Input.mousePosition;
		GameTime.text = "Time:" + sec +"s";
		Score.text = "Point:" + point;
	}

	void CountDown()
	{
		sec -= 1;

		if (sec < 0)
		{
			sec = 90;
			point = 0;
            SceneManager.LoadSceneAsync("Wellcome");
		}
	}
}
