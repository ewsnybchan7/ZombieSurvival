using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI; 

public partial class ZombieEntity : BattleEntity
{
    private float m_ZombieDamage = 25.0f;
    private float ZOMBIE_MAX_HP = 100.0f;

    public int CurrentPoint { get; set; }

    public Image HPImage;

    private Coroutine PatrolCoroutine = null;
    private bool IsPatrolRoutineEnd = false;

    Coroutine TimerCoroutine = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        SetUpOperation += ZombieSetUp;
        OnDeath += ZombieDeath;
        OnDamagedOperation += ZombieOnDamaged;

        IdleStateOperation += ZombieIdleOp;
        PatrolStateOperation += ZombiePatrolOp;
        ChaseStateOperation += ZombieChaseOp;
        AttackStateOperation += ZombieAttackOp;

        base.Start();
    }

    protected override void Update()
    {
        m_StateControl.Update();

        base.Update();

        ZombieAttackAnim();
    }

    // Update is called once per frame

    private void ZombieSetUp()
    {
        MaxHp = ZOMBIE_MAX_HP;
        CurrentHp = ZOMBIE_MAX_HP;
        Damage = m_ZombieDamage;

        ChaseRange = 5f;
        AttackRange = 0.8f;

        PatrolSpeed = 0.5f;
        ChaseSpeed = 1.5f;

        m_NavMeshAgent = GetComponent<NavMeshAgent>();

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
            Debug.Log("Anim");
            m_Animator?.SetInteger("type", 3);
        }
    }

    private void ZombieDeath()
    {
        m_Animator.SetTrigger("Die");

        DisableMove();

        Invoke("Death", 4f);
    }

    private void ZombieOnDamaged()
    {
        HPImage.fillAmount = CurrentHp / MaxHp;
    }

    private void Death()
    {
        this.gameObject.SetActive(false);
        Destroy(this);
    }

    private void ZombieAttackAnim()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f &&
            m_StateControl.eState != StateControl.BATTLE_STATE.ATTACK)
        {
            m_Animator.enabled = false;
        }

        m_Animator.enabled = true;
    }
}
