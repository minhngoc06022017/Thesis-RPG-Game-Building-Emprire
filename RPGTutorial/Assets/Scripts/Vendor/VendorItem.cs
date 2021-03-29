using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VendorItem 
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private int quality;

    [SerializeField]
    private bool unlimited;

    public Item MyItem
    {
        get
        {
            return item;
        }
    }
    public int MyQuality
    {
        get
        {
            return quality;
        }
        set
        {
            quality = value;
        }
    }
    public bool Unlimited
    {
        get
        {
            return unlimited;
        }
    }
}
