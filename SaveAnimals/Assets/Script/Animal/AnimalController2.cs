using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class AnimalController2 : MonoBehaviour, ObDataserver
{
    public Animal _ani = new Animal();
    public StateMachine _FSM;
    public Vector3 m_targetPos;
    public GameObject Bubble;
    public Vector3 InitPos;

    public int H_pos;
    public int V_pos;

    void Start()
    {
        ObserverSystem.share.Attach(this);
        _FSM = new StateMachine();
        _FSM.NowState = _ani.idle;
        InitPos = Camera.main.WorldToViewportPoint(this.transform.position);
    }

    public void BubbleOn()
    {
        Bubble.SetActive(true);
    }

    public void BubbleOff()
    {
        Bubble.SetActive(false);
    }

    public void BeNotified(string AniName, AnimalState state)
    {
        try
        {
            MainTask.Singleton.AddTask(delegate
           {
               if (AniName == this.name || AniName == "All")
               {
                   Debug.Log(this.name + ": " + state);
                   switch (state)
                   {
                       case AnimalState.Idle:
                           _FSM.NowState = _ani.idle;
                           _FSM.NowState.StateDoing(this.gameObject);
                           break;
                       case AnimalState.Help:
                           _FSM.NowState = _ani.Traped;
                           _FSM.NowState.StateDoing(this.gameObject);
                           break;
                       case AnimalState.Saved:
                           _FSM.NowState = _ani.Saved;
                           _FSM.NowState.StateDoing(this.gameObject);
                           break;
                       case AnimalState.Success:
                           _FSM.NowState = _ani.Cheering;
                           _FSM.NowState.StateDoing(this.gameObject);
                           break;
                       case AnimalState.Fail:
                           _FSM.NowState = _ani.Cry;
                           _FSM.NowState.StateDoing(this.gameObject);
                           break;
                   }
               }
           });
        }
        catch (Exception e)
        {
            Debug.Log("Error convert, message" + e.Message + ", reason: " + e.StackTrace + ", Text: ");
        }

    }

    private void Update()
    {
        var NowPos = Camera.main.WorldToScreenPoint(this.transform.position);
        //Debug.Log(NowPos + " / " + TargetSystem.ShootPoint[H_pos, 0]);
        if (NowPos.y > TargetSystem.ShootPoint[H_pos, 0].y)
            V_pos = 0;
        if (NowPos.y > TargetSystem.ShootPoint[H_pos, 1].y)
            V_pos = 1;
        if (NowPos.y > TargetSystem.ShootPoint[H_pos, 2].y)
            V_pos = 2;
    }
    public void BeNotified(int Horizental, int Vertical)
    {
        if(Horizental == H_pos && Vertical == V_pos)
        {
            ObserverSystem.share.Notify(this.name, AnimalState.Saved);
        }

    }
}


