using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity, IDamageable
{
    public const float MAX_HP = 100.0f; // �ִ� ü��
    public float CurrentHp { get; protected set; } // ���� ü��
    public float Damage { get; protected set; }
    public bool Dead => CurrentHp <= 0; // ���� ����
    public event Action OnDeath; // ���� �̺�Ʈ

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
        CurrentHp = MAX_HP;
    }

    public BattleEntity()
    {
        SetUpOperation += BattleSetUp;
    }
}
