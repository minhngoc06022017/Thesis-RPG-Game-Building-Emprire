using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject, IMoveable,IDescriable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
    }

    public string MyTitle
    {
        get
        {
            return title;
        }
    }

    private SlotScript slot;

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }
    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }
    public SlotScript MySlot
    {
        get
        {
            return slot;
        }
        set
        {
            slot = value;
        }
    }

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    public CharButton MyCharButton { get; set; }

    [SerializeField]
    private int price;

    public int MyPrice
    {
        get
        {
            return price;
        }
    }

    public virtual string GetDescription()
    {
        string color = string.Empty;

        return string.Format("<color={0}> {1} </color>",QualityColor.MyColors[quality],title);
    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
            
        }
    }

}
