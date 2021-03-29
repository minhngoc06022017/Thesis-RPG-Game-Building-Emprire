using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;
    public static LootWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LootWindow>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public IInteractable MyInteractable { get; set; }

    [SerializeField]
    private LootButton[] lootButtons;

    private List<List<Drop>> pages = new List<List<Drop>>();

    private List<Drop> droppedLoot = new List<Drop>();

    private CanvasGroup canvasGroup;

    [SerializeField]
    private Item[] items;

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextBtn, previousBtn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        //List<Item> tmp = new List<Item>();
        //for(int i =0;i< items.Length; i++)
        //{
        //    tmp.Add(items[i]);
        //}

        //CreatePages(tmp);
    }
    public void CreatePages(List<Drop> items)
    {
        if (!IsOpen)
        {
            List<Drop> page = new List<Drop>();

            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 4 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Drop>();
                }
            }

            addLoot();

            Open();
        }
    }
    private void addLoot()
    {

        if(pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousBtn.SetActive(pageIndex > 0);

            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if(pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyItem.MyIcon;

                    lootButtons[i].MyLoot = pages[pageIndex][i].MyItem;

                    lootButtons[i].gameObject.SetActive(true);

                    string title = string.Format("<color={0}> {1} </color>", QualityColor.MyColors[pages[pageIndex][i].MyItem.MyQuality], pages[pageIndex][i].MyItem.MyTitle);

                    lootButtons[i].MyTitle.text = title;
                }

            }
        }
    }

    public void ClearButton()
    {
        foreach(LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if(pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButton();
            addLoot();
        }
    }
    public void PreviousPage()
    {
        if(pageIndex > 0)
        {
            pageIndex--;
            ClearButton();
            addLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        Drop drop = pages[pageIndex].Find(x => x.MyItem == loot);

        pages[pageIndex].Remove(drop);

        drop.Remove();

        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if(pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            addLoot();
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        pageIndex = 0;
        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButton();

        if (MyInteractable != null)
        {
            MyInteractable.StopInteract();
        }

        MyInteractable = null;
    }
}
