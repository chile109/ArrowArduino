using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UFOController : MonoBehaviour
{

    public UFO _ufo;
    public StateMachine _FSM;
    public GameObject Beam;
    public bool startHunt = false;
    public static bool HuntFinish = false;

    public Transform[] m_Targets;
    public int TargetID;
    public float MoveSpeed = 2;

    public Animator _Ani;
    public Vector3 InitPos;
    public Vector3 TargetPos;

    void Start()
    {
        _FSM = new StateMachine();
        _Ani = GetComponent<Animator>();
        _ufo = new UFO();
        _FSM.NowState = _ufo.Idle;
        InitPos = new Vector3(0, this.transform.position.y, this.transform.position.z);
        HuntFinish = false;
        TargetID = UnityEngine.Random.Range(0, m_Targets.Length);
        TargetPos = new Vector3(m_Targets[TargetID].position.x, this.transform.position.y, this.transform.position.z);
        FoolAround(5);
    }

    void Update()
    {
        if (!ObserverSystem.share.GameOver)
            _FSM.NowState.StateDoing(this.gameObject);
    }

    /// <summary>
    /// 遊盪一段時間進入狩獵
    /// </summary>
    /// <param name="_duration">Duration.</param>
    public async void FoolAround(double _duration)
    {
        //Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        TargetID = UnityEngine.Random.Range(0, m_Targets.Length);
        TargetPos = new Vector3(m_Targets[TargetID].position.x, this.transform.position.y, this.transform.position.z);
        //Debug.Log(m_Targets[TargetID].position.x);

        startHunt = true;
    }

}
