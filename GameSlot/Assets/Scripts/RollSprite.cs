using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSprite : MonoBehaviour
{
    
    [SerializeField] Transform _upTransform;
    [SerializeField] Transform _initialTransform;
    [SerializeField] Transform _downTransform;
    public Sprite _mainSprite;
    [SerializeField] float _chooseMainSprite;
    [SerializeField] float _timeCoolDown;
    [SerializeField] Sprite[] _spriteList;

    private SpriteRenderer _sp;
    float _time = 0;
    private Transform _transform;
    public float _speed = -3;
    public bool run = false;
    int count = 0;
    void Start()
    {
        _transform = transform;
        _sp = _transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float _tempSpeed;
        if (run)
        {
            _time += Time.deltaTime;
            if (_time >= _timeCoolDown)
            {
                _transform.position = new Vector3(_transform.position.x, _initialTransform.position.y, _transform.position.z);
                run = false;
                _time = 0;
                count = 0;
                return;
            }
            if (_time >= _timeCoolDown / 5 && _time <= _timeCoolDown - (_timeCoolDown / 5))
            {
                _tempSpeed = _speed * 5;
            }
            else _tempSpeed = _speed;
            _transform.Translate(0, _tempSpeed * Time.deltaTime, 0);
            if (_transform.position.y < _downTransform.position.y)
            {
                count++;
                //Debug.Log(count);
                if (count == _chooseMainSprite)
                {
                    _sp.sprite = _mainSprite;
                }
                else
                {
                    int r = Random.Range(0, 8);
                    _sp.sprite = _spriteList[r];
                }
                _transform.position = new Vector3(_transform.position.x, _upTransform.position.y, _transform.position.z);
            }
        }
    }
}
