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
            return;
            //switch (buttons)
            //{
            //    case ItemButtons.normalHp:
            //        if(PlayerUI.WholeHP > PlayerUI.HP)
            //        {
            //            PlayerUI.HP += 20;
            //            Destroy(gameObject);
            //        }
            //        break;
            //    case ItemButtons.Mana:
            //        if (PlayerUI.WholeMana > PlayerUI.Mana)
            //        {
            //            PlayerUI.Mana += 20;
            //            Destroy(gameObject);
            //        }
            //        break;
            //    case ItemButtons.BigHp:
            //        if (PlayerUI.WholeHP > PlayerUI.HP)
            //        {
            //            PlayerUI.HP += 40;
            //            Destroy(gameObject);
            //        }
            //        break;
            //}     
        }
    }
}
