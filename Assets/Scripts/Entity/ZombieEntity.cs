using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public partial class ZombieEntity : BattleEntity
{
    private float m_ZombieDamage = 25.0f;
    private float ZOMBIE_MAX_HP = 100.0f;

    public bool Chase { get; private set; }

    private NavMeshAgent m_NavMeshAgent;

    public List<Transform> PatrolPoints;

    public Image HPImage;

    // Start is called before the first frame update
    protected override void Start()
    {
        SetUpOperation += ZombieSetUp;
        OnDeath += ZombieDeath;
        OnDamagedOperation += ZombieOnDamaged;
        PatrolDistance = 5f;

        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Patrol();
    }

    private void ZombieSetUp()
    {
        MaxHp = ZOMBIE_MAX_HP;
        CurrentHp = ZOMBIE_MAX_HP;
        Damage = m_ZombieDamage;

        PatrolSpeed = 0.5f;

        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        state = AttackState.Find;
    }

    private void ZombieDeath()
    {
        m_Animator.SetBool("Chase", false);
        m_Animator.SetBool("Die", true);

        m_NavMeshAgent.enabled = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;

        StartCoroutine(Death());
    }

    private void ZombieOnDamaged()
    {
        HPImage.fillAmount = CurrentHp / MaxHp;
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);

        this.gameObject.SetActive(false);
        Destroy(this);
    }
}
