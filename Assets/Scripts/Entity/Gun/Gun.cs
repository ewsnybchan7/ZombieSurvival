using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : BaseEntity, IShotable
{
    public enum FireState
    {
        Ready = 0,
        Empty,
        Reloading
    }

    public FireState m_FireState { get; protected set; }
    public int MaxAmmo { get; set; }
    public int CurAmmo { get; set; }

    public bool InfinityMode { get; set; }

    public float FireRate { get; protected set; }
    public float EffectRate { get; protected set; }
    public float ReloadTime { get; protected set; }
    public float FireDistance { get; protected set; }
    
    public float Damage { get; protected set; }

    public string Name { get; protected set; }

    public Transform FireTransform;

    public Transform leftHandMount; // 왼손이 위치할 지점
    public Transform rightHandMount; // 오른손이 위치할 지점

    public ParticleSystem m_MuzzleParticle;
    public ParticleSystem m_ShellEjectParticle;

    public float LastFireTime;

    protected delegate void FireOp();
    protected event FireOp FireOperation;

    protected Coroutine coFireEffect = null;

    private void Awake()
    {
        SetUpOperation += GunSetUp;
        FireOperation += GunFire;
    }

    protected abstract void GunSetUp();
    protected abstract void GunFire();

    public void Fire()
    {
        float curTime = Time.time;

        // 준비 상태
        if (m_FireState == FireState.Ready && curTime - LastFireTime > FireRate)
        {
            LastFireTime = curTime;

            FireOperation?.Invoke();
        }

        // 임시
        UIManager.UpdateAmmoText(CurAmmo, MaxAmmo);
    }

    public virtual void Reload()
    {
        if(m_FireState == FireState.Reloading || CurAmmo >= MaxAmmo)
        {
            return;
        }
    }
}