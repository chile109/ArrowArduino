using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO_IdleState : BaseState
{


    public override void StateDoing(GameObject Obj)
    {
        //Debug.Log("UFO_Idle");
        UFOController uFOController = Obj.GetComponent<UFOController>();
        uFOController.Beam.SetActive(false);

        Vector3 pos = Vector3.MoveTowards(Obj.transform.position,
                                          uFOController.TargetPos, uFOController.MoveSpeed * Time.deltaTime);
        Obj.transform.position = pos;
    }


}
