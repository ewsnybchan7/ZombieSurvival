using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseEntity, IShotable
{
    public enum FireState
    {
        Ready = 0,
        Empty,
        Reloading
    }

    public FireState m_FireState { get; protected set; }
    public int MaxAmmo { get; protected set; }
    public int CurAmmo { get; protected set; }

    public float FireRate { get; protected set; }
    public float EffectRate { get; private set; }
    public float ReloadTime { get; protected set; }
    public float FireDistance { get; protected set; }
    
    public float Damage { get; protected set; }

    public Transform FireTransform;

    public Vector3 gunPosition;
    public Transform leftHandMount; // 왼손이 위치할 지점
    public Transform rightHandMount; // 오른손이 위치할 지점

    public ParticleSystem m_MuzzleParticle;
    public ParticleSystem m_ShellEjectParticle;

    private LineRenderer m_LineRenderer;

    private float LastFireTime;

    protected delegate void FireOp();
    protected event FireOp FireOperation;

    void GunSetUp()
    {
        m_FireState = FireState.Ready;

        MaxAmmo = 15;
        CurAmmo = 15;
        FireRate = 0.12f;
        EffectRate = 0.03f;
        ReloadTime = 1.4f;
        FireDistance = 50f;
        LastFireTime = .0f;
        Damage = 25f;

        this.transform.localPosition = new Vector3(-0.2f, -0.04f, 0.17f);

        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
        m_LineRenderer.enabled = false;

        FireOperation += Uzi_Fire;
    }


    protected override void Start()
    {
        SetUpOperation += GunSetUp;

        base.Start();
    }

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

    
    private void Uzi_Fire()
    {
        Vector3 hitPosition = Vector3.zero;

        int layerMask = 1 << LayerMask.NameToLayer("Zombie");
        if (Physics.Raycast(FireTransform.position, FireTransform.forward, out var hit, FireDistance, layerMask))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamaged(Damage);
            }

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = FireTransform.position + FireTransform.forward * FireDistance;
        }

        CurAmmo--;
        if (CurAmmo > 0)
        {
            m_LineRenderer.SetPosition(0, FireTransform.position);
            m_LineRenderer.SetPosition(1, hitPosition);

            // Coroutine 안전하게 사용하는 방법
            if (coFireEffect != null)
                StopCoroutine(coFireEffect);

            coFireEffect = StartCoroutine(Uzi_FireEffect());
        }
        else
        {
            m_FireState = FireState.Empty;
        }
    }

    Coroutine coFireEffect = null;
    private IEnumerator Uzi_FireEffect()
    {
        // 파티클 시작
        m_MuzzleParticle.Play();
        m_ShellEjectParticle.Play();

        m_LineRenderer.enabled = true;

        yield return new WaitForSeconds(EffectRate);

        m_LineRenderer.enabled = false;
    }

    public void Reload()
    {
        if(m_FireState == FireState.Reloading || CurAmmo >= MaxAmmo)
        {
            return;
        }

        StartCoroutine(Uzi_Reload());
    }

    private IEnumerator Uzi_Reload()
    {
        m_FireState = FireState.Reloading;

        yield return new WaitForSeconds(ReloadTime);

        CurAmmo = MaxAmmo;

        m_FireState = FireState.Ready;
    }
}