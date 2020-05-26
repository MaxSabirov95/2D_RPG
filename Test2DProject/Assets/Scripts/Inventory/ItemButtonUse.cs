using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButtonUse : MonoBehaviour
{
    public enum ItemButtons {normalHp, Mana, BigHp };
    public ItemButtons buttons;

    public void OnPointerClick()
    {
        switch (buttons)
        {
            case ItemButtons.normalHp:
                if(PlayerUI.WholeHP> PlayerUI.HP)
                {
                    PlayerUI.HP += 20;
                    Destroy(gameObject);
                }
                break;
            case ItemButtons.Mana:
                if (PlayerUI.WholeMana > PlayerUI.Mana)
                {
                    PlayerUI.Mana += 20;
                    Destroy(gameObject);
                }
                break;
            case ItemButtons.BigHp:
                if (PlayerUI.WholeHP > PlayerUI.HP)
                {
                    PlayerUI.HP += 40;
                    Destroy(gameObject);
                }
                break;
        }     
    }
}
