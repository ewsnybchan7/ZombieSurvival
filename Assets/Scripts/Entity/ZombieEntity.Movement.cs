using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ZombieEntity : BattleEntity
{
    public Transform leftHand;
    public Transform rightHand;

    public float PatrolDistance { get; protected set; }
    public enum AttackState
    {
        Find,
        Goal,
        Patrol,
        Chase
    }

    public AttackState state = AttackState.Patrol;
    private PlayerEntity player;
    private int currentPointIndex;

    void Attack()
    {
        //PlayerEntity playerEntity = player[0].GetComponent<PlayerEntity>();
        //playerEntity.OnDamaged()
    }

    void Patrol()
    {
        if (Dead) return;

        Collider[] overlapedPlayer = Physics.OverlapSphere(this.transform.position, PatrolDistance, LayerMask.GetMask("Player"));

        if(overlapedPlayer.Length > 0)
        {
            state = AttackState.Chase;
            player = overlapedPlayer[0].GetComponent<PlayerEntity>();
            Chase = true;

            m_NavMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            player = null;
            Chase = false;

            if (state == AttackState.Chase || state == AttackState.Goal || state == AttackState.Find)
            {
                StartCoroutine(FindPatrolPoint());
            }
            else
            {
                float xDist = Mathf.Abs(transform.position.x - PatrolPoints[currentPointIndex].position.x);
                float zDist = Mathf.Abs(transform.position.z - PatrolPoints[currentPointIndex].position.z);

                if(xDist < 1 && zDist < 1)
                {
                    state = AttackState.Goal;
                }
            }
        }
    }

    private IEnumerator FindPatrolPoint()
    {
        state = AttackState.Find;
        m_Animator.SetBool("Chase", false);
        m_NavMeshAgent.enabled = false;

        yield return new WaitForSeconds(1.0f);

        state = AttackState.Patrol;
        m_Animator.SetBool("Chase", true);

        m_NavMeshAgent.enabled = true;
        m_NavMeshAgent.ResetPath();

        currentPointIndex = Random.Range(0, PatrolPoints.Count);
        m_NavMeshAgent.SetDestination(PatrolPoints[currentPointIndex].transform.position);
    }
}
