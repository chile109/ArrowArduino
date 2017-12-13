using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Fleeing : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        //Debug.Log("Animal Fleeing!");

        Obj.GetComponent<Animator>().SetTrigger("IsDangerous");

        var control = Obj.GetComponent<AnimalController>();

        Vector3 pos = Vector3.MoveTowards(Obj.transform.position,
               control.m_targetPos, control.FleeSpeed * Time.deltaTime);

        if (Vector3.Distance(pos, control.m_targetPos) < 0.1f)
        {
            control.m_target = control.m_target == 0 ? Target.Right : 0;
            control.SetTarget();
        }

        Obj.transform.position = pos;
    }
}
