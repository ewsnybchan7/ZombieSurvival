using System;
using UnityEngine;

public class BattleEnd : State
{
    public BattleEnd(StateControl control, Action<StateControl.BATTLE_STATE> callback)
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

    public override bool CheckState()
    {
        return true;
    }
}
