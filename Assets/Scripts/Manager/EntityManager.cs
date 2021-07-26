using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntityManager : Singleton<EntityManager>
{
    private Dictionary<long, GameObject> ItemTable;

    private Queue<ZombieEntity> ZombiePool;
    private Queue<ItemEntity> ItemPool;

    public PlayerEntity MainPlayer;

    public BaseEntity Patrols;
    public BaseEntity[] PatrolPoints { get; private set; }
    public GameObject Spawners;
    public List<EntitySpawner> ZombieSpawners { get; private set; }
    public EntitySpawner PlayerSpawner;

    public GameObject PlayerPrefab;
    public GameObject ZombiePrefab;
    public GameObject HealPackPrefab; 
    public GameObject InfinityAmmoPrefab;
    public GameObject ItemGunPrefab;
    
    public Action OnRemoveEntity_Event { get; set; }
    
    public enum EntityType
    {
        None,
        Zombie,
        Player,
        Item,
        Patrol,
        End
    }

    public enum ItemType
    {
        None,
        Infinity,
        Heart,
        Gun,
        End
    }
    protected override void Start()
    {
        base.Start();

        PatrolPoints = Patrols.GetComponentsInChildren<BaseEntity>();
        ZombieSpawners = ZombieSpawners.


        ZombiePool = new Queue<ZombieEntity>();
        ItemPool = new Queue<ItemEntity>();

        ItemTable = new Dictionary<long, GameObject>();

        ItemTable.Add(3001, InfinityAmmoPrefab);
        ItemTable.Add(3002, HealPackPrefab);
        //ItemTable.Add(3003, ItemGunPrefab);
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
                entity = Instantiate(ItemTable[usn]).GetComponent<ItemEntity>();
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

                GameManager.Instance.Score += 10; // score 정하기
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


    // item code로 바꾸기
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
