using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Saved : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsSaved");

        Vector3 Land = Obj.GetComponent<AnimalController2>().InitPos;

        LeanTween.move(Obj, Land, 2f).setOnComplete(_ =>
        {
            Debug.Log("land");
        });
    }
}
