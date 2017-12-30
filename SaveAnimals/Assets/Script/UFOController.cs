using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ObDataserver
{
    void BeNotified(string AniName, AnimalState state);
}

public class UFOController : MonoBehaviour {

    public UFO _ufo;
    public StateMachine _FSM;
    public GameObject Beam;
    public bool startHunt = false;
    public bool HuntFinish = false;

    public Transform[] m_Target;
    public float MoveSpeed = 2;

    public Animator _Ani;

    private static UFOController _instants;
    public static UFOController share { get { return _instants; } }

    private HashSet<ObDataserver> m_ObDateServers = new HashSet<ObDataserver>();

    public Vector3 InitPos;
    public Vector3 TargetPos = Vector3.zero;


    void Awake()
    {
        if (_instants == null)
        {
            _instants = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    void Start () {

        _FSM = new StateMachine();
        _Ani = GetComponent<Animator>();
        _ufo = new UFO();
        _FSM.NowState = _ufo.Idle;
        InitPos = new Vector3(0, this.transform.position.y, this.transform.position.z);
        TargetPos = new Vector3(m_Target[0].position.x, this.transform.position.y, this.transform.position.z);

        Notify("All", AnimalState.Idle);
        FoolAround(5);
	}
	
	void Update () {
        _FSM.NowState.StateDoing(this.gameObject);
	}

    public void Attach(ObDataserver theObserver)
    {
        if (theObserver != null)
            m_ObDateServers.Add(theObserver);

    }

    public void Detach(ObDataserver theObserver)
    {
        if (theObserver != null)
            m_ObDateServers.Remove(theObserver);
    }

    public void Notify(string AniName, AnimalState state)
    {
        Debug.Log(AniName + ": " + state);
        foreach (var theObserver in m_ObDateServers)
            Task.Run(() => theObserver.BeNotified(AniName, state));

    }


    /// <summary>
    /// 遊盪一段時間進入狩獵
    /// </summary>
    /// <param name="_duration">Duration.</param>
    public async void FoolAround(double _duration)
    {
        //Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        startHunt = true;
    }

    /// <summary>
    /// 進入狩獵直到時間結束
    /// </summary>
    /// <param name="_duration">Duration.</param>
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
