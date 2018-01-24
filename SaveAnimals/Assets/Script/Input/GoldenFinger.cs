using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFinger : MonoBehaviour {

    public BowController2 _bow;
    // Use this for initialization
    private int H_Force = 30;
    private int V_Force = 30;

	// Update is called once per frame
	void Update () {
		type2 ();
	}

	void type1()
	{
		if(Input.GetKey(KeyCode.LeftArrow) &&  H_Force > Compass.LeftMin)
		{
			H_Force -= 1;
		}
		if(Input.GetKey(KeyCode.RightArrow) && H_Force < Compass.RightMax)
		{
			H_Force += 1;
		}
		if (Input.GetKey(KeyCode.Space) && V_Force > Tonometer.MaxPow)
		{
			V_Force -= 1;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			V_Force = Tonometer.InitPow;
		}

		_bow.ArrowHorizental(H_Force);
		_bow.ArrowVertivcal(V_Force);
	}

	void type2()
	{
        if(Input.GetKey(KeyCode.Alpha1))
			_bow.ArrowHorizental((int)(Compass.LeftMin + Compass.Left2)/2);
        if(Input.GetKey(KeyCode.Alpha2))
			_bow.ArrowHorizental((int)(Compass.Left2 + Compass.Left3)/2);
        if(Input.GetKey(KeyCode.Alpha3))
			_bow.ArrowHorizental((int)(Compass.Left3 + Compass.Middle)/2);
        if(Input.GetKey(KeyCode.Alpha4))
			_bow.ArrowHorizental((int)(Compass.Middle + Compass.Right2)/2);
        if(Input.GetKey(KeyCode.Alpha5))
			_bow.ArrowHorizental((int)(Compass.Right2 + Compass.Right3)/2);
        if(Input.GetKey(KeyCode.Alpha6))
			_bow.ArrowHorizental((int)(Compass.Right3 + Compass.RightMax)/2);

		if (Input.GetKeyUp (KeyCode.A))
			_bow.shootArrow (2);
		if(Input.GetKeyUp(KeyCode.B))
			_bow.shootArrow (1);
		if(Input.GetKeyUp(KeyCode.C))
			_bow.shootArrow (0);

		if (Input.GetKeyUp (KeyCode.E)) 
		{
			CancelInvoke ("Timer");
			_bow.Error5 = true;
			Invoke("Timer", 2);
		}
	}

	void Timer()
	{
		_bow.Error5 = false;
	}
}
