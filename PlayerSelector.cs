using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;

public class PlayerSelector : MonoBehaviour
{

    public bool[] buttonsSelected = {false, false, false};

    public void OnButtonOneSelect()
    {
        for (int i = 0; i < buttonsSelected.Length; i++)
        {
            buttonsSelected[i] = false;
        }
        buttonsSelected[0] = true;
    }
    public void OnButtonTwoSelect()
    {
        for (int i = 0; i < buttonsSelected.Length; i++)
        {
            buttonsSelected[i] = false;
        }
        buttonsSelected[1] = true;
    }
    public void OnButtonThreeSelect()
    {
        for (int i = 0; i < buttonsSelected.Length; i++)
        {
            buttonsSelected[i] = false;
        }
        buttonsSelected[2] = true;
    }

}
