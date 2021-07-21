using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntitySpawner : MonoBehaviour
{
    public EntityManager.EntityType Entity;

    public bool IsRandomSpawn;
    public bool IsMultiSpawn;
    public float FixedSpawnCoolTime;

    public int nEntity = 0;

    private void NextSpawnCoolTime() {
        SpawnCoolTime = IsRandomSpawn ? Random.Range(15, 30) : FixedSpawnCoolTime;
    }

    [SerializeField]
    private float SpawnCoolTime;
    
    [SerializeField]
    private float elapsedTime = 0f;

    private void Start()
    {
        NextSpawnCoolTime();
    }

    // Update is called once per frame
    void Update()
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
    }

    private void SpawnZombie()
    {
        ZombieEntity zombie = EntityManager.GetZombieEntity();
        zombie.transform.SetParent(this.transform);
        zombie.transform.localPosition = Vector3.zero;
        elapsedTime = 0f;
        NextSpawnCoolTime();
    }

    private void SpawnPlayer()
    {
        PlayerEntity player = EntityManager.GetPlayerEntity();

        if (nEntity == 1)
            return;

        player.transform.position = this.transform.position;
        player.gameObject.SetActive(true);
        player.transform.SetParent(null);

        nEntity++;
        elapsedTime = 0f;
    }

    private void SpawnItem()
    {

    }
}