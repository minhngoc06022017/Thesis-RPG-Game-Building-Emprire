using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] GameObject _spawnEffect;
    public ArrayList _listSpawnEffect;
    int[,] fruitTable = new int[3, 5];
    private int tempValue = 0;
    public int totalAmount = 2000000;
    public int winBet = 0;
    private int tempWinBet = 0;
    public int betAmount = 0;
    public int startIndexBetAmount = 0;
    private bool rollButtonCollect = false;
    private int[] _comboCasePublic = new int[6];
    int[] betAmounts = {50000,100000,200000,300000,500000,1000000,2000000,3000000,5000000,10000000,20000000,30000000,50000000,100000000};//13 index
    [SerializeField] GameObject infoPanel, riskPanel, bigWinPanel,mainPanel;
    [SerializeField] Text winStatus, winAmount, betAmountTxt, totalAmountTXT,rollButtonText;
    public GameObject riskButton ;
    [SerializeField] GameObject[] sprites;
    [SerializeField] Sprite[] _listItems;
    [SerializeField] GameObject _rollButton;
    public int _moneyCountUp;
    int[] items = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    // non(15%) , xux xich(15%) , thung ruou(15%) , bia(15%) , ken(15%) , banh(10%) , image(5%) , wild(5%) , scatter(5%)

    void Start()
    {
        
        riskButton.SetActive(false);
        winStatus.text = "";
        winAmount.text = "";
        betAmount = betAmounts[startIndexBetAmount];
        betAmountTxt.text = betAmount + "";
        totalAmountTXT.text = totalAmount + "";
        _listSpawnEffect = new ArrayList(); 
        
    }

    
    void Update()
    {

        betAmount = betAmounts[startIndexBetAmount];
        betAmountTxt.text = betAmount + "";
        totalAmountTXT.text = totalAmount + "";
        winAmount.text = winBet + "";
    }
    
    public void onClickShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void outInfo()
    {
        infoPanel.SetActive(false);
    }
    public void onClickRiskPanel()
    {
        riskButton.active=false;
        riskPanel.active = true;
    }
    public void outRisk()
    {

    }
    public int checkScatter(bool takePoint)
    {
        int count = 0;
        int[,] _position = new int[5, 2];
        for (int i = 0; i < fruitTable.GetLength(0); i++)
        {
            for (int j = 0; j < fruitTable.GetLength(1); j++)
            {
                if (fruitTable[i,j] == items[8])
                {
                    _position[count, 0] = j;
                    _position[count, 1] = i;
                    count++;
                }
            }
        }
        
            if (count >= 3)
            {
                if (takePoint)
                {
                    SpawnEffect s = _spawnEffect.GetComponent<SpawnEffect>();
                    s.type = 0;
                    s.length = count;
                    s._position = _position;
                    s._spawnObject();
                }
                tempWinBet += betAmount * getMultiple(count, true);
            }
        

        return count;
    }


    public int checkLineX(int value, int max, int index,bool takePoint)
    {
        int count = 0;
        int[,] _position = new int[5, 2];
        _position[0,0] = 0;
        _position[0, 1] = index;
        if (value != items[7])
        {
            tempValue = value;
            for (int i = 1; i < max; i++)
            {
                if (fruitTable[index, i] == value || fruitTable[index, i] == items[7])
                {
                    _position[i, 0] = i;
                    _position[i, 1] = index;
                    count++;
                }
                else break;
            }
        }
        else
        {
            tempValue = value;
            bool firstChange = true;
            for (int i = 1; i < max; i++)
            {
                if (fruitTable[index, i] == items[7] || fruitTable[index, i] == value)
                {
                    _position[i, 0] = i;
                    _position[i, 1] = index;
                    count++;
                }else if (fruitTable[index, i] != value && firstChange)
                {
                    value = fruitTable[index, i];
                    tempValue = value;
                    firstChange = false;
                    _position[i, 0] = i;
                    _position[i, 1] = index;
                    count++;
                }
                else break;
            }
        }
        
            if (tempValue == 5 && count >= 1)
            {
                if (takePoint)
                {
                    SpawnEffect s = _spawnEffect.GetComponent<SpawnEffect>();
                    s.type = 1;
                    s.length = count + 1;
                    s.index = index;
                    s._position = _position;
                    s._spawnObject();
                }
                tempWinBet += betAmount * getMultiple(count, false);

            }
            if (tempValue != 5 && count >= 2)
            {
                if (takePoint)
                {
                    SpawnEffect s = _spawnEffect.GetComponent<SpawnEffect>();
                    s.type = 1;
                    s.length = count + 1;
                    s.index = index;
                    s._position = _position;
                    s._spawnObject();
                }
                if (tempValue == 6)
                {
                    tempWinBet += betAmount * getMultiple(count, true);

                }
                else tempWinBet += betAmount * getMultiple(count, false);
            }
        
        return count;
    }
    
    

public bool checkMaxMinIndex(int index)
{
    if (index == 0 || index == 2)
    {
        return true;
    }
    else
    {
        return false;
    }
}
public void onClickIncrease()
    {
        Debug.Log("Increase");
        if (startIndexBetAmount == 12)
        {
            return;
        }
        else
        {
            startIndexBetAmount++;
        }
        
    }
public void onClickDecrease()
    {
        Debug.Log("Decrease");
        if (startIndexBetAmount == 0)
        {
            return;
        }
        else
        {
            startIndexBetAmount--;
        }
       
        
    }
    public int checkZicZac(int value, int max, int index, int type,bool takePoint)
    {
        int firstType;
        if (type == 1)
        {
            firstType = 2;
        }
        else
        {
            firstType = 3;
        }

        int count = 0;
        int[,] _position = new int[5, 2];
        _position[0, 0] = 0;
        _position[0, 1] = index;
        if (value != items[7])
        {
            tempValue = value;
            for (int i = 1; i < max; i++)
            {
                //Debug.Log(index);
                if (type == 1)
                {
                    index++;
                    if (fruitTable[index, i] == value || fruitTable[index, i] == items[7])
                    {
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;
                    }
                    else break;
                }
                else
                {
                    index--;

                    if (fruitTable[index, i] == value || fruitTable[index, i] == items[7])
                    {
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;

                    }
                    else break;
                }
                if (checkMaxMinIndex(index)) type *= -1;
            }
        }
        else
        {
            tempValue = value;
            bool firstChange = true;
            for (int i = 1; i < max; i++)
            {
                
                //Debug.Log(index);
                if (type == 1)
                {
                    index++;
                    if (fruitTable[index, i] == items[7] || fruitTable[index, i] == value)
                    {
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;
                    }
                    else if (fruitTable[index, i] != value && firstChange)
                    {
                        value = fruitTable[index, i];
                        tempValue = value;
                        firstChange = false;
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;
                    }
                    else break;
                }
                else
                {
                    index--;

                    if (fruitTable[index, i] == items[7] || fruitTable[index, i] == value)
                    {
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;
                    }
                    else if (fruitTable[index, i] != value && firstChange)
                    {
                        value = fruitTable[index, i];
                        tempValue = value;
                        firstChange = false;
                        _position[i, 0] = i;
                        _position[i, 1] = index;
                        count++;
                    }
                    else break;
                }
                if (checkMaxMinIndex(index)) type *= -1;
            }
        }
        
            if (tempValue == 5 && count >= 1)
            {
                if (takePoint)
                {
                    SpawnEffect s = _spawnEffect.GetComponent<SpawnEffect>();
                    s.type = firstType;
                    s.length = count + 1;

                    s._position = _position;
                    s._spawnObject();
                }
                tempWinBet += betAmount * getMultiple(count, false);
            }
            if (tempValue != 5 && count >= 2)
            {
                if (takePoint)
                {
                    SpawnEffect s = _spawnEffect.GetComponent<SpawnEffect>();
                    s.type = firstType;
                    s.length = count + 1;

                    s._position = _position;
                    s._spawnObject();
                }
                if (tempValue == 6)
                {
                    tempWinBet += betAmount * getMultiple(count, true);
                }
                else tempWinBet += betAmount * getMultiple(count, false);
            }
        
        return count;
    }
    public void onClickRoll()
    {
        if (!rollButtonCollect)
        {
            _rollButton.active = false;
            totalAmount -= betAmount;
            winBet = 0;
            winAmount.text = "";
            winStatus.text = "";


            //fruitTable = new int[,]
            //        {{7,1,1,8,7},
            //         {1,1,1,1,8 },
            //         {7,1,1,2,8 }};
            int index = 0;
            for (int i = 15; i < 20; i++)
            {
                RollSprite roll = sprites[i].GetComponent<RollSprite>();
                roll.run = true;
            }
            for (int i = 0; i < fruitTable.GetLength(0); i++)
            {
                for (int j = 0; j < fruitTable.GetLength(1); j++)
                {
                    int r = chooseIndex(Random.Range(0, 100));
                    fruitTable[i, j] = items[r];
                    RollSprite roll = sprites[index].GetComponent<RollSprite>();
                    roll.run = true;
                    roll._mainSprite = _listItems[r];
                    index++;
                }
            }
            int countCombo = 0;
            int comboIndex = 0;
            int[] comboCase = new int[6];
            int countWinBet = 0;
            if (checkScatter(false) >= 3)
            {
                comboCase[comboIndex] = 1;
                countCombo++;
                countWinBet += tempWinBet;
                tempWinBet = 0;
            }
            else
            {
                comboCase[comboIndex] = 0;
            }
            comboIndex++;

            for (int i = 0; i < fruitTable.GetLength(0); i++)
            {
                int tempCombo = checkLineX(fruitTable[i, 0], fruitTable.GetLength(1), i, false);
                if (tempCombo >= 1 && tempValue == 5)
                {
                    comboCase[comboIndex] = 1;
                    countCombo++;
                    countWinBet += tempWinBet;
                    tempWinBet = 0;
                }
                else if (tempCombo >= 2 && tempValue != 5)
                {
                    comboCase[comboIndex] = 1;
                    countCombo++;
                    countWinBet += tempWinBet;
                    tempWinBet = 0;
                }
                else
                {
                    comboCase[comboIndex] = 0;
                }
                comboIndex++;

                if (i == 0)
                {
                    tempCombo = checkZicZac(fruitTable[i, 0], fruitTable.GetLength(1), i, 1, false);
                    if (tempCombo >= 1 && tempValue == 5)
                    {
                        comboCase[comboIndex] = 1;
                        countCombo++;
                        countWinBet += tempWinBet;
                        tempWinBet = 0;
                    }
                    else if (tempCombo >= 2 && tempValue != 5)
                    {
                        comboCase[comboIndex] = 1;
                        countCombo++;
                        countWinBet += tempWinBet;
                        tempWinBet = 0;
                    }
                    else
                    {
                        comboCase[comboIndex] = 0;
                    }
                    comboIndex++;
                }
                if (i == 2)
                {
                    tempCombo = checkZicZac(fruitTable[i, 0], fruitTable.GetLength(1), i, -1, false);
                    if (tempCombo >= 1 && tempValue == 5)
                    {
                        comboCase[comboIndex] = 1;
                        countCombo++;
                        countWinBet += tempWinBet;
                        tempWinBet = 0;
                    }
                    else if (tempCombo >= 2 && tempValue != 5)
                    {
                        comboCase[comboIndex] = 1;
                        countCombo++;
                        countWinBet += tempWinBet;
                        tempWinBet = 0;
                    }
                    else
                    {
                        comboCase[comboIndex] = 0;
                    }
                    comboIndex++;
                }
            }
            if (countCombo >= 3) _moneyCountUp = countWinBet;
            _comboCasePublic = comboCase;
            StartCoroutine(check(countCombo, false,comboCase));
        }
        else
        {
            rollButtonCollect = false;
            totalAmount += winBet;
            rollButtonText.text = "Roll";
            riskButton.active = false;
            winBet = 0;
            winAmount.text = "";
            winStatus.text = "";
        }
        
        
    }
public void onClickBigWinCollect()
    {
        StartCoroutine(check(1, true,_comboCasePublic));
    }


IEnumerator check(int countCombo,bool reCheck,int[] comboCase)
    {
        bool pass = true;
        if (reCheck)
        {
            mainPanel.active = true;
            bigWinPanel.active = false;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
        if (countCombo >= 3)
        {
            Debug.Log("Big Win");
            bigWinPanel.active = true;
            mainPanel.active = false;
            pass = false;
        }
        if(countCombo>=1 && pass)
        {
            int[] _comboCase = comboCase;
            int comboIndex = 0;
            if (comboCase[comboIndex] == 1)
            {
                checkScatter(true);
                if (tempWinBet > 0)
                {
                    winBet += tempWinBet;
                    winAmount.text = winBet + "";
                    winStatus.text = "Win";
                    tempWinBet = 0;
                }
            }
            comboIndex++;
            for (int i = 0; i < fruitTable.GetLength(0); i++)
            {


                if (comboCase[comboIndex] == 1)
                {
                    yield return new WaitForSeconds(1f);
                    checkLineX(fruitTable[i, 0], fruitTable.GetLength(1), i, true);
                    if (tempWinBet > 0)
                    {
                        winBet += tempWinBet;
                        winAmount.text = winBet + "";
                        winStatus.text = "Win";
                        tempWinBet = 0;
                    }
                }
                comboIndex++;


                if (i == 0)
                {

                    if (comboCase[comboIndex] == 1)
                    {
                        yield return new WaitForSeconds(1f);
                        checkZicZac(fruitTable[i, 0], fruitTable.GetLength(1), i, 1, true);
                        if (tempWinBet > 0)
                        {
                            winBet += tempWinBet;
                            winAmount.text = winBet + "";
                            winStatus.text = "Win";
                            tempWinBet = 0;
                        }
                    }
                    comboIndex++;

                }
                if (i == 2)
                {
                    if (comboCase[comboIndex] == 1)
                    {
                        yield return new WaitForSeconds(1f);
                        checkZicZac(fruitTable[i, 0], fruitTable.GetLength(1), i, -1, true);
                        if (tempWinBet > 0)
                        {
                            winBet += tempWinBet;
                            winAmount.text = winBet + "";
                            winStatus.text = "Win";
                            tempWinBet = 0;
                        }
                    }
                    comboIndex++;

                }
            }
            
            if (winBet > 0)
            {
                rollButtonText.text = "Collect";
                rollButtonCollect = true;
                riskButton.active = true;
            }
        }
        _rollButton.active = true;
    }




    //
    public int getMultiple(int combo,bool isSpecial)
    {
        int mul = 1;
        if (!isSpecial)
        {
            switch (combo)
            {
                case 1:
                    mul = 1;
                    break;
                case 2:
                    mul = 2;
                    break;
                case 3:
                    mul = 5;
                    break;
                case 4:
                    mul = 10;
                    break;

            }
        }
        else
        {
            switch (combo)
            {
                case 3:
                    mul = 10;
                    break;
                case 4:
                    mul = 20;
                    break;
                case 5:
                    mul = 50;
                    break;

            }
        }
        return mul;
    }
public int chooseIndex(int random)
{
    if (random >= 0 && random < 15)
    {
        return 0;
    }
    else if (random >= 15 && random < 30)
    {
        return 1;
    }
    else if (random >= 30 && random < 45)
    {
        return 2;
    }
    else if (random >= 45 && random < 60)
    {
        return 3;
    }
    else if (random >= 60 && random < 75)
    {
        return 4;
    }
    else if (random >= 75 && random < 85)
    {
        return 5;
    }
    else if (random >= 85 && random < 90)
    {
        return 6;
    }
    else if (random >= 90 && random < 95)
    {
        return 7;
    }
    else
    {
        return 8;
    }
}
    
}
