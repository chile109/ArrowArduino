using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class AnimalController2 : MonoBehaviour, ObDataserver
{
    public Animal _ani = new Animal();
    public StateMachine _FSM;
    public GameObject Bubble;
    public bool InBubble = false;
    public Vector3 SpawnPos;

    public int H_pos;
    public int V_pos;

    void Start()
    {
        ObserverSystem.share.Attach(this);
        _FSM = new StateMachine();
        SpawnPos = this.transform.transform.position;
        if (SceneManager.GetActiveScene().name == "Success")
        {
            ObserverSystem.share.Notify("All", AnimalState.Success);
        }
        else
        {
            _FSM.NowState = _ani.idle;

        }
    }

    void OnMouseDown()
    {
        if (InBubble)
        {
            Debug.Log(this.gameObject.name);
            UImanager.point += 5;
            ObserverSystem.share.Notify(this.name, AnimalState.Saved);
            InBubble = false;
            BubbleOff();
        }
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
                   //Debug.Log(this.name + ": " + state);
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

    public void BeHit(int Horizental, int Vertical)
    {
        try
        {
            MainTask.Singleton.AddTask(delegate
            {
                //Debug.Log(_FSM.NowState);
                if (Horizental == H_pos && Vertical == V_pos && _FSM.NowState == _ani.Traped)
                {
                    //Debug.Log("Hit " + this.name);
						UImanager.point += 5;
                    ObserverSystem.share.Notify(this.name, AnimalState.Saved);
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log("Error convert, message" + e.Message + ", reason: " + e.StackTrace + ", Text: ");
        }
    }
}


