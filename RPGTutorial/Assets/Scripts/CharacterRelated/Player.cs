using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex = 2;

    [SerializeField]
    private Profession profession;

    [SerializeField]
    private Text levelText;

    private List<IInteractable> interactables = new List<IInteractable>();
    public List<IInteractable> MyInteractables
    {
        get
        {
            return interactables;
        }
        set
        {
            interactables = value;
        }
    }

    [SerializeField ]
    private Block[] blocks;

    private Vector3 min, max;

    public int MyGold { get; set; }

    public Coroutine MyInitRoutine { get; set; }

    [SerializeField]
    private Stat mana;

    public Stat MyMana
    {
        get
        {
            return mana;
        }
    }

    [SerializeField]
    private Stat xpStat;

    public Stat MyXpStat
    {
        get
        {
            return xpStat;
        }
    }

    [SerializeField]
    private float initialMana;

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

    private List<Enemy> attackers = new List<Enemy>();
    public List<Enemy> MyAttackers
    {
        get
        {
            return attackers;
        }
        set
        {
            attackers = value;
        }
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
                                         Mathf.Clamp(transform.position.y, min.y, max.y),
                                         transform.position.z);

        base.Update();
   
    }

    public void SetDefaults()
    {
        MyGold = 1000;
        health.Initialize(initialHealth, initialHealth);
        mana.Initialize(initialMana, initialMana);
        xpStat.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(26);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyBindManager.Instance.Keybinds["UP"]))
        {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyBindManager.Instance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }

        if (IsMoving)
        {
            StopAction();
            StopInit();
        }

        foreach (string action in KeyBindManager.Instance.Actionbinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.Instance.Actionbinds[action]))
            {
                UIManager.Instance.ClickActionButton(action);
            }
        }
        
    }
    public void SetLimits(Vector3 min,Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public IEnumerator AttackRoutine(ICastable castable)
    {
        Transform myCurrentTarget = MyTarget;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if(myCurrentTarget != null && InLineOfSight())
        {
            Spell newSpell = SpellBook.Instance.GetSpell(castable.MyTitle);

            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(myCurrentTarget, newSpell.MyDamage , transform);
        }

        StopAction();
       
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> items)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        LootWindow.Instance.CreatePages(items);
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        profession.AddItemsToInventory();
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.Instance.Cast(castable);

        isAttacking = true;
        //myAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(castable.MyCastTime);

        StopAction();
    }

    public void CastSpell(ICastable castable)
    {
        Block();
        if (MyTarget != null && !isAttacking && !IsMoving && InLineOfSight())
        {
            //Debug.Log(MyTarget.GetComponent<Enemy>().IsAlive);
            MyInitRoutine = StartCoroutine(AttackRoutine(castable));
        }
       
    }

    public void Gather(ICastable castable, List<Drop> items)
    {
        if (!isAttacking)
        {
            MyInitRoutine = StartCoroutine(GatherRoutine(castable, items));
        }
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }
    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactive();
        }

        blocks[exitIndex].Active();
    }
    public void StopAction()
    {
        SpellBook.Instance.StopCasting();
        isAttacking = false;
        myAnimator.SetBool("attack", isAttacking);
        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }
    }

    private void StopInit()
    {
        if (MyInitRoutine != null)
        {
            StopCoroutine(MyInitRoutine);
        }
    }

    public void GainXP(int xp)
    {
        xpStat.MyCurrentValue += xp;
        CombatTextManager.Instance.CreateText(transform.position, xp.ToString(), SCTTYPE.Xp, false);

        if(xpStat.MyCurrentValue >= xpStat.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }

    private IEnumerator Ding()
    {
        while (!xpStat.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        levelText.text = MyLevel.ToString();
        xpStat.MyMaxValue = Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f));
        xpStat.MyCurrentValue = xpStat.myOverflow;
        xpStat.Reset();

        if(xpStat.MyCurrentValue >= xpStat.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractable interactable = collision.GetComponent<IInteractable>();
            if (!MyInteractables.Contains(interactable))
            {
                MyInteractables.Add(interactable);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if(MyInteractables.Count > 0)
            {
                IInteractable interactable = MyInteractables.Find(x => x == collision.GetComponent<IInteractable>());

                if (interactable != null)
                {
                    interactable.StopInteract();
                }
                MyInteractables.Remove(interactable);
            }

        }
    }

}
