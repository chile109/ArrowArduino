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
        
        uFOController = Obj.GetComponent<UFOController>();
        uFOController.Beam.SetActive(false);
        Vector3 pos;

        if (uFOController.startHunt)
            pos = Vector3.MoveTowards(Obj.transform.position, uFOController.TargetPos, uFOController.MoveSpeed * Time.deltaTime);
        else
            pos = Vector3.MoveTowards(Obj.transform.position, uFOController.InitPos, uFOController.MoveSpeed * Time.deltaTime);
        
            Obj.transform.position = pos;

        if (Math.Abs(Obj.transform.position.x - uFOController.TargetPos.x) < 0.01f && !UFOController.HuntFinish)
        {
            uFOController.startHunt = false;
            uFOController._Ani.SetTrigger("IsHuntting");
            uFOController._FSM.NowState = uFOController._ufo.Hunt;
            AudioManager.SFX_ES.Trigger("Beam");
            ObserverSystem.share.Notify(uFOController.m_Targets[uFOController.TargetID].name, AnimalState.Help);
        }
    }


}
