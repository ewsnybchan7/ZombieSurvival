using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity, IDamageable
{
    public float MaxHp { get; protected set; } // 최대 체력
    public float CurrentHp { get; protected set; } // 현재 체력
    public float Damage { get; protected set; }
    public bool Dead => CurrentHp <= 0; // 죽음 여부
    public event Action OnDeath; // 죽음 이벤트

    protected Rigidbody m_Rigidbody;
    protected Animator m_Animator;

    protected override void Start()
    {
        SetUpOperation += BattleSetUp;

        base.Start();
    }

    public virtual void OnDamaged(float damage)
    {
        CurrentHp -= damage;

        if(Dead)
        {
            OnDeath?.Invoke();
        }
    }

    public void BattleSetUp()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }
}
