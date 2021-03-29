using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closeSprite;

    private bool isOpen;

    [SerializeField]
    private BagScript bag;
    public BagScript MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            bag = value;
        }
    }

    private List<Item> items;
    public List<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool Contact = false;

    void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        Debug.Log("Inter");
        Contact = true;

        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (Contact)
        {
            Debug.Log("Stop");
            StoreItems();
            bag.Clear();
            isOpen = false;
            spriteRenderer.sprite = closeSprite;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;

            Contact = false;
        }

    }

    public void AddItems()
    {
        if(items != null)
        {
            foreach(Item item in items)
            {
                item.MySlot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        items = bag.GetItems();
    }
}
