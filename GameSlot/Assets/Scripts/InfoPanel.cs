using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]GameControl _gameControl;
    [SerializeField] Text[] _textList;
    [SerializeField] bool isSpecial;
    void Start()
    {
        
    }

    
    void Update()
    {
        printInfo();
    }

    public void printInfo()
    {
        if (isSpecial)
        {
            _textList[0].text = "x3:$ " + _gameControl.betAmount * 10;
            _textList[1].text = "x4:$ " + _gameControl.betAmount * 20;
            _textList[2].text = "x5:$ " + _gameControl.betAmount * 50;
        }
        else
        {
            if (_textList.Length == 3)
            {
                _textList[0].text = "x3:$ " + _gameControl.betAmount * 2;
                _textList[1].text = "x4:$ " + _gameControl.betAmount * 5;
                _textList[2].text = "x5:$ " + _gameControl.betAmount * 10;
            }
            else
            {
                _textList[0].text = "x2:$ " + _gameControl.betAmount;
                _textList[1].text = "x3:$ " + _gameControl.betAmount * 2;
                _textList[2].text = "x4:$ " + _gameControl.betAmount * 5;
                _textList[3].text = "x5:$ " + _gameControl.betAmount * 10;
            }
        }
    }
}
