using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour,IPointerClickHandler
{

    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    [SerializeField]
    private int bagIndex;

    public int MyBagIndex
    {
        get
        {
            return bagIndex;
        }
    }

    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.Instance.FromSlot!=null && HandScript.Instance.MyMoveable!=null && HandScript.Instance.MyMoveable is Bag)
            {
                if(MyBag != null)
                {
                    InventoryScript.Instance.SwapBags(MyBag, HandScript.Instance.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.Instance.MyMoveable;
                    tmp.MyBagButton = this;
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.Instance.Drop();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.Instance.TakeMoveable(MyBag);
            }
            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }

        
    }

    public void RemoveBag()
    {
        InventoryScript.Instance.RemoveBag(MyBag);
        MyBag.MyBagButton = null;

        foreach(Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScript.Instance.AddItem(item);
        }

        MyBag = null;
    }
}
