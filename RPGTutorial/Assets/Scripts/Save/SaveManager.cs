using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private Item[] items;

    private Chest[] chests;

    private CharButton[] equipment;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private SaveGame[] saveSlots;

    [SerializeField]
    private GameObject dialogue;

    [SerializeField]
    private Text dialogueText;

    private string action;

    private SaveGame current;

    private void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipment = FindObjectsOfType<CharButton>();

        foreach(SaveGame save in saveSlots)
        {
            ShowSavedFiles(save);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
          
            PlayerPrefs.DeleteKey("Load");
           
        }
        else
        {
            Player.Instance.SetDefaults();
       
        }
    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;

        switch (action)
        {
            case "Load":
                dialogueText.text = "Load game?";
                break;
            case "Save":
                dialogueText.text = "Save game?";
                break;
            case "Delete":
                dialogueText.text = "Delete savefile?";
                break;
        }

        current = clickButton.GetComponentInParent<SaveGame>();
        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                break;
            case "Save":
                Save(current);
                break;
            case "Delete":
                Delete(current);
                break;
        }

        CloseDialogue();
    }

    public void LoadScene(SaveGame saveGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", saveGame.MyIndex);
            SceneManager.LoadScene(data.MyScene);
            
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void Delete(SaveGame saveGame)
    {
        File.Delete(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat");
        saveGame.HideVisuals();
    }

    private void ShowSavedFiles(SaveGame saveGame)
    {
        if(File.Exists(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            saveGame.ShowInfo(data);
            
        }
    }

    public void Save(SaveGame saveGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGame.gameObject.name +".dat",FileMode.Create);

            SaveData data = new SaveData();

            data.MyScene = SceneManager.GetActiveScene().name;

            SaveEquipments(data);

            SaveBags(data);

            SaveInventory(data);

            SavePlayer(data);

            SaveChests(data);

            SaveActionButton(data);

            SaveQuests(data);

            SaveQuestGivers(data);

            bf.Serialize(file, data);

            file.Close();

            ShowSavedFiles(saveGame);

            Debug.Log("Success");
        }
        catch(System.Exception)
        {
            Delete(saveGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }
    private void Load(SaveGame saveGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadEquipments(data);

            LoadBags(data);

            LoadInventory(data);

            LoadPlayer(data);

            LoadChests(data);

            LoadActionButton(data);

            LoadQuests(data);

            LoadQuestGiver(data);
     
        }
        catch (System.Exception)
        {
            Delete(saveGame);
            PlayerPrefs.DeleteKey("Load");
            SceneManager.LoadScene(0);
        }
    }



    //SAVE INFORMATION TO THE FILE
    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.Instance.MyLevel,
            Player.Instance.MyXpStat.MyCurrentValue,
            Player.Instance.MyXpStat.MyMaxValue,
            Player.Instance.MyHealth.MyCurrentValue,
            Player.Instance.MyHealth.MyMaxValue,
            Player.Instance.MyMana.MyCurrentValue,
            Player.Instance.MyMana.MyMaxValue,
            Player.Instance.transform.position);
    }
    private void SaveChests(SaveData data)
    {
        for(int i = 0; i < chests.Length; i++)
        {
            data.MyChestData.Add(new ChestData(chests[i].name));

            foreach(Item item in chests[i].MyItems)
            {
                if(chests[i].MyItems.Count > 0)
                {
                    data.MyChestData[i].MyItems.Add(new ItemData(item.MyTitle, item.MySlot.MyItems.Count, item.MySlot.MyIndex));
                }
            }
        }
    }
    private void SaveBags(SaveData data)
    {
        for(int i = 1;i < InventoryScript.Instance.MyBags.Count; i++)
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScript.Instance.MyBags[i].MySlotsCount,
                InventoryScript.Instance.MyBags[i].MyBagButton.MyBagIndex));
        }
    }
    private void SaveEquipments(SaveData data)
    {
        foreach(CharButton charButton in equipment)
        {
            if (charButton.MyEquippedArmor != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charButton.MyEquippedArmor.MyTitle, charButton.name));
            }
        }
    }
    private void SaveActionButton(SaveData data)
    {
        for(int i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUseable != null)
            {
                ActionButtonData action;

                if (actionButtons[i].MyUseable is Spell)
                {
                     action = new ActionButtonData((actionButtons[i].MyUseable as Spell).MyTitle, false, i);
                }
                else
                {
                     action = new ActionButtonData((actionButtons[i].MyUseable as Item).MyTitle, true, i);
                }

                data.MyActionButtonData.Add(action);
            }
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.Instance.GetAllItems();

        foreach(SlotScript slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach(Quest quest in QuestLog.Instance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives,quest.MyQuestGiver.QuestGiverID));
        }
    }
    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach(QuestGiver questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.QuestGiverID, questGiver.MyCompletedQuests));
        }
    }


    //LOAD INFORMATION FROM THE FILE
    private void LoadPlayer(SaveData data)
    {
        Player.Instance.MyLevel = data.MyPlayerData.MyLevel;
        Player.Instance.UpdateLevel();
        Player.Instance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.Instance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.Instance.MyXpStat.Initialize(data.MyPlayerData.MyXp, data.MyPlayerData.MyMaxXp);
        Player.Instance.transform.position = new Vector2(data.MyPlayerData.MyPositionX, data.MyPlayerData.MyPositionY); 
    }
    private void LoadChests(SaveData data)
    {
        foreach(ChestData chest in data.MyChestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.MyName);

            foreach(ItemData itemData in chest.MyItems)
            {
                Item item =Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));
                item.MySlot = c.MyBag.MySlots.Find(x => x.MyIndex == itemData.MySlotIndex);
                c.MyItems.Add(item);
            }

        }
    }
    private void LoadBags(SaveData data)
    {
        foreach(BagData bagData in data.MyInventoryData.MyBags)
        {
            Bag newBag = (Bag)Instantiate(items[0]);

            newBag.Initialize(bagData.MySlotCount);

            InventoryScript.Instance.AddBag(newBag, bagData.MyBagIndex);
        }
    }
    private void LoadEquipments(SaveData data)
    {
        foreach(EquipmentData equipmentData in data.MyEquipmentData)
        {
            CharButton cb = Array.Find(equipment, x => x.name == equipmentData.MyType);

            cb.EquipArmor(Array.Find(items, x => x.MyTitle == equipmentData.MyTitle) as Armor);
        }
    }

    private void LoadActionButton(SaveData data)
    {
        foreach(ActionButtonData buttonData in data.MyActionButtonData)
        {
            if (buttonData.IsItem)
            {
                actionButtons[buttonData.MyIndex].SetUseable(InventoryScript.Instance.GetUseable(buttonData.MyAction));
            }
            else
            {
                actionButtons[buttonData.MyIndex].SetUseable(SpellBook.Instance.GetSpell(buttonData.MyAction));
            }
        }

    
    }

    private void LoadInventory(SaveData data)
    {
        foreach(ItemData itemData in data.MyInventoryData.MyItems)
        {
            Item item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));

            for(int i=0; i< itemData.MyStackCount; i++)
            {
                InventoryScript.Instance.PlaceInSpecific(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }
    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach(QuestData questData in data.MyQuestData)
        {
            QuestGiver qg = Array.Find(questGivers, x => x.QuestGiverID == questData.MyQuestGiverID);
            Quest q = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyTitle);
            q.MyQuestGiver = qg;
            q.MyKillObjectives = questData.MyKillObjectives;
            QuestLog.Instance.AcceptQuest(q);
        }
    }
    private void LoadQuestGiver(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();

        foreach(QuestGiverData questGiverData in data.MyQuestGiverData)
        {
            QuestGiver questGiver = Array.Find(questGivers, x => x.QuestGiverID == questGiverData.MyQuestGiverID);
            questGiver.MyCompletedQuests = questGiverData.MyCompleteQuests;
            questGiver.UpdateQuestStatus();
        }
    }
}
