using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    private  int _metals = 0;
    private  int _coins = 0;
    private  int _gems = 0;
    private void Awake()
    {
        _metals = PlayerPrefs.GetInt("Metals");
        _coins = PlayerPrefs.GetInt("Coins");
        _gems = PlayerPrefs.GetInt("Gems");
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  int Metals
    {
        get
        {
            return _metals;
        }
        set
        {
            PlayerPrefs.SetInt("Metals", (_metals = value));
        }
    }
    public  int Coins
    {
        get
        {
            return _coins;
        }
        set
        {
            PlayerPrefs.SetInt("Coins", (_coins = value));
        }
    }
    public  int Gems
    {
        get
        {
            return _gems;
        }
        set
        {
            PlayerPrefs.SetInt("Gems", (_gems = value));
        }
    }
}
