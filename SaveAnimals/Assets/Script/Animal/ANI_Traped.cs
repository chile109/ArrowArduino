using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Traped : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        AnimalController2 Control = Obj.GetComponent<AnimalController2>();

        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsArrested");

        Vector3 InitPos = Obj.transform.position;

        Vector3 goal = new Vector3(Obj.transform.position.x, 3, Obj.transform.position.z);

        LeanTween.move(Obj, goal, 10f).setOnUpdate((Vector2 val) =>
        {
            Debug.Log(val);
            var NowPos = Camera.main.WorldToScreenPoint(Obj.transform.position);

            //Debug.Log(NowPos + " / " + TargetSystem.ShootPoint[H_pos, 0]);
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 0].y)
                Control.V_pos = 0;
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 1].y)
                Control.V_pos = 1;
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 2].y)
                Control.V_pos = 2;

        }).setOnComplete(_ =>
        {
            Debug.Log("goal");
            UFOController.HuntFinish = true;
            Obj.transform.position = InitPos;
            Obj.GetComponent<AnimalController2>().BubbleOff();
            _animat.Play("Standby");
        });
    }
}
