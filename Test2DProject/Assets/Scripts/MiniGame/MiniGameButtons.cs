using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameButtons : MonoBehaviour
{
    public enum Buttons { Red, Yellow, Green };
    public Buttons buttons;

    public void circles()
    {
        switch (buttons)
        {
            case Buttons.Red:
                break;
            case Buttons.Yellow:
                break;
            case Buttons.Green:
                break;
        }
    }
}
