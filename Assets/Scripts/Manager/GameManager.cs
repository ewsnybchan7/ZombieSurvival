using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public EntityManager EntityManager;
    public UIManager UIManager;

    public int Score { get; set; }
    public int GameTime { get; set; }

    public bool IsGameOver { get; set; }

    private void Awake()
    {
        Score = 0;
        GameTime = 0;
        IsGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
