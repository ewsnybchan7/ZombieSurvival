using System;
using UnityEngine;

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

    public override void Update()
    {
        base.Update();

        if(ownerEntity.Dead)
        {
            StateChange(StateControl.BATTLE_STATE.END);
        }

        if (!ownerEntity.EnableAttack) 
            return;

        if (ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if (!ownerEntity.IsAttacking)
            {
                if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetAttackRange(ownerEntity.TargetEntity) == false)
                {
                    if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetChaseRange(ownerEntity.TargetEntity))
                    {
                        StateChange(StateControl.BATTLE_STATE.CHASE);
                    }
                    else
                    {
                        StateChange(StateControl.BATTLE_STATE.IDLE);
                    }
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
