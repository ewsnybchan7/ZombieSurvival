using System;


public class BattleHit : State
{
    public BattleHit(StateControl control, Action<StateControl.BATTLE_STATE> callback)
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

        if (ownerEntity.Dead)
        {
            StateChange(StateControl.BATTLE_STATE.END);
        }

        if (ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            if (ownerEntity.TargetEntity is PlayerEntity && stateControl.IsTargetAttackRange(ownerEntity.TargetEntity))
            {
                if (!stateControl.IsAttacked)
                {
                    
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
