using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Traped : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsArrested");

        Vector3 InitPos = Obj.transform.position;

        Vector3 goal = new Vector3(Obj.transform.position.x, 3, Obj.transform.position.z);
        LeanTween.move(Obj, goal, 10f).setOnComplete(_ =>
        {
            Debug.Log("goal");
            UFOController.HuntFinish = true;
            Obj.transform.position = InitPos;
            Obj.GetComponent<AnimalController2>().BubbleOff();
            _animat.Play("Standby");
        });
    }
}
