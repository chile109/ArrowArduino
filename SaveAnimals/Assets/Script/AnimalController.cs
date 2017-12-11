using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    public Animal _ani = new Animal();
    public StateMachine _FSM;
    public bool isCatched = false;

	// Use this for initialization
	void Start () {
        _FSM = new StateMachine();
        _FSM.NowState = _ani.idle;
	}
	
	// Update is called once per frame
	void Update () {
        _FSM.NowState.StateDoing(this.gameObject);
	}
}

public class Animal
{
    public ANI_Idle idle = new ANI_Idle();
    public ANI_Fleeing flee = new ANI_Fleeing();
    public ANI_Traped Traped = new ANI_Traped();
    public ANI_Cheering Cheering = new ANI_Cheering();
}
