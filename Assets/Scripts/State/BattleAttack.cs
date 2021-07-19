using System;


public class BattleAttack : State
{
    public BattleAttack(StateControl control, Action<StateControl.BATTLE_STATE> callback)
        : base(control, callback)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (!CheckState())
            return;
    }

    BaseEntity EvaluateTarget()
    {
        return new BaseEntity();
    }

    public override void Update()
    {
        base.Update();

        stateControl.TargetEntity = EvaluateTarget();

        ////if (stateControl.TargetEntity != null)
        ////    StateChange(StateControl.BATTLE_STATE.);

        //else
        //{

        //}
    }

    public override bool CheckState()
    {
        return true;
    }
}
