using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealPack : ItemEntity
{
    private float healHp = 10f;

    protected override void ItemSetUpOp()
    {
        EntityType = EntityManager.EntityType.Item;

        ItemCode = 3000;

        ItemOperation += OnUseHealPack;
    }

    private void OnUseHealPack()
    {
        EntityManager.Instance.MainPlayer?.HealHp(healHp);
        EntityManager.ReturnEntity(this);
    }
}
