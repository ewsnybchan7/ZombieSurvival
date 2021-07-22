using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleEntity : BaseEntity, IDamageable
{
    protected StateControl m_StateControl;

    public BaseEntity TargetEntity { get; set; }
    public bool HasTarget => (TargetEntity != null) ? true : false;
    public bool EmptyTarget => (TargetEntity == null) ? true : false;

    public bool EnabledMove => (m_NavMeshAgent?.enabled == true || GetComponent<Rigidbody>() != null) ? true : false;

    public float MovingSpeed { get; set; }
    public float PatrolSpeed { get; protected set; }
    public float ChaseSpeed { get; protected set; }
    public bool Chase { get; private set; }

    public bool EnableAttack { get; set; }
    public float AttackCoolTime = 2f;
    public float elapsedTime = 0f;

    public float AttackRange { get; set; }
    public float ChaseRange { get; set; }

    public float MaxHp { get; protected set; } // 최대 체력
    public float CurrentHp { get; protected set; } // 현재 체력
    public float Damage { get; protected set; }
    public bool Dead => CurrentHp <= 0; // 죽음 여부
    
    public event Action OnDeath; // 죽음 이벤트

    protected Rigidbody m_Rigidbody;
    protected NavMeshAgent m_NavMeshAgent;
    protected Animator m_Animator;
    protected CapsuleCollider m_Collider;
    
    protected delegate void IdleStateOp();
    protected event IdleStateOp IdleStateOperation;

    protected delegate void PatrolStateOp();
    protected event IdleStateOp PatrolStateOperation;

    protected delegate void ChaseStateOp();
    protected event IdleStateOp ChaseStateOperation;

    protected delegate void AttackStateOp();
    protected event IdleStateOp AttackStateOperation;

    protected delegate void HitStateOp();
    protected event IdleStateOp HitStateOperation;

    protected delegate void EndStateOp();
    protected event IdleStateOp EndStateOperation;

    protected delegate void OnDamagedOp();
    protected event OnDamagedOp OnDamagedOperation;

    protected override void Start()
    {
        SetUpOperation += BattleSetUp;

        base.Start();
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsGameOver)
        {
            switch (m_StateControl.eState)
            {
                case StateControl.BATTLE_STATE.IDLE:
                    {
                        IdleStateOperation?.Invoke();
                        break;
                    }
                case StateControl.BATTLE_STATE.PATROL:
                    {
                        PatrolStateOperation?.Invoke();
                        break;
                    }
                case StateControl.BATTLE_STATE.CHASE:
                    {
                        ChaseStateOperation?.Invoke();
                        break;
                    }
                case StateControl.BATTLE_STATE.ATTACK:
                    {
                        AttackStateOperation?.Invoke();
                        break;
                    }
                case StateControl.BATTLE_STATE.HIT:
                    {
                        HitStateOperation?.Invoke();
                        break;
                    }
                case StateControl.BATTLE_STATE.END:
                    {
                        EndStateOperation?.Invoke();
                        break;
                    }
            }
        }
    }

    public virtual void OnDamaged(float damage)
    {
        if (Dead) return;

        CurrentHp -= damage;

        OnDamagedOperation?.Invoke();

        if(Dead) OnDeath?.Invoke();
    }

    public void BattleSetUp()
    {

    }

    public void EnableMove()
    {
        if (m_NavMeshAgent != null)
            m_NavMeshAgent.enabled = true;

        if (m_Collider != null)
            m_Collider.enabled = true;

        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void DisableMove()
    {
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.isStopped = true;
            m_NavMeshAgent.speed = 0;
        }

        if (m_Rigidbody != null)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void SetPatrolMode()
    {
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.speed = PatrolSpeed;
        }
    }

    public void SetChaseMode()
    {
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.speed = ChaseSpeed;
        }
    }

    public void SetDestination(Vector3 dest)
    {
        if (m_NavMeshAgent.enabled)
        {
            m_NavMeshAgent.ResetPath();
            m_NavMeshAgent.SetDestination(dest);
        }
    }

    public void CheckEnableAttack()
    {
        if (!EnableAttack)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > AttackCoolTime)
            {
                EnableAttack = true;
            }
        }
    }

    public virtual bool Attack()
    {
        TargetEntity.GetComponent<IDamageable>().OnDamaged(Damage);
        return true;
    }
}
