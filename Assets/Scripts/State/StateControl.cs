using UnityEngine;
public class StateControl : MonoBehaviour
{
    public enum BATTLE_STATE{
        IDLE,
        SEARCH,
        PATROL,
        CHASE,
        ATTACK,
        HIT,
        END
    }

    public BattleEntity OwnerEntity { get; private set; }
    private BaseEntity targetEntity;
    public BaseEntity TargetEntity;

    public bool HasTarget => (TargetEntity != null) ? true : false;
    public bool EmptyTarget => (TargetEntity == null) ? true : false;

    private State[] battleStates;
    public BATTLE_STATE eState { get; private set; }
    public BATTLE_STATE prevState { get; private set; }
    public BATTLE_STATE prevAttackState { get; private set; }

    public int attackType { get; private set; }
    public int skillType { get; private set; }

    public StateControl(BattleEntity entity)
    {
        OwnerEntity = entity;

        battleStates[(int)BATTLE_STATE.IDLE] = new BattleIdle(this, ChangeState);
        battleStates[(int)BATTLE_STATE.SEARCH] = new BattleSearch(this, ChangeState);
        battleStates[(int)BATTLE_STATE.ATTACK] = new BattleAttack(this, ChangeState);
    }

    public void Update()
    {
        battleStates[(int)eState].Update();
    }

    public bool IsChangeState()
    {
        return eState == BATTLE_STATE.IDLE;
    }

    public bool IsTargetAttackRange() => Vector3.Distance(OwnerEntity.transform.position, targetEntity.transform.position) <= OwnerEntity.AttackRange;

    public void SetAttackType(int type)
    {
        attackType = type;
    }

    public void ChangeState(BATTLE_STATE state = BATTLE_STATE.IDLE)
    {
        prevState = eState;
        eState = state;

        Debug.Log(eState);

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