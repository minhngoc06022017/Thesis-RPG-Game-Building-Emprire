using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Enemy>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //parent.MyTarget = collision.transform;
            parent.SetTarget(collision.transform);
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        parent.MyTarget = null;
    //    }
    //}
}
