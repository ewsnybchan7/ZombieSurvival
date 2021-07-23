using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStopWatch : ItemEntity
{
    public float healHp = 10f;

    protected override void ItemSetUpOp()
    {
        EntityType = EntityManager.EntityType.Item;

        ItemCode = 3000;

        ItemOperation += OnUseHealPack;
    }

    private void OnUseHealPack()
    {
        EntityManager.Instance.MainPlayer?.HealHp(healHp);
    }
}
