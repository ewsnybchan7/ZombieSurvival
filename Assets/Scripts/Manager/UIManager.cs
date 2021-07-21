using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    PlayerEntity MainPlayer;

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

    private void Start()
    {
        UpdateHpText(100, 100);
        UpdateGunText("Uzi");
        UpdateAmmoText(15, 15);
        UpdateScoretText(0);
    }

    private void Update()
    {
        if(!MainPlayer)
        {
            MainPlayer = EntityManager.Instance.MainPlayer;
        }
    }


    public void UpdateHpText(float currentHP, float maxHP)
    {
        m_PlayerHP.text = currentHP + " / " + maxHP;
    }

    public void UpdateGunText(string gunName)
    {
        m_PlayerGun.text = gunName;
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        m_PlayerAmmo.text = "Ammo : " + currentAmmo + " / " + maxAmmo;
    }

    public void UpdateWaveText()
    {

    }

    public void UpdateTimeText()
    {

    }

    public void UpdateScoretText(int curScore)
    {
        m_GameScore.text = curScore.ToString();
    }
}
