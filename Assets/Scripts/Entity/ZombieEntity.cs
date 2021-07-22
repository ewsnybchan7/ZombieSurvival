using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class ZombieEntity : BattleEntity
{
    protected float m_PatrolSpeed = 0.5f;
    protected float m_ChaseSpeed = 0.5f;

    protected float m_ZombieDamage = 5;
    protected float m_ZombieMaxHp = 100.0f;

    protected float m_ZombieAttackCoolTime = 2;

    protected float m_ZombieAttackRange = 0.8f;
    protected float m_ZombieChaseRange = 5f;

    public int CurrentPoint { get; set; }

    public Image HPImage;

    private void Awake()
    {
        SetUpOperation += ZombieSetUp;

        IdleStateOperation += ZombieIdleOp;
        PatrolStateOperation += ZombiePatrolOp;
        ChaseStateOperation += ZombieChaseOp;
        AttackStateOperation += ZombieAttackOp;
        EndStateOperation += ZombieEndOp;
        OnDeath += ZombieDeath;
        OnDamagedOperation += ZombieOnDamaged;
    }

    protected override void Update()
    {
        base.Update();

        float curTime = Time.time;
        if (curTime - LastAttackTime > AttackCoolTime)
        {
            EnableAttack = true;
        }

        m_StateControl.Update();
    }

    // Update is called once per frame

    private void ZombieSetUp()
    {
        EntityType = EntityManager.EntityType.Zombie;
        gameObject.layer = LayerMask.NameToLayer("Zombie");

        m_StateControl = new StateControl(this);
        m_StateControl.ChangeState(StateControl.BATTLE_STATE.IDLE);

        TargetEntity = null;

        // Speed variable
        MovingSpeed = 0f; // Player speed
        PatrolSpeed = m_PatrolSpeed;
        ChaseSpeed = m_ChaseSpeed;

        // Hp variable
        MaxHp = m_ZombieMaxHp;
        CurrentHp = m_ZombieMaxHp;

        // Hp UI setup
        HPImage.fillAmount = CurrentHp / MaxHp;

        // Attack variable
        Damage = m_ZombieDamage;
        ChaseRange = m_ZombieChaseRange;
        AttackRange = m_ZombieAttackRange;
        AttackCoolTime = m_ZombieAttackCoolTime;
        LastAttackTime = 0f;
        EnableAttack = true;

        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_NavMeshAgent.enabled = true;
        m_Animator.enabled = true;
        m_Collider.enabled = true;
    }

    private void ZombieIdleOp()
    {
        m_Animator?.SetInteger("type", 0);
    }

    private void ZombiePatrolOp()
    {
        m_Animator?.SetInteger("type", 1);
    }

    private void ZombieChaseOp()
    {
        m_Animator?.SetInteger("type", 2);  
    }

    private void ZombieAttackOp()
    {
        if (m_StateControl.eState == StateControl.BATTLE_STATE.ATTACK && EnableAttack)
        {
            Attack();
        }

        DisableMove();
        ZombieAttackAnimControl();
    }

    public override void Attack()
    {
        StartCoroutine(AttackEnd());
    }

    private IEnumerator AttackEnd()
    {
        m_Animator?.SetInteger("type", 3);
        EnableAttack = false;

        yield return new WaitUntil(() => (m_StateControl.IsTargetAttackRange(TargetEntity) == false ||
            (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) ||
            Time.time - LastAttackTime > AttackCoolTime));

        m_Animator?.SetInteger("type", 0);

        float curTime = Time.time;
        float percentage = (curTime - LastAttackTime) / AttackCoolTime;
        LastAttackTime = Time.time;

        TargetEntity.GetComponent<IDamageable>().OnDamaged(Mathf.Round(Damage * percentage));
    }

    private void ZombieOnDamaged()
    {
        HPImage.fillAmount = CurrentHp / MaxHp;
    }

    private void ZombieEndOp()
    {
        DisableMove();
    }

    private void ZombieDeath()
    {
        m_Animator.SetTrigger("Die");

        gameObject.layer = LayerMask.NameToLayer("Dead");

        GameManager.Instance.Score += 10;
        UIManager.UpdateScoretText();

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);
        EntityManager.ReturnEntity(this);
    }

    private void ZombieAttackAnimControl()
    {
        if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            EnableAttack == false)
        {
            m_Animator.SetInteger("type", 0);
        }
    }
}
