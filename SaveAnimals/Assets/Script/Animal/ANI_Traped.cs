﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Traped : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsArrested");

    }
}
