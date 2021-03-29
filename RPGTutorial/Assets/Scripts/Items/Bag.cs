using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag",menuName ="Items/Bag",order =1)]
public class Bag : Item , IUseable
{
    [SerializeField]
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagButton MyBagButton { get; set; }

    public BagScript MyBagScript { get; set; }

    public int MySlotsCount
    {
        get
        {
            return slots;
        }
    }
    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScript.Instance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            if(MyBagButton == null)
            {
                InventoryScript.Instance.AddBag(this);
            }
            else
            {
                InventoryScript.Instance.AddBag(this, MyBagButton);
            }

            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n{0} slot bag", slots);
    }


    public void SetupScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }

}
