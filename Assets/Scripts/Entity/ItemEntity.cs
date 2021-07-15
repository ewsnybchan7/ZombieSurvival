using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : BaseEntity, IItem
{
    public float LivingTime { get; protected set; } = 3.0f;
    public uint ItemCode { get; protected set; }
    


    // Start is called before the first frame update
    protected override void Start()
    {
        SetUpOperation += ItemSetUpOp;

        base.Start();
    }

    private void ItemSetUpOp()
    {

    }

    protected override void Update()
    {
        
    }

    public virtual void OnUse()
    {
        Debug.Log("아이템 사용");
    }
}
