using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntityManager : Singleton<EntityManager>
{
    private Dictionary<long, BaseEntity> EntityTable;

    private Queue<ZombieEntity> ZombiePool;
    private Queue<ItemEntity> ItemPool;

    public PlayerEntity MainPlayer;

    public BaseEntity[] PatrolPoints;
    public GameObject Spawners;
    public EntitySpawner[] ZombieSpawners;
    public EntitySpawner PlayerSpawner;

    public GameObject PlayerPrefab;
    public GameObject ZombiePrefab;
    public GameObject Item1Prefab; 
    public GameObject Item2Prefab;
    
    public Action OnRemoveEntity_Event { get; set; }
    
    public enum EntityType
    {
        None,
        Zombie,
        Player,
        Item,
        Patrol,
    }

    public enum ItemType
    {
        None,
        Bullet,
        Heart,
    }
    protected override void Start()
    {
        base.Start();

        ZombiePool = new Queue<ZombieEntity>();
        ItemPool = new Queue<ItemEntity>();

        EntityTable = new Dictionary<long, BaseEntity>();
    }

    private BaseEntity CreateEntity(EntityType entitytype, long usn)
    {
        BaseEntity entity;

        switch(entitytype)
        {
            case EntityType.Zombie:
                entity = Instantiate(ZombiePrefab).GetComponent<ZombieEntity>();
                break;

            case EntityType.Player:
                entity = Instantiate(PlayerPrefab).GetComponent<PlayerEntity>();
                break;

            case EntityType.Item:
                BaseEntity itemPrefab = EntityTable[usn];
                entity = Instantiate(itemPrefab).GetComponent<ItemEntity>();
                break;

            default:
                entity = null;
                break;
        }

        entity?.gameObject.SetActive(false);
        entity?.transform.SetParent(this.transform);

        return entity;
    }

    public static void ReturnEntity(BaseEntity entity)
    {
        if (entity == null)
            return;

        switch (entity.EntityType)
        {
            case EntityType.Zombie:
                entity.gameObject.SetActive(false);
                entity.transform.SetParent(Instance.transform);
                Instance.ZombiePool.Enqueue((ZombieEntity)entity);

                GameManager.Instance.Score += 10; // score Á¤ÇÏ±â
                break;

            case EntityType.Player:
                entity.gameObject.SetActive(false);
                break;

            case EntityType.Item:
                entity.gameObject.SetActive(false);
                entity.transform.SetParent(Instance.transform);
                Instance.ItemPool.Enqueue((ItemEntity)entity);
                break;

            default:
                entity = null;
                break;
        }
    }

    public static ZombieEntity GetZombieEntity()
    {
        if(Instance.ZombiePool.Count > 0)
        {
            ZombieEntity zombie = Instance.ZombiePool.Dequeue();
            zombie.gameObject.SetActive(true);
            zombie.transform.SetParent(null);
            
            return zombie;
        }
        else
        {
            ZombieEntity zombie = (ZombieEntity)Instance.CreateEntity(EntityType.Zombie, 2000);
            zombie?.transform.SetParent(null);
            zombie?.gameObject.SetActive(true);
            
            return zombie;
        }
    }

    public static PlayerEntity GetPlayerEntity()
    {
        if(Instance.MainPlayer == null)
        {
            Instance.MainPlayer = (PlayerEntity)Instance.CreateEntity(EntityType.Player, 1000);
        }

        return Instance.MainPlayer;
    }

    public static ItemEntity GetItemEntity(ItemType itemtype)
    {
        if (Instance.ItemPool.Count > 0)
        {
            ItemEntity item = Instance.ItemPool.Dequeue();
            item.transform.SetParent(null);
            item.gameObject.SetActive(true);

            return item;
        }
        else
        {
            ItemEntity item = (ItemEntity)Instance.CreateEntity(EntityType.Item, 3000 + (int)itemtype);
            item?.transform.SetParent(null);
            item?.gameObject.SetActive(true);

            return item;
        }
    }
}
