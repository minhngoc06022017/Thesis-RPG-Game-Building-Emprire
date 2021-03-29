using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private string type;

    public string MyType
    {
        get
        {
            return type;
        }
    }
    [SerializeField]
    private int level;
    public int MyLevel
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    [SerializeField]
    private float speed;
    public float MySpeed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public Transform MyTarget { get; set; }

    protected Vector2 direction;

    public Vector2 MyDirection
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    protected Animator myAnimator;
    public Animator MyAnimator
    {
        get
        {
            return myAnimator;
        }
        set
        {
            myAnimator = value;
        }
    }

    private Rigidbody2D myRigidbody;

    protected bool isAttacking=false;
    public bool MyIsAttacking
    {
        get
        {
            return isAttacking;
        }
        set
        {
            isAttacking = value;
        }
    }

    protected Coroutine actionRoutine;

    public bool IsAlive
    {
        get
        {
            return MyHealth.MyCurrentValue > 0;
        }
    }

    [SerializeField]
    protected Stat health;

    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    protected float initialHealth; 

    [SerializeField]
    protected Transform hitBox;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Handlelayers();
    }
    private void FixedUpdate()
    {
        Move();
        
    }

    public void Move()
    {
        if (IsAlive)
        {
            myRigidbody.velocity = direction.normalized * speed;
        }

    }
    public void Handlelayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {

                Activatelayer("Walk");

                myAnimator.SetFloat("x", direction.x);
                myAnimator.SetFloat("y", direction.y);
                //Debug.Log("Walking");


            }
            else if (isAttacking)
            {
                Activatelayer("Attack");
            }
            else
            {
                Activatelayer("Idle");
                //Debug.Log("Standing");
            }
        }
        else
        {
            Activatelayer("Death");
        }
    }
    
    public void Activatelayer(string layername)
    {
        for(int i=0;i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layername), 1);
    }
    

    public virtual void TakeDamage(float damage, Transform source)
    { 
        health.MyCurrentValue -= damage;
        CombatTextManager.Instance.CreateText(transform.position, damage.ToString(), SCTTYPE.Damage,true);
        if (health.MyCurrentValue <= 0)
        {
            MyDirection = Vector2.zero;
            myRigidbody.velocity = MyDirection;
            gameObject.GetComponent<Enemy>().OnCharacterRemoved();
            GameManager.Instance.OnKillConfirmed(this);
            myAnimator.SetTrigger("die");
            
        }
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.Instance.CreateText(transform.position, health.ToString(), SCTTYPE.Health,false);
    }
}
