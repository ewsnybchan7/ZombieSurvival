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

        if(ownerEntity.EmptyTarget == true)
        {
            StateChange(StateControl.BATTLE_STATE.IDLE);
            return;
        }

        if(ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if(ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetAttackRange(ownerEntity.TargetEntity))
            {
                if (!stateControl.IsAttacked) 
                {
                    Debug.Log("Damage");
                    ownerEntity.Attack();
                    stateControl.IsAttacked = true;
                    ownerEntity.DisableMove();
                    stateControl.elapsedTime = 0f;
                }
            }
            else
            {
                if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetChaseRange(ownerEntity.TargetEntity))
                {
                    if (stateControl.IsAttacked)
                        StateChange(StateControl.BATTLE_STATE.IDLE);
                    else
                        StateChange(StateControl.BATTLE_STATE.CHASE);
                }
                else
                {
                    StateChange(StateControl.BATTLE_STATE.IDLE);
                }
            }
        }
    }

    public override bool CheckState()
    {
        return true;
    }
}
