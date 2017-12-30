using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UFO_IdleState : BaseState
{
    UFOController uFOController;
    public override void StateDoing(GameObject Obj)
    {
        //Debug.Log("UFO_Idle");
        uFOController = Obj.GetComponent<UFOController>();
        uFOController.Beam.SetActive(false);
        Vector3 pos;

        if (uFOController.startHunt)
            pos = Vector3.MoveTowards(Obj.transform.position, uFOController.TargetPos, uFOController.MoveSpeed * Time.deltaTime);
        else
            pos = Vector3.MoveTowards(Obj.transform.position, uFOController.InitPos, uFOController.MoveSpeed * Time.deltaTime);
        
            Obj.transform.position = pos;

        if (Math.Abs(Obj.transform.position.x - uFOController.TargetPos.x) < 0.01f && !uFOController.HuntFinish)
        {
            uFOController._Ani.SetTrigger("IsHuntting");
            uFOController._FSM.NowState = uFOController._ufo.Hunt;

            ObserverSystem.share.Notify(uFOController.m_Target[0].name, AnimalState.Help);
        }
    }


}
