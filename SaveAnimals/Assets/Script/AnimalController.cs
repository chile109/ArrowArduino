﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class AnimalController : MonoBehaviour {

    public Animal _ani = new Animal();
    public StateMachine _FSM;
    public bool isCatched = false;

    public float MoveSpeed = 2.0f;
    public float FleeSpeed = 5.0f;

    public Target m_target = Target.Left;
    public Vector3 m_targetPos;
    Vector3 InitPos;

	void Start () {
        _FSM = new StateMachine();
        _FSM.NowState = _ani.idle;
        InitPos = Camera.main.WorldToViewportPoint(this.transform.position);

        BossCome(5);
	}

	void Update () {
        _FSM.NowState.StateDoing(this.gameObject);
	}

    public void SetTarget()
    {  
        
        Vector3 scale = this.transform.localScale;
        scale.x = Math.Abs(scale.x) * (m_target == Target.Right ? 1 : -1);
        this.transform.localScale = scale;

        float cameraZ = Camera.main.transform.position.z;
        m_targetPos = Camera.main.ViewportToWorldPoint(new Vector3((int)m_target, InitPos.y, -cameraZ));
        Debug.Log("targetPos:" + m_targetPos);
    }

    public async void BossCome(double _duration)
    {
        Debug.Log("Waiting " + _duration + " second...");
        await Task.Delay(TimeSpan.FromSeconds(_duration));
        _FSM.NowState = _ani.flee;
        Debug.Log("Done!");
    }
}

public class Animal
{
    public ANI_Idle idle = new ANI_Idle();
    public ANI_Fleeing flee = new ANI_Fleeing();
    public ANI_Traped Traped = new ANI_Traped();
    public ANI_Cheering Cheering = new ANI_Cheering();
}

