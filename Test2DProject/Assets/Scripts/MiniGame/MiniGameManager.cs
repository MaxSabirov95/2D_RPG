using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Button redButton;
    public Button yellowButton;
    public Button greenButton;
    public Button playButton;

    Vector2 pos1 = new Vector2(-30, -30);
    Vector2 pos2 = new Vector2(0, -30);
    Vector2 pos3 = new Vector2(30, -30);

    int Pos1Place;
    int Pos2Place;
    int Pos3Place;

    public static float time=0;
    bool canChange;

    private void Start()
    {
        redButton.interactable = false;
        yellowButton.interactable = false;
        greenButton.interactable = false;
    }

    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }

        else if((time <= 0)&&(canChange))
        {
            redButton.interactable = true;
            yellowButton.interactable = true;
            greenButton.interactable = true;
            random();
            canChange = false;
        }
    }

    public void Which()
    {
        if (time <= 0)
        {
            redButton.interactable = false;
            yellowButton.interactable = false;
            greenButton.interactable = false;
            time = 2;
            canChange = true;
        }
    }

    void random()
    {
        Pos1Place = Random.Range(1, 4);
        Pos2Place = Random.Range(1, 4);
        Pos3Place = Random.Range(1, 4);

        while (Pos1Place == Pos2Place || Pos1Place == Pos3Place || Pos2Place == Pos3Place)
        {
            if (Pos1Place == Pos2Place)
            {
                Pos2Place = Random.Range(1, 4);
            }
            else if ((Pos1Place == Pos3Place) || (Pos3Place == Pos2Place))
            {
                Pos3Place = Random.Range(1, 4);
            }
        }
        Position();
    }

    void Position()
    {
        if (Pos1Place == 1)
        {
            redButton.transform.position = pos1;
        }
        else if (Pos1Place == 2)
        {
            redButton.transform.position = pos2;
        }
        else if (Pos1Place == 3)
        {
            redButton.transform.position = pos3;
        }

        if (Pos2Place == 1)
        {
            yellowButton.transform.position = pos1;
        }
        else if (Pos2Place == 2)
        {
            yellowButton.transform.position = pos2;
        }
        else if (Pos2Place == 3)
        {
            yellowButton.transform.position = pos3;
        }

        if (Pos3Place == 1)
        {
            greenButton.transform.position = pos1;
        }
        else if (Pos3Place == 2)
        {
            greenButton.transform.position = pos2;
        }
        else if (Pos3Place == 3)
        {
            greenButton.transform.position = pos3;
        }
    }

    public void PlayButton()
    {
        redButton.interactable = true;
        yellowButton.interactable = true;
        greenButton.interactable = true;
        playButton.interactable = false;
        random();
    }
}
