using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ZombieEntity : BattleEntity
{
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Player") 
        //{
        //    IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        //    Attack(target);
        //}
    }

    //void Attack(IDamageable player)
    //{
    //    player.OnDamaged(0);
    //    m_Animator.SetBool("Attack", true);
    //    m_NavMeshAgent.enabled = false;
    //    IsAttack = true;
    //    state = AttackState.Goal;
    //}

    void Patrol()
    {
        //if (Dead) return;

        //if (IsAttack && player)
        //{
        //    StopAttack();

        //    float xDist = Mathf.Abs(transform.position.x - player.transform.position.x);
        //    float zDist = Mathf.Abs(transform.position.z - player.transform.position.z);

        //    if (xDist > 1 || zDist > 1)
        //    {
        //        player = null;
        //        Chase = false;
        //        IsAttack = false;
        //    }

        //    return;
        //}

        //Collider[] overlapedPlayer = Physics.OverlapSphere(this.transform.position, PatrolDistance, LayerMask.GetMask("Player"));

        //if (overlapedPlayer.Length > 0)
        //{
        //    state = AttackState.Chase;
        //    player = overlapedPlayer[0].GetComponent<PlayerEntity>();
        //    Chase = true;
        //    m_NavMeshAgent.speed = 1.5f;

        //    m_Animator.SetBool("Chase", true);
        //    m_Animator.SetBool("Walk", false);
        //    m_Animator.SetBool("Idle", false);

        //    m_NavMeshAgent.SetDestination(player.transform.position);
        //}
        //else
        //{
        //    player = null;
        //    Chase = false;

        //    if (state == AttackState.Chase || state == AttackState.Goal || state == AttackState.Idle)
        //    {
        //        if(PatrolCoroutine == null)
        //        {
        //            PatrolCoroutine = StartCoroutine(FindPatrolPoint());
        //            IsPatrolRoutineEnd = false;
        //        }
        //        else
        //        {
        //            if(IsPatrolRoutineEnd)
        //            {
        //                StopCoroutine(PatrolCoroutine);
        //                PatrolCoroutine = null;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //float xDist = Mathf.Abs(transform.position.x - PatrolPoints[currentPointIndex].position.x);
        //        //float zDist = Mathf.Abs(transform.position.z - PatrolPoints[currentPointIndex].position.z);

        //        //if(xDist < 1 && zDist < 1)
        //        //{
        //        //    state = AttackState.Goal;
        //        //}
        //    }
        //}
    }

    private IEnumerator FindPatrolPoint()
    {
        //m_Animator.SetBool("Chase", false);
        //m_Animator.SetBool("Walk", false);
        //m_Animator.SetBool("Idle", true);

        //state = AttackState.Idle;
        //m_NavMeshAgent.enabled = false;

        //yield return new WaitForSeconds(2.0f);

        //state = AttackState.Patrol;
        //m_Animator.SetBool("Chase", false);
        //m_Animator.SetBool("Walk", true);
        //m_Animator.SetBool("Idle", false);

        //m_NavMeshAgent.enabled = true;
        //m_NavMeshAgent.speed = PatrolSpeed;
        //m_NavMeshAgent.ResetPath();

        ////m_NavMeshAgent.SetDestination([currentPointIndex].transform.position);
        //IsPatrolRoutineEnd = true;
        yield return null;

    }

    private IEnumerator StopAttack()
    {
        //yield return new WaitUntil(() => !IsAttack);
        //m_Animator.SetBool("Attack", false);
        yield return null;
    }
}
