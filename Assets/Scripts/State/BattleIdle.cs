using System;
using UnityEngine;

public class BattleIdle : State
{
    private float Timer;
    private bool IsPause { get; set; }
    private float WaitingTime => stateControl.IsAttacked == true ? 1f : 2f;

    public BattleIdle(StateControl control, Action<StateControl.BATTLE_STATE> callback)
        : base(control, callback)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (!CheckState())
            return;

        ownerEntity.DisableMove();
    }

    public override void Update()
    {
        base.Update();

        if (ownerEntity.EntityType == EntityManager.EntityType.Zombie)
        {
            Timer += Time.deltaTime;

            if (Timer > WaitingTime)
            {
                Timer = 0;

                StateChange(FindTargetEntity());
            }
        }
    }

    private StateControl.BATTLE_STATE FindTargetEntity()
    {
        StateControl.BATTLE_STATE state = StateControl.BATTLE_STATE.IDLE;

        ownerEntity.TargetEntity = stateControl.FindPlayerEntity(out state);

        if(ownerEntity.TargetEntity == null)
        {
            ownerEntity.TargetEntity = FindNextPatrolPoint(out state);
        }

        return state;
    }

    private BaseEntity FindNextPatrolPoint(out StateControl.BATTLE_STATE state)
    {
        if(ownerEntity is ZombieEntity zombie)
        {
            int nextPoint = zombie.CurrentPoint;
            while(nextPoint == zombie.CurrentPoint)
            {
                nextPoint = UnityEngine.Random.Range(0, EntityManager.Instance.PatrolPoints.Length);
            }

            zombie.CurrentPoint = nextPoint;

            state = StateControl.BATTLE_STATE.PATROL;

            return EntityManager.Instance.PatrolPoints[zombie.CurrentPoint];
        }

        state = StateControl.BATTLE_STATE.IDLE;

        return null;
    }

    public override bool CheckState()
    {
        return true;
    }
}
