using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseEntity
{
    public bool IsFired { get; private set; }
    public int MaxAmmo { get; protected set; }
    public int CurAmmo { get; protected set; }

    public float FireRate { get; protected set; }

    public Transform FireTransform;


    public Vector3 gunPosition;
    public Transform leftHandMount; // 왼손이 위치할 지점
    public Transform rightHandMount; // 오른손이 위치할 지점

    public ParticleSystem m_MuzzleParticle;
    public ParticleSystem m_ShellEjectParticle;

    void GunSetUp()
    {
        IsFired = false;
        MaxAmmo = 15;
        CurAmmo = 15;
        FireRate = 0.2f;

        this.transform.localPosition = new Vector3(-0.2f, -0.04f, 0.17f);
    }


    protected override void Start()
    {
        SetUpOperation += GunSetUp;

        base.Start();
    }

    public void Fire()
    {
        m_MuzzleParticle.Play();
        m_ShellEjectParticle.Play();
    }
}
