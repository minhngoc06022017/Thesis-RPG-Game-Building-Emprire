using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, ICastable
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private string description;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private Image highlight;

    [SerializeField]
    private float craftTime;

    [SerializeField]
    private Color barColor;



    public Item MyOutput
    {
        get
        {
            return output;
        }
    }
    public string MyDescription
    {
        get
        {
            return description;
        }
    }
    public int MyOutputCount
    {
        get
        {
            return outputCount;
        }
    }
    public Image MyHighlight
    {
        get
        {
            return highlight;
        }
    }
    public CraftingMaterial[] MyMaterials
    {
        get
        {
            return materials;
        }
    }

    public string MyTitle
    {
        get
        {
            return output.MyTitle;
        }
    }
    public Sprite MyIcon
    {
        get
        {
            return output.MyIcon;
        }
    }
    public float MyCastTime
    {
        get
        {
            return craftTime;
        }
    }
    public Color MyBarColor
    {
        get
        {
            return barColor;
        }
    }
  

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = output.MyTitle;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = 0.4f;
        highlight.color = c;
    }
    public void Deselect()
    {
        Color c = highlight.color;
        c.a = 0f;
        highlight.color = c;
    }
}
