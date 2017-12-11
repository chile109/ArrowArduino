using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Idle : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Debug.Log("Animal idle!");
    }
}
