using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : BaseEntity, IShotable
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

    private LineRenderer m_LineRenderer;

    private Vector3[] m_Points;

    void GunSetUp()
    {
        IsFired = false;
        MaxAmmo = 15;
        CurAmmo = 15;
        FireRate = 0.2f;

        this.transform.localPosition = new Vector3(-0.2f, -0.04f, 0.17f);

        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
        m_Points = new Vector3[2];
        m_Points[0] = FireTransform.position;
    }


    protected override void Start()
    {
        SetUpOperation += GunSetUp;

        base.Start();
    }

    public void Fire(Vector3 dir)
    {
        m_Points[0] = FireTransform.position;
        m_Points[1] = Vector3.MoveTowards(FireTransform.position, dir * 10f, 10f);
        m_LineRenderer.SetPositions(m_Points);

        m_LineRenderer.enabled = true;

        // 파티클 시작
        m_MuzzleParticle.Play();
        m_ShellEjectParticle.Play();

        
    }
}
