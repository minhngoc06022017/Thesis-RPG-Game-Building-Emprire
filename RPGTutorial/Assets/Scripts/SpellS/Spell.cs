using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

[Serializable]
public class Spell : IUseable, IMoveable, IDescriable,ICastable
{
    [SerializeField]
    private string title;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    [SerializeField]
    private string description;

    public string MyTitle
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
        }
    }
    public GameObject MySpellPrefab
    {
        get
        {
            return spellPrefab;
        }
        set
        {
            spellPrefab = value;
        }
    }

    public int MyDamage { get => damage; set => damage = value; }
    public Sprite MyIcon { get => icon; set => icon = value; }
    public float MySpeed { get => speed; set => speed = value; }
    public float MyCastTime { get => castTime; set => castTime = value; }
    public Color MyBarColor { get => barColor; set => barColor = value; }


    public string GetDescription()
    {
        return string.Format("{0}\nCast time: {1} second(s)\n<color=#ffd111>{2}\n that causes {3} damage</color>", title,castTime,description,damage);
    }

    public void Use()
    {
        Player.Instance.CastSpell(this);
    }
}
