using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text manaText;
    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;
    public int MyIndex
    {
        get
        {
            return index;
        }
    }

    private void Awake()
    {
        //visuals.SetActive(false);
    }

    public void ShowInfo(SaveData data)
    {
        visuals.SetActive(true);

        dateTime.text = "Date: " + data.MyDateTime.ToString("dd/MM/yyy") + " - Time: " + data.MyDateTime.ToString("H:mm");

        health.fillAmount = data.MyPlayerData.MyHealth / data.MyPlayerData.MyMaxHealth;
        healthText.text = data.MyPlayerData.MyHealth + "/" + data.MyPlayerData.MyMaxHealth;

        mana.fillAmount = data.MyPlayerData.MyMana / data.MyPlayerData.MyMaxMana;
        manaText.text= data.MyPlayerData.MyMana +"/"+ data.MyPlayerData.MyMaxMana;

        xp.fillAmount = data.MyPlayerData.MyXp / data.MyPlayerData.MyMaxXp;
        xpText.text = data.MyPlayerData.MyXp +"/"+ data.MyPlayerData.MyMaxXp;

        levelText.text = data.MyPlayerData.MyLevel.ToString();

        

    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }

}
