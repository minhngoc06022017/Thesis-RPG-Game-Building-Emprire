using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;

    public string MyTitle
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }

    [SerializeField]
    private int level;
    public int MyLevel
    {
        get
        {
            return level;
        }
    }

    [SerializeField]
    private int xp;
    public int MyXp 
    {
        get
        {
            return xp;
        }
    }

    [SerializeField]
    private CollectObjective[] collectObjectives;
    public CollectObjective[] MyCollectObjectives
    {
        get
        {
            return collectObjectives;
        }
    }

    [SerializeField]
    private KillObjective[] killObjectives;
    public KillObjective[] MyKillObjectives
    {
        get
        {
            return killObjectives;
        }
        set
        {
            killObjectives = value;
        }
    }

    public bool IsComplete
    {
        get
        {
            foreach(Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            foreach (Objective o in killObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }

    [SerializeField]
    private string description;
    public string MyDescription
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

    
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
        }
    }
    public int MyCurrentAmount
    {
        get
        {
            return currentAmount;
        }
        set
        {
            currentAmount = value;
        }
    }
    public string MyType
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.Instance.GetItemCount(item.MyTitle);

            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}:{1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }

            QuestLog.Instance.UpdateSelected();
            QuestLog.Instance.CheckCompletion();
            
        }
    }
    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.Instance.GetItemCount(MyType);
        QuestLog.Instance.UpdateSelected();
        QuestLog.Instance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.Instance.GetItems(MyType, MyAmount);

        foreach(Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if(MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}:{1}/{2}", character.MyType, MyCurrentAmount, MyAmount));
                QuestLog.Instance.UpdateSelected();
                QuestLog.Instance.CheckCompletion();
            }
        }
    }

}
