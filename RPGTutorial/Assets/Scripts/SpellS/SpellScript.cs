using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField]
    private float speed;

    private int damage;

    private Transform source;

    private Transform target;
    public Transform Mytarget
    { get
       {
            return target;
       }
        set
        {
            target = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        this.Mytarget = target;
        this.damage = damage;
        this.source = source;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Mytarget != null)
        {
            Vector2 direction = target.position - transform.position;
            myRigidbody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == Mytarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDamage(damage, source);
            target = null;
            Destroy(gameObject);
        }
    }
}
