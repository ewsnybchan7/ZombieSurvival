using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class ZombieEntity : BattleEntity
{
    protected float m_PatrolSpeed = 0.5f;
    protected float m_ChaseSpeed = 1.5f;

    protected float m_ZombieDamage = 5;
    protected float m_ZombieMaxHp = 100.0f;

    protected float m_ZombieAttackCoolTime = 2;

    public float m_ZombieAttackRange = 1f;
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
        m_StateControl.Update();

        base.Update();
    }

    // Update is called once per frame

    private void ZombieSetUp()
    {
        EntityType = EntityManager.EntityType.Zombie;
        gameObject.layer = LayerMask.NameToLayer("Zombie");

        if (m_StateControl == null)
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
        EnableAttack = true;

        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
            m_Animator.enabled = true;
        }
        if (m_Collider == null)
        {
            m_Collider = GetComponent<CapsuleCollider>();
            m_Collider.enabled = true;
        }
        if (m_NavMeshAgent == null)
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_NavMeshAgent.enabled = true;
        }
        
        m_NavMeshAgent.enabled = true;
        m_Animator.enabled = true;
        m_Collider.enabled = true;
    }

    private void ZombieIdleOp()
    {
        m_Animator?.SetInteger("type", 0);
        DisableMove();
    }

    private void ZombiePatrolOp()
    {
        m_Animator?.SetInteger("type", 1);
    }

    private void ZombieChaseOp()
    {
        m_Animator?.SetInteger("type", 2);
        EnableMove();
        SetChaseMode();
        SetDestination(TargetEntity.Position);
    }

    private void ZombieAttackOp()
    {
        if (EnableAttack)
        {
            Attack();
        }

    }

    public override void Attack()
    {
        if(!IsAttacking)
            StartCoroutine(ZombieAttack());
    }


    private IEnumerator ZombieAttack()
    {
        IsAttacking = true;
        EnableAttack = false;

        m_Animator?.SetInteger("type", 3);
       

        float StartAttackTime = Time.time;

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            TargetEntity.GetComponent<IDamageable>().OnDamaged(0.2f);
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.2f) 
                continue;

            if (!m_StateControl.IsTargetAttackRange(TargetEntity))
                break;
        }

        m_Animator.SetInteger("type", 0);

        IsAttacking = false;

        yield return new WaitUntil(() => Time.time - StartAttackTime > AttackCoolTime);
        EnableAttack = true;
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
        if(GameManager.Instance.Score % 100 == 0) 
        UIManager.UpdateScoretText();

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);
        EntityManager.ReturnEntity(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnableAttack = true;
    }

    private void OnTriggerExit(Collider other)
    {
        EnableAttack = false;
    }
}