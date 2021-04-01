using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiskPanel : MonoBehaviour
{
    [SerializeField] GameControl _gameControl;
    
    private int betAmount;
    private int multiple;
    private int totalAmount;
    
    [SerializeField] Text _totalAmount, _winAmount,_status;
    [SerializeField] GameObject _riskPanel;
    [SerializeField] Button _fireButton, _iceButton,_take;
    [SerializeField] Transform _background;
    [SerializeField] GameObject[] _listItems;
    bool _activateTimeCount = false;
    float _timeCount = 0;
    float _timeOut = 1.5f;
    bool run = false;
    public float speed = 20f;
    void Start()
    {
        betAmount = _gameControl.winBet;
        totalAmount = _gameControl.totalAmount;
        multiple = 1;
        _totalAmount.text = "$" + totalAmount;
    }

    // Update is called once per frame
    void Update()
    {
        _winAmount.text = "$" + betAmount * multiple;
        if (multiple == 0)
        {
            _status.text = "LOSE";
            _activateTimeCount = true;
        }
        if (multiple == 32)
        {
            _status.text = "YOU WIN";
            _activateTimeCount = true;
        }
        if (_activateTimeCount)
        {
            _timeCount += Time.deltaTime;
            if(_timeCount >= _timeOut)
            {
                run = true;
            }
        }
        if (run)
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
            if(transform.position.y >= 580)
            {
                _gameControl.winBet = betAmount * multiple;
                _riskPanel.active = false;
            }
        }
        
    }
    public void onClickFire()
    {
        int choose = 0;
        int r = Random.Range(0, 2);
        Debug.Log(r);
        spawnObject(r);
        if (choose == r)
        {
            multiple *= 2;
        }
        else
        {
            multiple = 0;
            _iceButton.enabled = false;
            _fireButton.enabled = false;
        }
    }
    public void onClickIce()
    {
        
        int choose = 1;
        int r = Random.Range(0, 2);
        Debug.Log(r);
        spawnObject(r);
        if(choose == r)
        {
            multiple *= 2;
        }
        else
        {
            multiple=0;
            _iceButton.enabled = false;
            _fireButton.enabled = false;
        }
    }
    public void spawnObject(int index)
    {
        GameObject newListItem = Instantiate(_listItems[index]) as GameObject;
        newListItem.transform.SetParent(_background);
    }
    public void onClickTake()
    {
        _gameControl.winBet = betAmount * multiple;
        _riskPanel.active = false;
    }
}
