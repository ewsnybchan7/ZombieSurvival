using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : BattleEntity
{
    private Rigidbody m_Rigidbody;

    private Animator m_Animator;

    public Gun m_Gun;
    public Transform gunTransform; // 총 배치의 기준점
    private Transform leftHandMount; // 왼손이 위치할 지점
    private Transform rightHandMount; // 오른손이 위치할 지점

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

        // animator 설정 x축 y축 값을 설정하는 좋은 방법
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
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        gunTransform.position = m_Animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
        m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);


        // IK를 사용하여 오른손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤
        m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        m_Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        m_Animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}