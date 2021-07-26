using UnityEngine;

public abstract class ItemGun : ItemEntity
{
    protected override void Awake()
    {
        SetUpOperation += ItemSetUpOp;
    }

    protected override void ItemSetUpOp()
    {

    }
}
