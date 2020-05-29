using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Max_Almog.MyCompany.MyGame
{
    public class GameUI : MonoBehaviour
    {
        public static int skillpoints;
        public Text skillpointsText;
        public GameObject skillPointBoard;
        private bool SkillBoard;

        public GameObject inventoryBoard;
        private bool InventoryBoard;


        private void Start()
        {
            skillPointBoard.SetActive(false);
            SkillBoard = false;
            InventoryBoard = false;
        }
        void Update()
        {
            skillpointsText.GetComponent<Text>().text=""+skillpoints.ToString("f0");
            if (Input.GetKeyDown(KeyCode.X))
            {
                SkillBoard = !SkillBoard;
                if (!SkillBoard)
                {
                    Time.timeScale = 1;
                    skillPointBoard.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    skillPointBoard.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                InventoryBoard = !InventoryBoard;
                if (!InventoryBoard)
                {
                    inventoryBoard.SetActive(false);
                }
                else
                {
                    inventoryBoard.SetActive(true);
                }
            }
        }

        public void SkillPointToHP()
        {
            return;
            //if (skillpoints > 0)
            //{
            //    skillpoints--;
            //    PlayerUI.WholeHP += 15;
            //}
        }
        public void SkillPointToMana()
        {
            return;
            //if (skillpoints > 0)
            //{
            //    skillpoints--;
            //    PlayerUI.WholeMana += 15;
            //}
        }
    }
}
