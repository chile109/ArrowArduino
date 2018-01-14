using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANI_Traped : BaseState
{
    public override void StateDoing(GameObject Obj)
    {
        AnimalController2 Control = Obj.GetComponent<AnimalController2>();
        Control.InBubble = true;

        Animator _animat = Obj.GetComponent<Animator>();
        _animat.SetTrigger("IsArrested");

        Vector3 _InitPos = Obj.transform.position;

        Vector3 goal = new Vector3(Obj.transform.position.x, 3, Obj.transform.position.z);

        LeanTween.move(Obj, goal, 3f).setOnUpdate((Vector2 val) =>
        {
            var NowPos = Camera.main.WorldToScreenPoint(Obj.transform.position);

            if (!Control.InBubble)
            {
                UFOController.HuntFinish = true;
                LeanTween.cancel(Obj);
            }
                
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 0].y)
                Control.V_pos = 0;
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 1].y)
                Control.V_pos = 1;
            if (NowPos.y > TargetSystem.ShootPoint[Control.H_pos, 2].y)
                Control.V_pos = 2;

        }).setOnComplete(_ =>
        {
            Obj.SetActive(false);
            ObserverSystem.share.Notify(Obj.name, AnimalState.Idle);
            UFOController.HuntFinish = true;
        });
    }
}
