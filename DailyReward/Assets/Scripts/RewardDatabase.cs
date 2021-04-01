using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName ="RewardDB" , menuName = "Time rewards System/Reward Database")]
public class RewardDatabase : ScriptableObject
{
    public Reward[] rewards;

    public int rewardsCount
    {
        get
        {
            return rewards.Length;
        }
    }

    public Reward getReward(int index)
    {
        return rewards[index];
    }
}
