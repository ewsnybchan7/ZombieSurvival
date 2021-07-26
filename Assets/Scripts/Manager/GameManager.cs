using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Score { get; set; }
    public int GameTime { get; set; }
    public bool IsGameOver { get; set; }

    private bool IsStart = true;

    private void Awake()
    {
        Score = 0;
        GameTime = 0;
        IsGameOver = false;
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameOver)
        {
            UIManager.Instance.GameUICanvas.SetActive(false);
            UIManager.Instance.GameOverCanvas.SetActive(true);
            foreach(EntitySpawner spawner in EntityManager.Instance.ZombieSpawners)
            {
                spawner.CanSpawn = false;
            }
        }
    }

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2.0f);

        UIManager.Instance.GameUICanvas.SetActive(true);
        UIManager.Instance.GameOverCanvas.SetActive(false);
        EntityManager.Instance.Spawners.SetActive(true);

        yield return new WaitUntil(() => EntityManager.Instance.MainPlayer);

        UIManager.InitUI();
    }

    public void GameRestart()
    {
        UIManager.Instance.GameOverCanvas.SetActive(false);
        
        foreach(EntitySpawner spawner in EntityManager.Instance.ZombieSpawners)
        {
            foreach(BaseEntity entity in spawner.EntityList)
            {
                EntityManager.ReturnEntity(entity);
            }
            spawner.EntityList.Clear();
        }

        EntityManager.Instance.Spawners.SetActive(false);

        Score = 0;
        GameTime = 0;
        IsGameOver = false;

        StartCoroutine(GameStart());
    }
}
