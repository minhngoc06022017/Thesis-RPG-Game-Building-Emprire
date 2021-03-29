using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    private List<SlotScript> slots = new List<SlotScript>();

    public int MyBagIndex { get; set; }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach(SlotScript slot in MySlots)
            {
                if (slot.IsEmty)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();
        foreach(SlotScript slot in slots)
        {
            if (!slot.IsEmty)
            {
                foreach(Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }

        return items;
    }

    public void AddSlots(int slotCount)
    {
        for(int i = 0; i < slotCount; i++)
        {
            SlotScript slot= Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyIndex = i;
            slot.MyBag = this;
            slots.Add(slot);
        }
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public bool AddItem(Item item)
    {
        foreach(SlotScript slot in slots)
        {
            if (slot.IsEmty)
            {
                slot.AddItem(item);

                return true;
            }
        }

        return false;
    }
    public void Clear()
    {
        foreach(SlotScript slot in slots)
        {
            slot.Clear();   
        }
    }
}
