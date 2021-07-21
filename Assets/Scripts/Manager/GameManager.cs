using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public EntityManager EntityManager;
    public UIManager UIManager;

    public int Score { get; set; }
    public int GameTime { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        GameTime = 0;
    }

    private void FixedUpdate()
    {
        UIManager.Instance.UpdateScoretText(Score);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
