using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private Image icon;

    private Armor equippedArmor;
    public Armor MyEquippedArmor
    {
        get
        {
            return equippedArmor;
        }
        set
        {
            equippedArmor = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.Instance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.Instance.MyMoveable;

                if(tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }

                UIManager.Instance.RefreshTooltip(tmp);
            }else if(HandScript.Instance.MyMoveable == null && equippedArmor != null)
            {
                HandScript.Instance.TakeMoveable(equippedArmor);
                CharacterPanel.Instance.MySelectedButton = this;
                icon.color = Color.grey;

            }
        }
    }
    
    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (equippedArmor != null)
        {
            if(equippedArmor != armor)
            {
                armor.MySlot.AddItem(equippedArmor);
                armor.MySlot = null;
                UIManager.Instance.RefreshTooltip(equippedArmor);
            }
            
        }
        else
        {
            UIManager.Instance.HideToolTip();
        }

        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        this.equippedArmor = armor;
        this.MyEquippedArmor.MyCharButton = this;

        if(HandScript.Instance.MyMoveable == (armor as IMoveable))
        {
            HandScript.Instance.Drop();
        }

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(equippedArmor != null)
        {
            UIManager.Instance.ShowToolTip(new Vector2(0, 0),transform.position, equippedArmor);
        }
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        equippedArmor.MyCharButton = null;

        equippedArmor = null;
    }
}
