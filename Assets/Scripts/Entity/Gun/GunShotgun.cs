using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShotgun : Gun
{
    private int m_ShotgunMaxAmmo = 12;
    private float m_ShotgunFireRate = 0.5f;
    private float m_ShotgunEffectRate = 0.04f;
    private float m_ShotgunReloadTime = 2.0f;
    private float m_ShotgunDamage = 75f;

    private LineRenderer[] m_LineRenderers;

    protected override void GunSetUp()
    {
        m_FireState = FireState.Ready;

        MaxAmmo = m_ShotgunMaxAmmo;
        CurAmmo = m_ShotgunMaxAmmo;

        FireRate = m_ShotgunFireRate;
        EffectRate = m_ShotgunEffectRate;
        ReloadTime = m_ShotgunReloadTime;

        FireDistance = 50f;
        LastFireTime = .0f;
        Damage = m_ShotgunDamage;

        InfinityMode = false;

        Name = "Shotgun";

        this.transform.localPosition = new Vector3(-0.17f, -0.04f, -0.01f);

        m_LineRenderers = GetComponentsInChildren<LineRenderer>();
        
        foreach(var line in m_LineRenderers)
        {
            line.positionCount = 2;
            line.enabled = false;
        }
    }

    protected override void GunFire()
    {
        Vector3[] hitPositions = new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero };
        Vector3[] lineDir = new Vector3[3] { FireTransform.forward, FireTransform.forward, FireTransform.forward };

        Quaternion rotation = Quaternion.Euler(0, -15, 0);
        Matrix4x4 rm = Matrix4x4.Rotate(rotation);
        lineDir[0] = rm.MultiplyPoint3x4(FireTransform.forward);

        rotation = Quaternion.Euler(0, 15, 0);
        rm = Matrix4x4.Rotate(rotation);
        lineDir[2] = rm.MultiplyPoint3x4(FireTransform.forward);

        for(int i = 0; i < 3; i++)
        {
            HitLineRenderer(lineDir[i], out hitPositions[i]);
        }

        if (CurAmmo > 0)
        {
            for(int i = 0; i < 3; i++)
            {
                m_LineRenderers[i].SetPosition(0, FireTransform.position);
                m_LineRenderers[i].SetPosition(1, hitPositions[i]);
            }

            // Coroutine 안전하게 사용하는 방법
            if (coFireEffect != null)
                StopCoroutine(coFireEffect);

            coFireEffect = StartCoroutine(Shotgun_FireEffect());

            if (!InfinityMode)
                CurAmmo--;
        }
        else
        {
            m_FireState = FireState.Empty;
        }

    }

    private void HitLineRenderer(Vector3 dir, out Vector3 hitPosition)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Dead"));
        layerMask = ~layerMask;

        if (Physics.Raycast(FireTransform.position, dir, out var hit, FireDistance, layerMask))
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
    }

    private IEnumerator Shotgun_FireEffect()
    {
        // 파티클 시작
        m_MuzzleParticle.Play();
        m_ShellEjectParticle.Play();

        foreach(var line in m_LineRenderers)
            line.enabled = true;
        
        yield return new WaitForSeconds(EffectRate);

        foreach (var line in m_LineRenderers)
            line.enabled = false;
    }

    public override void Reload()
    {
        base.Reload();

        StartCoroutine(Shotgun_Reload());
    }

    private IEnumerator Shotgun_Reload()
    {
        m_FireState = FireState.Reloading;

        yield return new WaitForSeconds(ReloadTime);

        CurAmmo = MaxAmmo;

        m_FireState = FireState.Ready;
        UIManager.UpdateAmmoText(CurAmmo, MaxAmmo);
    }
}