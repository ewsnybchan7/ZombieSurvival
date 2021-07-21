using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class ZombieEntity : BattleEntity
{
    private float m_ZombieDamage = 25.0f;
    private float ZOMBIE_MAX_HP = 100.0f;

    public int CurrentPoint { get; set; }

    public Image HPImage;

    protected override void Start()
    {
        SetUpOperation += ZombieSetUp;
        OnDamagedOperation += ZombieOnDamaged;

        IdleStateOperation += ZombieIdleOp;
        PatrolStateOperation += ZombiePatrolOp;
        ChaseStateOperation += ZombieChaseOp;
        AttackStateOperation += ZombieAttackOp;
        EndStateOperation += ZombieEndOp;

        base.Start();
    }

    protected override void Update()
    {
        m_StateControl.Update();

        base.Update();
    }

    // Update is called once per frame

    private void ZombieSetUp()
    {
        m_Animator = GetComponent<Animator>();

        m_Collider = GetComponent<CapsuleCollider>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_NavMeshAgent.enabled = true;
        m_Animator.enabled = true;
        m_Collider.enabled = true;

        // Hp
        MaxHp = ZOMBIE_MAX_HP;
        CurrentHp = ZOMBIE_MAX_HP;
        HPImage.fillAmount = CurrentHp / MaxHp;

        Damage = m_ZombieDamage;

        ChaseRange = 5f;
        AttackRange = 0.8f;

        PatrolSpeed = 0.5f;
        ChaseSpeed = 1.5f;

        EntityType = EntityManager.EntityType.Zombie;

        m_StateControl = new StateControl(this);
        m_StateControl.ChangeState(StateControl.BATTLE_STATE.IDLE);
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
        if(!m_StateControl.IsAttacked)
        {
            m_Animator?.SetInteger("type", 3);
        }

        ZombieAttackAnimControl();
    }

    private void ZombieOnDamaged()
    {
        HPImage.fillAmount = CurrentHp / MaxHp;
    }

    private void ZombieEndOp()
    {
        m_Animator.SetTrigger("Die");

        DisableMove();

        m_Collider.enabled = false;

        StartCoroutine(Death());
    }

    Coroutine DeathCoroutine = null;

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);
        EntityManager.ReturnEntity(this);
    }

    private void ZombieAttackAnimControl()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f &&
            m_StateControl.eState != StateControl.BATTLE_STATE.ATTACK)
        {
            m_Animator.enabled = false;
        }
        else if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            !m_StateControl.IsAttacked)
        {
            m_Animator.SetInteger("type", 0);
        }
    }
}
