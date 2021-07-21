using UnityEngine;
public class StateControl
{
    public enum BATTLE_STATE {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        HIT,
        END
    }

    public BattleEntity OwnerEntity { get; private set; }
    public BaseEntity targetEntity;
    public BaseEntity TargetEntity;

    public bool HasTarget => (TargetEntity != null) ? true : false;
    public bool EmptyTarget => (TargetEntity == null) ? true : false;

    private State[] battleStates;
    public BATTLE_STATE eState { get; private set; }
    public BATTLE_STATE prevState { get; private set; }
    public BATTLE_STATE prevAttackState { get; private set; }

    public int attackType { get; private set; }
    public int skillType { get; private set; }

    public bool IsAttacked { get; set; }
    public float elapsedTime = 0f;
    public float AttackCoolTime = 2f;

    public StateControl(BattleEntity entity)
    {
        OwnerEntity = entity;

        battleStates = new State[(int)BATTLE_STATE.END];

        battleStates[(int)BATTLE_STATE.IDLE] = new BattleIdle(this, ChangeState);
        battleStates[(int)BATTLE_STATE.PATROL] = new BattlePatrol(this, ChangeState);
        battleStates[(int)BATTLE_STATE.CHASE] = new BattleChase(this, ChangeState);
        battleStates[(int)BATTLE_STATE.ATTACK] = new BattleAttack(this, ChangeState);
        battleStates[(int)BATTLE_STATE.HIT] = new BattleHit(this, ChangeState);
    }

    public void Update()
    {
        battleStates[(int)eState].Update();

        if(IsAttacked)
        {
            elapsedTime += Time.deltaTime;
            
            if(elapsedTime > AttackCoolTime)
            {
                IsAttacked = false;
            }
        }
    }

    public bool IsChangeState()
    {
        return eState == BATTLE_STATE.IDLE;
    }

    public bool IsTargetAttackRange(BaseEntity target) => Vector3.Distance(OwnerEntity.transform.position, target.transform.position) <= OwnerEntity.AttackRange;

    public bool IsTargetChaseRange(BaseEntity target) => Vector3.Distance(OwnerEntity.transform.position, target.transform.position) <= OwnerEntity.ChaseRange;

    public PlayerEntity FindPlayerEntity(out BATTLE_STATE state)
    {
        if (IsTargetChaseRange(EntityManager.Instance.MainPlayer))
        {
            if (IsTargetAttackRange(EntityManager.Instance.MainPlayer))
            {
                state = BATTLE_STATE.ATTACK;
            }
            else
            {
                state = BATTLE_STATE.CHASE;
            }

            return EntityManager.Instance.MainPlayer;
        }
        else
        {
            state = eState;
            return null;
        }
    }

    public void SetAttackType(int type)
    {
        attackType = type;
    }

    public void ChangeState(BATTLE_STATE state = BATTLE_STATE.IDLE)
    {
        prevState = eState;
        eState = state;

        battleStates[(int)prevState].Exit();
        battleStates[(int)eState].Enter();
    }


    public void SavePrevAttackState(BATTLE_STATE state)
    {
        prevAttackState = state;
    }

    private (bool, int) CalculateAttackDamage()
    {
        var isCritical = (Random.Range(1, 11) <= 2) ? true : false;
        var attackDamage = isCritical ? Random.Range(5, 10) : Random.Range(1, 3);

        return (isCritical, attackDamage);
    }

    public void SetHit(float damage)
    {
        OwnerEntity.OnDamaged(damage);

        UIManager.Instance.UpdateStatusFunc?.Invoke();
    }
}