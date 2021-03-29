using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Obbstacle : MonoBehaviour, IComparable<Obbstacle>
{
    // Start is called before the first frame update
    public SpriteRenderer MySpriteRenderer { get; set; }

    private Color defaultColor;

    private Color fadeColor;


    public int CompareTo(Obbstacle other)
    {
        if(MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;
        }
        else if(MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        defaultColor = MySpriteRenderer.color;
        fadeColor = defaultColor;
        fadeColor.a = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        MySpriteRenderer.color = fadeColor;
    }
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }
}
