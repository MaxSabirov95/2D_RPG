using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Max_Almog.MyCompany.MyGame
{
    public class ItemButtonUse : MonoBehaviour
    {
        public enum ItemButtons {normalHp, Mana, BigHp };
        public ItemButtons buttons;

        public void OnPointerClick()
        {
            switch (buttons)
            {
                case ItemButtons.normalHp:
                    if (BlackBoard.playerUI.WholeHP > BlackBoard.playerUI.HP)
                    {
                        BlackBoard.playerUI.HP += 20;
                        Destroy(gameObject);
                    }
                    break;
                case ItemButtons.Mana:
                    if (BlackBoard.playerUI.WholeMana > BlackBoard.playerUI.Mana)
                    {
                        BlackBoard.playerUI.Mana += 20;
                        Destroy(gameObject);
                    }
                    break;
                case ItemButtons.BigHp:
                    if (BlackBoard.playerUI.WholeHP > BlackBoard.playerUI.HP)
                    {
                        BlackBoard.playerUI.HP += 40;
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}
