using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quality;

    private VendorItem vendorItem;
    
    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;

        if(vendorItem.MyQuality > 0 || (vendorItem.MyQuality == 0 && vendorItem.Unlimited))
        {
            icon.sprite = vendorItem.MyItem.MyIcon;
            title.text = string.Format("<color={0}> {1} </color>", QualityColor.MyColors[vendorItem.MyItem.MyQuality], vendorItem.MyItem.MyTitle);
            


            if (!vendorItem.Unlimited)
            {
                quality.text = vendorItem.MyQuality.ToString();
            }
            else
            {
                quality.text = string.Empty;
            }


            if(vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "Price: " + vendorItem.MyItem.MyPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }

            gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.Instance.MyGold >= vendorItem.MyItem.MyPrice) && InventoryScript.Instance.AddItem(Instantiate(vendorItem.MyItem)))
        {
            SellItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowToolTip(new Vector2(0, 0), transform.position, vendorItem.MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }

    private void SellItem()
    {
        Player.Instance.MyGold -= vendorItem.MyItem.MyPrice;

        if (!vendorItem.Unlimited)
        {
            vendorItem.MyQuality--;
            quality.text = vendorItem.MyQuality.ToString();
            
            if(vendorItem.MyQuality ==0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
