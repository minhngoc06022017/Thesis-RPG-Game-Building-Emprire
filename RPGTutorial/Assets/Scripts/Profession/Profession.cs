using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profession : MonoBehaviour
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    private List<int> amounts = new List<int>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private ItemInfo craftItemInfo;

    [SerializeField]
    private Text countTxt;

    private int maxAmount;

    private int amount;

    private int MyAmount
    {
        set
        {
            countTxt.text = value.ToString();
            amount = value;
        }
        get
        {
            return amount;
        }
    }

    private void Start()
    {
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChange(UpdateMaterialCount);

        ShowDescription(selectedRecipe);
    }

    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.Deselect();
        }

        this.selectedRecipe = recipe;
        this.selectedRecipe.Select();

        foreach(GameObject gameObject in materials)
        {
            Destroy(gameObject);
        }

        materials.Clear();

        title.text = recipe.MyOutput.MyTitle;

        description.text = recipe.MyDescription + " " + recipe.MyOutput.MyTitle.ToLower();

        craftItemInfo.Initialize(recipe.MyOutput, 1);

        foreach(CraftingMaterial material in recipe.MyMaterials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);

            materials.Add(tmp);
        }

        UpdateMaterialCount(null);
    }

    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }
        if (CanCraft())
        {
            maxAmount = amounts[0];

            if(countTxt.text == "0")
            {
                MyAmount = 1;
            }
            else if(int.Parse(countTxt.text) > maxAmount)
            {
                MyAmount = maxAmount;
            }
        }
        else
        {
            MyAmount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool all)
    {
        
        if (CanCraft() && !Player.Instance.MyIsAttacking)
        {
            if (all)
            {
                amounts.Sort();
                countTxt.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else
            {
                StartCoroutine(CraftRoutine(MyAmount));
            }
        }
    }

    private bool CanCraft()
    {
        bool canCraft = true;

        amounts = new List<int>();

        foreach(CraftingMaterial material in selectedRecipe.MyMaterials)
        {
            int count = InventoryScript.Instance.GetItemCount(material.MyItem.MyTitle);

            if(count >= material.MyCount)
            {
                amounts.Add(count/material.MyCount);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }
        }

        return canCraft;
    }

    public void ChangeAmount(int i)
    {
        if((amount + i) > 0 && (amount + i) <= maxAmount)
        {
            MyAmount += i;
        }
    }

    public IEnumerator CraftRoutine(int count)
    {
        for(int i=0;i< count; i++)
        {
            yield return Player.Instance.MyInitRoutine = StartCoroutine(Player.Instance.CraftRoutine(selectedRecipe));
        }
    }

    public void AddItemsToInventory()
    {
        if (InventoryScript.Instance.AddItem(craftItemInfo.MyItem))
        {
            foreach(CraftingMaterial material in selectedRecipe.MyMaterials)
            {
                for(int i = 0; i < material.MyCount; i++)
                {
                    InventoryScript.Instance.RemoveItem(material.MyItem);
                }
            }
        }
        
    }
}
