using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : BattleEntity
{
    public Gun m_Gun;
    public Transform gunTransform; // 총 배치의 기준점
    private Transform leftHandMount; // 왼손이 위치할 지점
    private Transform rightHandMount; // 오른손이 위치할 지점

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

            // animator 설정 x축 y축 값을 설정하는 좋은 방법
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
        // Damage를 입었다는 클릭커 깜빡깜빡 효과 추가
        // World canvas를 둬서 숫자 데미지 효과
        // 블러드 파티클 정도?
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