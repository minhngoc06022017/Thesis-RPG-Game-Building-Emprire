using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField]
    private Text statValue;

    [SerializeField]
    private float lerpSpeed;

    private float currentFill;

    private float overflow;
    public float myOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }

    public float MyMaxValue { get; set;}
    
    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }

    private float currentValue;
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if(value> MyMaxValue)
            {
                overflow = value - MyMaxValue;
                currentValue = MyMaxValue;
            }else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

          

            currentFill = currentValue / MyMaxValue;
            if (statValue != null)
            {
                statValue.text = currentValue + "/" + MyMaxValue;
            }
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
        content.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
        
    }

    private void HandleBar()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed); //chuyen dong deu
            /*content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);*/// Cang ngay toc do cang giam
        }
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }

    public void Initialize(float currentValue,float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }
}
