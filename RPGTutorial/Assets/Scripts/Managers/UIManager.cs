using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private RectTransform recTransformTooltip;

    private KeyCode action1, action2, action3;

    private GameObject[] keybindButtons;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    private Stat healthStat;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;

    [SerializeField]
    private Text tooltipText;

    [SerializeField]
    private CharacterPanel characterPanel;

    [SerializeField]
    private CanvasGroup[] menus;

    
    // Start is called before the first frame update

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenClose(menus[3]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenClose(menus[6]);
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    OpenClose(keybindMenu);
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OpenClose(spellBook);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    InventoryScript.Instance.OpenClose();
        //}


    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);

        
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        levelText.text = target.MyLevel.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if(target.MyLevel >= Player.Instance.MyLevel + 5)
        {
            levelText.color = Color.red;
        }
        else if(target.MyLevel == Player.Instance.MyLevel + 3 || target.MyLevel == Player.Instance.MyLevel + 4)
        {
            levelText.color = new Color32(255,124,0,255);
        }else if(target.MyLevel >= Player.Instance.MyLevel - 2 || target.MyLevel <= Player.Instance.MyLevel + 2)
        {
            levelText.color = Color.yellow;
        }else if(target.MyLevel <= Player.Instance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGrayLevel())
        {
            levelText.color = Color.green;
        }
        else
        {
            levelText.color = Color.grey;
        }
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
    

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }
    
    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach(CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.MyCount > 1)//If our slot has more than one item
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.black;
            clickable.MyIcon.color = Color.white;
        }
        else//If it has only 1 itemon it
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }

        if(clickable.MyCount == 0)//If the slot is empty
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }

    public void ShowToolTip(Vector2 pivot,Vector3 position, IDescriable description)
    {
        recTransformTooltip.pivot = pivot;
        toolTip.transform.position = position;
        toolTip.SetActive(true);
        tooltipText.text = description.GetDescription();
    }
    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    public void RefreshTooltip(IDescriable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
