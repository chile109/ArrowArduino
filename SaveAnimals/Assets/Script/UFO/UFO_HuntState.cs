using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UFO_HuntState : BaseState
{

    public override void StateDoing(GameObject Obj)
    {
        UFOController uFOController = Obj.GetComponent<UFOController>();
        uFOController.Beam.SetActive(true);

        if (UFOController.HuntFinish)
        {
            Debug.Log("Finish huntting");
            uFOController._FSM.NowState = uFOController._ufo.Idle;
            UFOController.HuntFinish = false;

            uFOController.FoolAround(3);
        }

    }
}
