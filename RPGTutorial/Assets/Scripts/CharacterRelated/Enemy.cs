using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();
public class Enemy : Character,IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }

    [SerializeField]
    private float initAggroRange; 

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    [SerializeField]
    private LootTable lootTable;

    //private Transform target;
    //public Transform MyTarget
    //{
    //    get
    //    {
    //        return target;
    //    }
    //    set
    //    {
    //        target = value;
    //    }
    //}

    protected void Awake()
    {
        health.Initialize(initialHealth, initialHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 2;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!MyIsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();
           
        }
        base.Update();
    }

    public Transform Select()
    {
        healthGroup.alpha = 1;

        return hitBox;
    }

    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.Instance.HideTargetFrame);
        
    }
   
    public override void TakeDamage(float damage, Transform source)
    {
        if(!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.Instance.MyAttackers.Remove(this);
                    Player.Instance.GainXP(XPManager.CalculateXP(this));
                }
            }
        }
    }
   

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach(IInteractable interactable in Player.Instance.MyInteractables)
            {
                if(interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.Instance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        LootWindow.Instance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }

    }
    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        //Destroy(gameObject);
    }
}
