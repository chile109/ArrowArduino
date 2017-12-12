using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Idle : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Debug.Log("Animal idle!");
        var control = Obj.GetComponent<AnimalController>();

        Vector3 pos = Vector3.MoveTowards(Obj.transform.position,
                 control.m_targetPos, control.MoveSpeed * Time.deltaTime);

        if(Vector3.Distance(pos, control.m_targetPos) < 0.1f)
        {
            control.m_target = control.m_target == Target.Left ? Target.Right : Target.Left;
            control.SetTarget();
        }

        Obj.transform.position = pos;
    }
}
