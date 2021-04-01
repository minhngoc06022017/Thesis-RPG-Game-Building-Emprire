using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCountUp : MonoBehaviour
{
    public int money;
    [SerializeField] Text winBetText;
    [SerializeField] GameControl _gameControl;
    private int divisor;
    int count = 0;
    
    void Start()
    {
        Debug.Log(_gameControl._moneyCountUp);
        divisor = _gameControl._moneyCountUp / 200;
        money = 0;
    }

    
    void Update()
    {
        if (count < 200)
        {
            count++;
        }
        else return;
        money += divisor;
        winBetText.text = "$" + money;
    }
}
