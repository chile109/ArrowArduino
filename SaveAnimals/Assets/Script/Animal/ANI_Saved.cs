using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 泡泡破裂邏輯於animation中
/// </summary>
public class ANI_Saved : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        AnimalController2 Control = Obj.GetComponent<AnimalController2>();
        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsSaved");

        Vector3 Land = Obj.GetComponent<AnimalController2>().InitPos;

        LeanTween.move(Obj, Land, 2f).setOnComplete(_ =>
        {
            Debug.Log("land");
            Control._FSM.NowState = Control._ani.idle;
        });
    }
}
