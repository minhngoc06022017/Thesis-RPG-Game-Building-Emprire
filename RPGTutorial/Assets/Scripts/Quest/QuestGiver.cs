using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSilver, exclemation;

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private SpriteRenderer miniMapIcon;

    [SerializeField]
    private int questGiverID;

    public int QuestGiverID
    {
        get
        {
            return questGiverID;
        }
    }

    private List<string> completedQuests = new List<string>();
    public List<string> MyCompletedQuests
    {
        get
        {
            return completedQuests;
        }
        set
        {
            completedQuests = value;

            foreach(string title in completedQuests)
            {
                for(int i = 0; i < quests.Length; i++)
                {
                    if(quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    public Quest[] MyQuests
    {
        get
        {
            return quests;
        }
    }

    private void Start()
    {
        foreach(Quest quest in quests)
        {
            quest.MyQuestGiver = this;
        }
        UpdateQuestStatus();
    }

    public void UpdateQuestStatus()
    {
        foreach(Quest quest in quests)
        {
            int count = 0;

            if(quest != null)
            {
                if(quest.IsComplete && QuestLog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    miniMapIcon.sprite = question;
                    break;
                }else if (!QuestLog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclemation;
                    miniMapIcon.sprite = exclemation;
                    break; 
                }else if(!quest.IsComplete && QuestLog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    miniMapIcon.sprite = questionSilver;
                    break;
                }

                Debug.Log("");
            }
            else
            {
                count++;

                if(count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    miniMapIcon.enabled = false;
                }
            }
        }
    }
}
