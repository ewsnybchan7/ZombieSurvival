using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    static PlayerEntity MainPlayer;

    public delegate void UpdateUI();
    public UpdateUI UpdateStatusFunc;

    public GameObject GameUICanvas;

    public GameObject m_PlayerStatus;
    public Text m_PlayerHP;
    public Text m_PlayerGun;
    public Text m_PlayerAmmo;

    public GameObject m_GameStatus;
    public Text m_GameTime;
    public Text m_GameScore;

    public GameObject m_MenuUI;

    public GameObject GameOverCanvas;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(!MainPlayer)
        {
            MainPlayer = EntityManager.Instance.MainPlayer;
        }
    }

    public static void InitUI()
    {
        MainPlayer = EntityManager.Instance.MainPlayer;

        UpdateHpText(MainPlayer.CurrentHp, MainPlayer.MaxHp);
        UpdateGunText(MainPlayer.m_Gun.Name);
        UpdateAmmoText(MainPlayer.m_Gun.CurAmmo, MainPlayer.m_Gun.MaxAmmo);
        UpdateScoretText();
    }

    public static void UpdateHpText(float currentHP, float maxHP)
    {
        Instance.m_PlayerHP.text = currentHP + " / " + maxHP;
    }

    public static void UpdateGunText(string gunName)
    {
        Instance.m_PlayerGun.text = gunName;
    }

    public static void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        Instance.m_PlayerAmmo.text = "Ammo : " + currentAmmo + " / " + maxAmmo;
    }

    public static void UpdateWaveText()
    {

    }

    public static void UpdateTimeText()
    {

    }

    public static void UpdateScoretText()
    {
        Instance.m_GameScore.text = GameManager.Instance.Score.ToString();
    }
}
