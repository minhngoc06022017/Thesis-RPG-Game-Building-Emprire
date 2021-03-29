using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }

    public List<ChestData> MyChestData { get; set; }

    public List<EquipmentData> MyEquipmentData { get; set; }

    public InventoryData MyInventoryData { get; set; }

    public List<ActionButtonData> MyActionButtonData { get; set; }

    public List<QuestData> MyQuestData { get; set; }

    public List<QuestGiverData> MyQuestGiverData { get; set; }

    public DateTime MyDateTime { get; set; }

    public string MyScene { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyChestData = new List<ChestData>();
        MyEquipmentData = new List<EquipmentData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now;
    }
}
[System.Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }

    public float MyXp { get; set; }

    public float MyMaxXp { get; set; }

    public float MyHealth { get; set; }

    public float MyMaxHealth { get; set; }

    public float MyMana { get; set; }

    public float MyMaxMana { get; set; }

    public float MyPositionX { get; set; }

    public float MyPositionY { get; set; }

    public PlayerData(int level, float xp , float maxXp , float health , float maxHealth , float mana , float maxMana , Vector2 position)
    {
        this.MyLevel = level;
        this.MyXp = xp;
        this.MyMaxXp = maxXp;
        this.MyHealth = health;
        this.MyMaxHealth = maxHealth;
        this.MyMana = mana;
        this.MyMaxMana = maxMana;
        this.MyPositionX = position.x;
        this.MyPositionY = position.y;
    }
}

[System.Serializable]
public class ItemData
{
    public string MyTitle { get; set; }

    public int MyStackCount { get; set; }

    public int MySlotIndex { get; set; }

    public int MyBagIndex { get; set; }

    public ItemData(string title, int stackCount = 0 ,int slotIndex = 0,int bagIndex =0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;
    }
}

[System.Serializable]
public class ChestData
{
    public string MyName { get; set; }

    public List<ItemData> MyItems { get; set; }

    public ChestData(string name)
    {
        MyName = name;

        MyItems = new List<ItemData>();
    }
}

[System.Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }

    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}

[System.Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; }

    public BagData(int count , int index)
    {
        MySlotCount = count;
        MyBagIndex = index; 
    }
}

[System.Serializable]
public class EquipmentData
{
    public string MyTitle { get; set; }

    public string MyType { get; set; }

    public EquipmentData(string title,string type)
    {
        MyTitle = title;
        MyType = type;
    }
}

[System.Serializable]
public class ActionButtonData
{
    public string MyAction { get; set; }

    public bool IsItem { get; set; }

    public int MyIndex { get; set; }

    public ActionButtonData(string action , bool isItem , int index)
    {
        MyAction = action;
        IsItem = isItem;
        MyIndex = index;
    }
    
}

[System.Serializable]
public class QuestData
{
    public string MyTitle { get; set; }

    public string MyDescription { get; set; }

    public CollectObjective[] MyCollectObjectives { get; set; }

    public KillObjective[] MyKillObjectives { get; set; }

    public int MyQuestGiverID { get; set; }

    public QuestData(string title , string description , CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        MyTitle = title;
        MyDescription = description;
        MyCollectObjectives = collectObjectives;
        MyKillObjectives = killObjectives;
        MyQuestGiverID = questGiverID;
    }
  
}

[System.Serializable]
public class QuestGiverData
{
    public List<string> MyCompleteQuests { get; set; }

    public int MyQuestGiverID { get; set; }

    public QuestGiverData(int questGiverId , List<string> completedQuests)
    {
        this.MyQuestGiverID = questGiverId;
        this.MyCompleteQuests = completedQuests;
    }
}


