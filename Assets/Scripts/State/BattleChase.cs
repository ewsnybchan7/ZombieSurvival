using System;
using UnityEngine;

public class BattleChase : State
{
    public BattleChase(StateControl control, Action<StateControl.BATTLE_STATE> callback)
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

        if(ownerEntity.Dead)
        {
            StateChange(StateControl.BATTLE_STATE.END);
        }

        if (ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetChaseRange(ownerEntity.TargetEntity))
            {
                if (ownerEntity.EnabledMove)
                {
                    if(stateControl.IsTargetAttackRange(ownerEntity.TargetEntity))
                    {
                        StateChange(StateControl.BATTLE_STATE.ATTACK);
                    }
                }
            }
            else
            {
                if (ownerEntity.EnabledMove)
                {
                    StateChange(StateControl.BATTLE_STATE.IDLE);
                }
            }
        }
    }

    public override bool CheckState()
    {
        if (ownerEntity.EmptyTarget == true)
        {
            StateChange(StateControl.BATTLE_STATE.IDLE);
            return false;
        }

        return true;
    }
}
