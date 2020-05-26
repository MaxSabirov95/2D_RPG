using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameItems : MonoBehaviour
{
    public static int money;
    public Text MoneyText;

    void Update()
    {
        MoneyText.GetComponent<Text>().text = "" + money.ToString("f0");
    }
}
