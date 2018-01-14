using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFinger : MonoBehaviour {

    public BowController _bow;
    // Use this for initialization
    private int H_Force = 30;
    private int V_Force = 30;

	// Update is called once per frame
	void Update () {
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
}
