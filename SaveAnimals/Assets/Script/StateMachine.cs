using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachine {

    public BaseState NowState;
    public List<BaseState> states;

    public StateMachine()
    {
        states = new List<BaseState>();
    }

    public void AddState(BaseState _State)
    {
        states.Add(_State);
    }
	
}
