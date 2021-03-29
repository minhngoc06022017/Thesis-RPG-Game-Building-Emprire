using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HealthPotion",menuName ="Items/Potion",order =1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;

    public void Use()
    {
        if(Player.Instance.MyHealth.MyCurrentValue < Player.Instance.MyHealth.MyMaxValue)
        {
            Remove();

            Player.Instance.GetHealth(health);
        }

    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff> Use: Restores {0} health </color>",health);
    }
}
