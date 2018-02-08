using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour {

    public GameObject InGame;
    public GameObject ScoreBoard;

    public Image _cursor;
	public Text GameTime;
	public Text Score;

    public Text NowScore;
    public Text TodayBest;
    public Text HistoryBest;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorTexture;
	public static int sec;
	public static int point;
    public static int BestScore;

	// Use this for initialization
	void Start () {
		sec = 90;
		point = 0;
		InvokeRepeating ("CountDown", 0, 1);
        ObserverSystem.share.GameOver = false;
        Cursor.visible = false;

	}

	void Update()
	{
        _cursor.transform.position = Input.mousePosition;
        GameTime.text = sec.ToString() ;
        Score.text = point.ToString();

        if(Input.GetMouseButtonUp(0))
        {
            AudioManager.SFX_ES.Trigger("Shooting");
        }

	}

	void CountDown()
	{
		sec -= 1;

		if (sec < 0)
		{

            CancelInvoke("CountDown");

            ObserverSystem.share.GameOver = true;


            if (point > BestScore)
                BestScore = point;
            
            InGame.SetActive(false);
            ScoreBoard.SetActive(true);

            NowScore.text = point.ToString();
            TodayBest.text = BestScore.ToString();
            HistoryBest.text = "100";

            Invoke("GoResult", 10);
		}
	}

    void GoResult()
    {
        if(point > 40)
            SceneManager.LoadSceneAsync("Success");
        else
            SceneManager.LoadSceneAsync("Fail");
    }
}
