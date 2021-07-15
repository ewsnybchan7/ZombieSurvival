using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : BattleEntity
{
    private Rigidbody m_Rigidbody;

    private Animator m_Animator;

    public Gun m_Gun;
    public Transform gunTransform; // �� ��ġ�� ������
    private Transform leftHandMount; // �޼��� ��ġ�� ����
    private Transform rightHandMount; // �������� ��ġ�� ����

    public ItemEntity EquipItem;

    private float m_PlayerDamage = 50.0f;

    public bool IsReload { get; private set; }

    private void OnEnable()
    {
        m_Gun.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetUpOperation += PlayerSetUp;
        OnDeath += PlayerDeath;

        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        Rotate();
        Move();

        // animator ���� x�� y�� ���� �����ϴ� ���� ���
        m_Animator.SetFloat("MoveX", Input.GetAxis("Vertical"));
        m_Animator.SetFloat("MoveY", Input.GetAxis("Horizontal"));
    }

    protected override void Update()
    {
        Attack();
    }

    private void OnDisable()
    {
        m_Gun.gameObject.SetActive(false);
    }

    public void PlayerSetUp()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        Damage = m_PlayerDamage;

        leftHandMount = m_Gun.leftHandMount;
        rightHandMount = m_Gun.rightHandMount;
    }

    public void PlayerDeath()
    {
        m_Animator.SetTrigger("Die");
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
}