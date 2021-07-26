using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUzi : Gun
{
    private int m_UziMaxAmmo = 25;
    private float m_UziFireRate = 0.12f;
    private float m_UziEffectRate = 0.04f;
    private float m_UziReloadTime = 1.4f;
    private float m_UziDamage = 20f;

    private LineRenderer m_LineRenderer;

    protected override void GunSetUp()
    {
        m_FireState = FireState.Ready;

        MaxAmmo = m_UziMaxAmmo;
        CurAmmo = m_UziMaxAmmo;

        FireRate = m_UziFireRate;
        EffectRate = m_UziEffectRate;
        ReloadTime = m_UziReloadTime;

        FireDistance = 50f;
        LastFireTime = .0f;
        Damage = m_UziDamage;

        InfinityMode = false;
        
        Name = "Uzi";

        this.transform.localPosition = new Vector3(-0.13f, -0.1f, 0.03f);

        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
        m_LineRenderer.enabled = false;
    }

    protected override void GunFire()
    {
        Vector3 hitPosition = Vector3.zero;

        int layerMask = (1 << LayerMask.NameToLayer("Dead"));
        layerMask = ~layerMask;

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

        if (CurAmmo > 0)
        {
            m_LineRenderer.SetPosition(0, FireTransform.position);
            m_LineRenderer.SetPosition(1, hitPosition);

            // Coroutine 안전하게 사용하는 방법
            if (coFireEffect != null)
                StopCoroutine(coFireEffect);

            coFireEffect = StartCoroutine(Uzi_FireEffect());

            if (!InfinityMode)
                CurAmmo--;
        }
        else
        {
            m_FireState = FireState.Empty;
        }
    }

    private IEnumerator Uzi_FireEffect()
    {
        // 파티클 시작
        m_MuzzleParticle.Play();
        m_ShellEjectParticle.Play();

        m_LineRenderer.enabled = true;

        yield return new WaitForSeconds(EffectRate);

        m_LineRenderer.enabled = false;
    }

    public override void Reload()
    {
        base.Reload();

        StartCoroutine(Uzi_Reload());
    }

    private IEnumerator Uzi_Reload()
    {
        m_FireState = FireState.Reloading;

        yield return new WaitForSeconds(ReloadTime);

        CurAmmo = MaxAmmo;

        m_FireState = FireState.Ready;
        UIManager.UpdateAmmoText(CurAmmo, MaxAmmo);
    }
}