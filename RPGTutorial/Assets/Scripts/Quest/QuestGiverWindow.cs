using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;
    public static QuestGiverWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject backBtn, acceptBtn,completeBtn,questDescription;

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    [SerializeField]
    private Transform questArea;

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach(GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                go.GetComponent<Text>().text ="["+quest.MyLevel+"] "+quest.MyTitle;
                
                go.GetComponent<QGQuestScript>().MyQuest = quest;
                
                quests.Add(go);

                if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text += "(C)";
                }
                else if (QuestLog.Instance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;

                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                }
            }
            
        }
    }
    public override void Open(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if(!QuestLog.Instance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ":" + obj.MyCurrentAmount + "/" + obj.MyAmount;
        }

        questDescription.GetComponent<Text>().text = string.Format("<b>{0}</b>\n{1}", quest.MyTitle, quest.MyDescription);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accpet()
    {
        QuestLog.Instance.AcceptQuest(selectedQuest);
        Back();
    }
    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for(int i=0;i< questGiver.MyQuests.Length; i++)
            {
                if(selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }

            foreach(CollectObjective o in selectedQuest.MyCollectObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChange(o.UpdateItemCount);
                o.Complete();
            }
            foreach (KillObjective o in selectedQuest.MyKillObjectives)
            {
                GameManager.Instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
            }

            Player.Instance.GainXP(XPManager.CalculateXP(selectedQuest));

            QuestLog.Instance.RemoveQuest(selectedQuest.MyQuestScript);

            Back();
        }
    }
}
