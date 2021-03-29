using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour , IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    

    public IUseable MyUseable { get; set; }

    [SerializeField]
    private Text stackSize;

    public Button MyButton { get; private set; }

    private Stack<IUseable> useables = new Stack<IUseable>();

    public Stack<IUseable> MyUseables
    {
        get
        {
            return useables;
        }
        set
        {
            if(value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            useables = value;
        }
    }

    private int count;

    [SerializeField]
    private Image icon;

    public Image Icon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChange(UpdateItemCount);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.Instance.MyMoveable != null && HandScript.Instance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.Instance.MyMoveable as IUseable);
            }
        }
    }
    public void SetUseable(IUseable useable)
    {
        if(useable is Item)
        {
            MyUseables = InventoryScript.Instance.GetUseables(useable);
            if(InventoryScript.Instance.FromSlot != null)
            {
                InventoryScript.Instance.FromSlot.MyIcon.color = Color.white;
                InventoryScript.Instance.FromSlot = null;
            }
            
        }
        else
        {
            MyUseables.Clear();
            this.MyUseable = useable;
        }
        count = MyUseables.Count;
        UpdateVisual(useable as IMoveable);
        UIManager.Instance.RefreshTooltip(MyUseable as IDescriable);
    }
    public void OnClick()
    {
        if(HandScript.Instance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            else if(MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }
    }
    public void UpdateVisual(IMoveable moveable)
    {
        if(HandScript.Instance.MyMoveable != null)
        {
            HandScript.Instance.Drop();
        }

        Icon.sprite = moveable.MyIcon;
        Icon.color = Color.white;

        if(count > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
        else if(MyUseable is Spell)
        {
            UIManager.Instance.ClearStackCount(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if(item is IUseable && MyUseables.Count > 0)
        {
            if(MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.Instance.GetUseables(item as IUseable);

                count = MyUseables.Count;

                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescriable tmp = null;

        if (MyUseable!=null && MyUseable is IDescriable)
        {
            tmp = (IDescriable)MyUseable;
            
        }else if(MyUseables.Count > 0)
        {
            UIManager.Instance.HideToolTip();
        }
        if (tmp != null)
        {
            UIManager.Instance.ShowToolTip(new Vector2(1,0),transform.position, tmp);
        }
    }
}
