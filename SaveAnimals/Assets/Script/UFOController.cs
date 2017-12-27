using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UFOController : MonoBehaviour {

    public UFO _ufo;
    public StateMachine _FSM;
    public GameObject Beam;
    public bool HuntFinish = false;

    public Transform m_Target;
    public float MoveSpeed = 2;

    Animator _Ani;

    public Vector3 TargetPos = Vector3.zero;

    void Start () {

        _FSM = new StateMachine();
        _Ani = GetComponent<Animator>();
        _ufo = new UFO();
        _FSM.NowState = _ufo.Idle;

        TargetPos = new Vector3(m_Target.position.x, this.transform.position.y, this.transform.position.z);
        //FoolAround(3);
	}
	
	void Update () {
        _FSM.NowState.StateDoing(this.gameObject);

        if(Math.Abs(this.transform.position.x - TargetPos.x) < 0.01f && !HuntFinish)
        {
            _Ani.SetTrigger("IsHuntting");
            _FSM.NowState = _ufo.Hunt;
        }
	}

    public async void FoolAround(double _duration)
    {
        //Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        _Ani.SetTrigger("IsHuntting");
        _FSM.NowState = _ufo.Hunt;
        TestTimer(5);
    }

    public async void TestTimer(double _duration)
    {
        Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        _Ani.SetTrigger("IsRest");
        HuntFinish = true;
    }
}

public class UFO
{
    public UFO_IdleState Idle = new UFO_IdleState();
    public UFO_HuntState Hunt = new UFO_HuntState();

}
