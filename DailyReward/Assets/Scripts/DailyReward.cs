using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    private static DailyReward instance;
    public static DailyReward Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DailyReward>();
            }
            return instance;
        }
    }

    [SerializeField]
    private RewardDatabase rewardsDB;

    [SerializeField]
    private Text metalText;

    [SerializeField]
    private Text coinText;

    [SerializeField]
    private Text gemText;


    [SerializeField] Text rewardAmountText;
    [SerializeField] Button claimButton;
    [SerializeField] GameObject notificationRewards;
    [SerializeField] GameObject noMoreRewardCanvas;
    [SerializeField] GameObject getRewardCanvas;

    private int nextIndex;
    private bool isRewardReady = false;

    [SerializeField] GameObject rewardsCanvas;

    [SerializeField] double nextRewardDelay = 20f;
    [SerializeField] float checkForRewardDelay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        StartCoroutine(CheckForReward());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        nextIndex = PlayerPrefs.GetInt("Index_Reward", 0);

        UpdateMetalTextUI();
        UpdateCoinTextUI();
        UpdateGemTextUI();

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime")))
        {
            PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
        }
    }

    IEnumerator CheckForReward()
    {
        while (true)
        {
            if (!isRewardReady)
            {
                DateTime currentDatetime = DateTime.Now;
                DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));

                double elapsedSeconds = (currentDatetime - rewardClaimDatetime).TotalSeconds;

                if (elapsedSeconds >= nextRewardDelay)
                {
                    ActivateReward();
                }
                else
                {
                    DesactivateReward();
                }
            }

            yield return new WaitForSeconds(checkForRewardDelay);
        }
        
    }

    void UpdateMetalTextUI()
    {
        metalText.text = Player.Instance.Metals.ToString();
    }
    void UpdateCoinTextUI()
    {
        coinText.text = Player.Instance.Coins.ToString();
    }
    void UpdateGemTextUI()
    {
        gemText.text = Player.Instance.Gems.ToString();
    }

    public void OnOpenButtonClick()
    {
        rewardsCanvas.SetActive(true);
    }
    public void OnCloseButtonClick()
    {
        rewardsCanvas.SetActive(false);
    }
    public void OnClaimButtonClick()
    {
        Reward reward = rewardsDB.getReward(nextIndex);

        if(reward.Type == RewardType.Metals)
        {
            Player.Instance.Metals += reward.amount;
            UpdateMetalTextUI();
        }
        else if (reward.Type == RewardType.Coins)
        {
            Player.Instance.Coins += reward.amount;
            UpdateCoinTextUI();
        }
        else if (reward.Type == RewardType.Gems)
        {
            Player.Instance.Gems += reward.amount;
            UpdateGemTextUI();
        }

        nextIndex++;
        if(nextIndex >= rewardsDB.rewardsCount)
        {
            nextIndex = 0;
        }
        PlayerPrefs.SetInt("Index_Reward", nextIndex);

        PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());

        DesactivateReward();
    }

    public void ActivateReward()
    {
        isRewardReady = true;

        notificationRewards.SetActive(true);
        noMoreRewardCanvas.SetActive(false);
        getRewardCanvas.SetActive(true);

        Reward reward = rewardsDB.getReward(nextIndex);
        rewardAmountText.text = reward.amount.ToString();
    }
    public void DesactivateReward()
    {
        isRewardReady = false;

        notificationRewards.SetActive(false);
        noMoreRewardCanvas.SetActive(true);
        getRewardCanvas.SetActive(false);
    }
}

public enum RewardType
{
    Metals,
    Coins,
    Gems
}

[System.Serializable]
public struct Reward
{
    public RewardType Type;
    public int amount;
}
