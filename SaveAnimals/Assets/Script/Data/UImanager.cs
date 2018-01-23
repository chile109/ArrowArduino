﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {

	public Text GameTime;
	public Text Score;

	public static int sec;
	public static int point;

	// Use this for initialization
	void Start () {
		sec = 90;
		point = 0;
		InvokeRepeating ("CountDown", 0, 1);
	}

	void Update()
	{
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
		}
	}
}