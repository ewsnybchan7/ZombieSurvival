using System;
using UnityEngine;

public class BattlePatrol : State
{
    private bool Goal;
    private bool IsOnce;

    public BattlePatrol(StateControl control, Action<StateControl.BATTLE_STATE> callback)
        : base(control, callback)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (!CheckState())
            return;

        Goal = true;
    }

    public override void Update()
    {
        base.Update();

        if (ownerEntity.EmptyTarget == true)
        {
            StateChange(StateControl.BATTLE_STATE.IDLE);
            return;
        }

        if (ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if (Goal)
            {
                ownerEntity.EnableMove();
                ownerEntity.SetDestination(ownerEntity.TargetEntity.Position);
                Goal = false;
            }

            StateControl.BATTLE_STATE state = StateOperation();

            switch (state)
            {
                case StateControl.BATTLE_STATE.PATROL:
                    
                    break;
                default:
                    StateChange(state);
                    break;
            }
        }
    }

    private StateControl.BATTLE_STATE StateOperation()
    {
        StateControl.BATTLE_STATE state = StateControl.BATTLE_STATE.PATROL;

        ownerEntity.TargetEntity = stateControl.FindPlayerEntity(out state) ?? ownerEntity.TargetEntity;

        if(!(ownerEntity.TargetEntity is PlayerEntity))
        {
            state = IsInPatrolPoint();
        }

        return state;
    }

    private StateControl.BATTLE_STATE IsInPatrolPoint()
    {
        if (ownerEntity is ZombieEntity zombie)
        {
            Vector3 vec = EntityManager.Instance.PatrolPoints[zombie.CurrentPoint].Position;

            float dist = Vector3.Distance(ownerEntity.Position, vec);
            
            if (dist < 1)
            {
                Goal = true;
                return StateControl.BATTLE_STATE.IDLE;
            }
        }

        return StateControl.BATTLE_STATE.PATROL;
    }

    public override bool CheckState()
    {
        if(ownerEntity.EmptyTarget == true)
        {
            StateChange(StateControl.BATTLE_STATE.IDLE);

            return false;
        }

        return true;
    }
}
