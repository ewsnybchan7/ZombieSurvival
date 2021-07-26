using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class EntitySpawner : MonoBehaviour
{
    public EntityManager.EntityType Entity;

    public List<BaseEntity> EntityList;

    public bool IsRandomSpawn = false;
    public bool IsMultiSpawn = false;
    public float FixedSpawnCoolTime;

    public int nEntity;

    public bool CanSpawn = true;

    private void NextSpawnCoolTime() {
        SpawnCoolTime = IsRandomSpawn ? Random.Range(15, 30) : FixedSpawnCoolTime;
    }

    [SerializeField]
    private float SpawnCoolTime;
    
    [SerializeField]
    private float elapsedTime = 0f;

    private void OnEnable()
    {
        nEntity = 0;
        CanSpawn = true;
        NextSpawnCoolTime();
    }

    private void Start()
    {
        EntityList = new List<BaseEntity>();
        nEntity = 0;
        CanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawn)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < SpawnCoolTime)
                return;

            switch (Entity)
            {
                case EntityManager.EntityType.Zombie:
                    SpawnZombie();
                    break;
                case EntityManager.EntityType.Player:
                    SpawnPlayer();
                    break;

                case EntityManager.EntityType.Item:
                    SpawnItem();
                    break;

                default:
                    break;

            }

            elapsedTime = 0f;
        }
    }

    private void SpawnZombie()
    {
        ZombieEntity zombie = EntityManager.GetZombieEntity();
        zombie.transform.SetParent(this.transform);
        zombie.transform.localPosition = Vector3.zero;
        EntityList.Add(zombie);
        nEntity++;

        NextSpawnCoolTime();
    }

    private void SpawnPlayer()
    {
        PlayerEntity player = EntityManager.GetPlayerEntity();

        if (nEntity == 1)
            return;

        player.transform.position = this.transform.position;
        player.gameObject.SetActive(true);
        player.transform.SetParent(this.transform);

        EntityList.Add(player);
        nEntity++;
    }

    public void SpawnItem()
    {
        ItemEntity item = EntityManager.GetItemEntity((EntityManager.ItemType)Random.Range(1, (int)EntityManager.ItemType.Gun));

        Vector3 point = NavMeshUtil.GetRandomPoint(this.transform.position, 20f);

        item.transform.position = point;
        item.gameObject.SetActive(true);
        item.transform.SetParent(this.transform);

        EntityList.Add(item);
        nEntity++;
    }
}