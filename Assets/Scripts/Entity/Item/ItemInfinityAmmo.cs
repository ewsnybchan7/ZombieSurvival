using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfinityAmmo : ItemEntity
{
    public float ItemTime = 4.0f;

    protected override void ItemSetUpOp()
    {
        EntityType = EntityManager.EntityType.Item;

        ItemCode = 3001;

        ItemOperation += OnUseInfinityAmmo;
    }

    private void OnUseInfinityAmmo()
    {
        EntityManager.Instance.MainPlayer.OnInfinityMode(ItemTime);
        EntityManager.ReturnEntity(this);
    }
}
