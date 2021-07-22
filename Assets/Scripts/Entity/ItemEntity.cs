using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : BaseEntity, IItem
{
    public float LivingTime { get; protected set; } = 3.0f;
    public uint ItemCode { get; protected set; }

    private void ItemSetUpOp()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual void OnUse()
    {
        Debug.Log("아이템 사용");
    }
}
