using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : BattleEntity
{
    public Gun m_Gun;
    public Transform gunTransform; // �� ��ġ�� ������
    private Transform leftHandMount; // �޼��� ��ġ�� ����
    private Transform rightHandMount; // �������� ��ġ�� ����

    public ItemEntity EquipItem;

    private const float PLAYER_MAX_HP = 100;

    public ParticleSystem m_BloodParticle;

    private void Awake()
    {
        SetUpOperation += PlayerSetUp;
        OnDeath += PlayerDeath;
        OnDamagedOperation += PlayerOnDamaged;


    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_Gun.gameObject.SetActive(true);
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!Dead)
        {
            Rotate();
            Move();

            // animator ���� x�� y�� ���� �����ϴ� ���� ���
            m_Animator.SetFloat("MoveX", Input.GetAxis("Vertical"));
            m_Animator.SetFloat("MoveY", Input.GetAxis("Horizontal"));
        }
    }

    protected override void Update()
    {
        PlayerInput();
    }

    private void OnDisable()
    {
        m_Gun.gameObject.SetActive(false);
    }

    private void PlayerSetUp()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<CapsuleCollider>();

        MaxHp = PLAYER_MAX_HP;
        CurrentHp = PLAYER_MAX_HP;

        EntityType = EntityManager.EntityType.Player;

        leftHandMount = m_Gun.leftHandMount;
        rightHandMount = m_Gun.rightHandMount;

        m_StateControl = new StateControl(this);
        //m_StateControl = new StateControl
    }

    private void PlayerOnDamaged()
    {
        UIManager.UpdateHpText(CurrentHp, MaxHp);
        Debug.Log("Damage");
        // Damage�� �Ծ��ٴ� Ŭ��Ŀ �������� ȿ�� �߰�
        // World canvas�� �ּ� ���� ������ ȿ��
        // ���� ��ƼŬ ����?
    }

    public void PlayerDeath()
    {
        m_Animator.SetTrigger("Die");

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        GameManager.Instance.IsGameOver = true;

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);
        EntityManager.ReturnEntity(this);
    }

    public void HealHp(float hp)
    {
        CurrentHp += hp;
        if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        UIManager.UpdateHpText(CurrentHp, MaxHp);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // ���� ������ gunPivot�� 3D ���� ������ �Ȳ�ġ ��ġ�� �̵�
        gunTransform.position = m_Animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ���� �����̿� ����
        m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);


        // IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� ���� �����̿� ����
        m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        m_Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        m_Animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            other.GetComponent<IItem>().OnUse();
        }
    }

    public void OnInfinityMode(float time)
    {
        StartCoroutine(InfinityModeCoroutine(time));
    }

    private IEnumerator InfinityModeCoroutine(float time)
    {
        EntityManager.Instance.MainPlayer.m_Gun.InfinityMode = true;
        yield return new WaitForSeconds(time);
        EntityManager.Instance.MainPlayer.m_Gun.InfinityMode = false;
    }
}