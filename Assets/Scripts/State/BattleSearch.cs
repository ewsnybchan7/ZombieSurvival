using System;


public class BattleSearch : State
{
    public BattleSearch(StateControl control, Action<StateControl.BATTLE_STATE> callback)
        : base(control, callback)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (!CheckState())
            return;
    }

    public override void Update()
    {
        base.Update();

        if (stateControl.TargetEntity != null)
            return;
    }

    public override bool CheckState()
    {
        return true;
    }
}
