using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이동

public partial class PlayerEntity : BattleEntity
{
    public float Speed_Move = 5.0f;
    public float Speed_Rotate = 180.0f;

    public string m_MovementAxisX = "Vertical";
    public string m_MovementAxisY = "Horizontal";

    public string m_FireButton = "";
    public string m_ReloadButton = "";

    public string m_UseItemButton = "";

    private Vector3 mouseDir; 

    void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Vector3 dir = Vector3.zero;

        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 targetPos = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            mouseDir = targetPos.normalized;
            this.transform.forward = mouseDir;
        }
    }

    void Move()
    {
        // forward를 기준으로 x 축 y축 이동하는 것 추가
        Vector3 movementForward = Input.GetAxis(m_MovementAxisX) * transform.forward;
        Vector3 movementRightward = Input.GetAxis(m_MovementAxisY) * transform.right;

        Vector3 movement = (movementForward + movementRightward).normalized * Speed_Move * Time.deltaTime;

        m_Rigidbody.MovePosition(this.transform.position + movement);
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (m_Gun.m_FireState == Gun.FireState.Ready)
            {
                m_Gun.Fire();
            }
            else if (m_Gun.m_FireState == Gun.FireState.Empty)
            {
                PlayerReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerReload();
        }
    }

    private void PlayerReload()
    {
        m_Gun.Reload();
        m_Animator.SetTrigger("Reload");
    }
}
