using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : BaseEntity, IDamageable
{
    public const float MAX_HP = 100.0f; // 최대 체력
    public float CurrentHp { get; protected set; } // 현재 체력
    public float Damage { get; protected set; }
    public bool Dead => CurrentHp <= 0; // 죽음 여부
    public event Action OnDeath; // 죽음 이벤트

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
