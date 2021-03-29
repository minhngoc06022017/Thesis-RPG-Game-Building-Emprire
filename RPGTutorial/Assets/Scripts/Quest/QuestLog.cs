using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();
    public List<Quest> MyQuests
    {
        get
        {
            return quests;
        }
    }

    [SerializeField]
    private Transform questParent;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountTxt;

    [SerializeField]
    private int maxCount;

    [SerializeField]
    private int currentCount;

    private Quest selected;

    [SerializeField]
    private Text textDescription;

    private static QuestLog instance;
    public static QuestLog Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    // Update is called once per frame
    

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent += new ItemCountChange(o.UpdateItemCount);
                o.UpdateItemCount();
            }
            foreach (KillObjective o in quest.MyKillObjectives)
            {
                GameManager.Instance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }


            quests.Add(quest);


            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.MyTitle;

            CheckCompletion();
        }
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected!=quest)
            {
                selected.MyQuestScript.Deselect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ":" + obj.MyCurrentAmount + "/" + obj.MyAmount;
            }
            objectives += "\n";
            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ":" + obj.MyCurrentAmount + "/" + obj.MyAmount;
            }

            textDescription.text = string.Format("<b>{0}</b>\n{1}\n<b>\nObjectives\n</b>{2}", title, quest.MyDescription, objectives);
        }

    }

    public void CheckCompletion()
    {
        foreach(QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }
    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChange(o.UpdateItemCount);
            
        }
        foreach (KillObjective o in selected.MyKillObjectives)
        {
            GameManager.Instance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);
    }
    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);
        textDescription.text = string.Empty;
        selected = null;
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
