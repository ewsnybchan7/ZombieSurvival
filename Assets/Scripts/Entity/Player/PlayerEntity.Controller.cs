using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̵�

public partial class PlayerEntity : BattleEntity
{
    public float Speed_Move = 7.0f;
    public float Speed_Rotate = 180.0f;

    public string m_MovementAxisX = "Vertical";
    public string m_MovementAxisY = "Horizontal";

    public string m_FireButton = "";
    public string m_ReloadButton = "";

    private Vector3 mouseDir; 

    void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 dir = Vector3.zero;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 targetPos = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            mouseDir = targetPos.normalized;
            this.transform.forward = mouseDir;
        }
    }

    void Move()
    {
        // forward�� �������� x �� y�� �̵��ϴ� �� �߰�
        Vector3 movementForward = Input.GetAxis(m_MovementAxisX) * transform.forward;
        Vector3 movementRightward = Input.GetAxis(m_MovementAxisY) * transform.right;

        Vector3 movement = (movementForward + movementRightward).normalized * Speed_Move * Time.deltaTime;

        m_Rigidbody.MovePosition(this.transform.position + movement);
    }

    void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            m_Gun.Fire(mouseDir);
        }
    }
}
