using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Idle : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("Idle");
    }

    /// <summary>
    /// 移動邏輯
    /// </summary>
    /// <param name="Obj">Object.</param>
    void fishAround(GameObject Obj)
    {
        //Debug.Log("Animal idle!");
        var control = Obj.GetComponent<AnimalController>();

        Vector3 pos = Vector3.MoveTowards(Obj.transform.position,
                 control.m_targetPos, control.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(pos, control.m_targetPos) < 0.1f)
        {
            control.m_target = control.m_target == 0 ? Target.Right : 0;
            control.SetTarget();
        }

        Obj.transform.position = pos;
    }
}
