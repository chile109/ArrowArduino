using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UFOController : MonoBehaviour {

    public UFO _ufo;
    public StateMachine _FSM;
    public bool isHunting = false;

    void Start () {

        _FSM = new StateMachine();

        _ufo = new UFO();
        _FSM.NowState = _ufo.Idle;
        FoolAround(3);
	}
	
	void Update () {
        _FSM.NowState.StateDoing(this.gameObject);
	}

    public async void FoolAround(double _duration)
    {
        //Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        _FSM.NowState = _ufo.Hunt;
        TestTimer(5);
        Debug.Log("Done!");
    }

    public async void TestTimer(double _duration)
    {
        Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        isHunting = true;
        Debug.Log("Done!");
    }
}

public class UFO
{
    public UFO_IdleState Idle = new UFO_IdleState();
    public UFO_HuntState Hunt = new UFO_HuntState();

}
