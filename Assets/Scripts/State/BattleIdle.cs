using System;

public class BattleIdle : State
{
    public BattleIdle(StateControl control, Action<StateControl.BATTLE_STATE> callback)
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
    }

    void EvaluateTarget()
    {

    }

    public override bool CheckState()
    {
        return true;
    }
}
