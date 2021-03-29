using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour,IPointerClickHandler,IClickable,IPointerEnterHandler,IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Text stackSize;

    
    public BagScript MyBag { get; set; }

    
    public int MyIndex { get; set; }

    public bool IsEmty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmty || MyCount < MyItem.StackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmty)
            {
                return items.Peek();
            }

            return null;
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
        get { return items.Count; }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
        set
        {
            items = value;
        }
    }

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    [SerializeField]
    private Image icon;

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.Icon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if(IsEmty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for(int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveItem(Item item)
    {
        if (!IsEmty)
        {
           
            InventoryScript.Instance.OnItemCountChanged(MyItems.Pop());

        }
    }

    public void Clear()
    {
        int initCount = MyItems.Count;

        if(initCount > 0)
        {
            for(int i = 0; i < initCount; i++)
            {
                InventoryScript.Instance.OnItemCountChanged(MyItems.Pop());
            }
            
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot == null && !IsEmty)//If we don't have something to move
            {
                if (HandScript.Instance.MyMoveable !=null)
                {
                    if(HandScript.Instance.MyMoveable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.Instance.SwapBags(HandScript.Instance.MyMoveable as Bag, MyItem as Bag);
                        }
                    }else if(HandScript.Instance.MyMoveable is Armor)
                    {
                        if(MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.Instance.MyMoveable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Eqiup();
                            
                            HandScript.Instance.Drop();
                        }
                    }
                    
                }
                else
                {
                    HandScript.Instance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.Instance.FromSlot = this;
                }
            }
            else if (InventoryScript.Instance.FromSlot == null && IsEmty)
            {
                if(HandScript.Instance.MyMoveable is Bag)
                {
                    Bag bag = (Bag)HandScript.Instance.MyMoveable;

                    if (bag.MyBagScript != MyBag && InventoryScript.Instance.MyEmptySlotCount - bag.MySlotsCount > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        HandScript.Instance.Drop();
                    }
                }
                else if(HandScript.Instance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.Instance.MyMoveable;
                    AddItem(armor);
                    CharacterPanel.Instance.MySelectedButton.DequipArmor();
                    HandScript.Instance.Drop();
                    
                }
                
            }
            else if(InventoryScript.Instance.FromSlot != null)//if we have something to move
            {
                if (PutItemBack() || MergeItems(InventoryScript.Instance.FromSlot) || SwapItems(InventoryScript.Instance.FromSlot) || AddItems(InventoryScript.Instance.FromSlot.items))
                {
                    HandScript.Instance.Drop();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
            
        }
        if(eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.MyMoveable == null)
        {
            UseItem();
        }
    }
    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }else if(MyItem is Armor)
        {
            (MyItem as Armor).Eqiup();
        }
    }
    public bool StackItem(Item item)
    {
        if (!IsEmty && item.name == MyItem.name && items.Count < MyItem.StackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }
    public bool PutItemBack()
    {
        if (InventoryScript.Instance.FromSlot == this)
        {
            InventoryScript.Instance.FromSlot.MyIcon.color = Color.white;
            return true;
        }
        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmty)
        {
            return false;
        }

        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.StackSize)
        {
            //Copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

            //Clear slot A
            from.items.Clear();
            //All items from slot b and copy them to A
            from.AddItems(items);

            //Clear slot b
            items.Clear();

            //Move items from a copy to b
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmty)
        {
            return false;
        }
        if(from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //How many free slots we have in the stack
            int free = MyItem.StackSize - MyCount;

            for(int i = 0; i < free; i++)
            {
                AddItem(from.items.Pop());
            }

            return true;
        }
        return false;
    }

    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmty)
        {
            UIManager.Instance.ShowToolTip(new Vector2(1, 0),transform.position, MyItem); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }
}
