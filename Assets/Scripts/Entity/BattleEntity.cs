using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity, IDamageable
{
    public float MaxHp { get; protected set; } // �ִ� ü��
    public float CurrentHp { get; protected set; } // ���� ü��
    public float Damage { get; protected set; }
    public bool Dead => CurrentHp <= 0; // ���� ����
    public event Action OnDeath; // ���� �̺�Ʈ

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
