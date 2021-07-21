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

        if(ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if(ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetAttackRange(ownerEntity.TargetEntity))
            {
                if (ownerEntity.EnableAttack) 
                {
                    // 움직임을 처리하는 것은 엔티티에서 & 여기서는 상태 전이만을 다룰 것
                    ownerEntity.Attack();
                    ownerEntity.EnableAttack = true;
                    ownerEntity.DisableMove();
                    ownerEntity.elapsedTime = 0f;
                }
            }
            else
            {
                if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetChaseRange(ownerEntity.TargetEntity))
                {
                    if (!ownerEntity.EnableAttack)
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
        if (ownerEntity.EmptyTarget == true)
        {
            StateChange(StateControl.BATTLE_STATE.IDLE);
            return false;
        }

        return true;
    }
}
