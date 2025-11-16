using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{

    [SerializeField] private GameObject ProgressBar;
    [SerializeField] private GameObject ProgressBarFill;
    private Slider slider;
    private Image image;

    public float totalBuildingsNum = 0f;
    private float remainingBuildingsNum;
    private int count = 0;


    void Start()
    {
        slider = ProgressBar.GetComponent<Slider>();
        image = ProgressBarFill.GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (slider.value <= 0)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }

        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");
        if (structures.Length > 0)
        {
            if (count <= 1)
            {
                totalBuildingsNum = structures.Length;
            }
            count++;
        }
        remainingBuildingsNum = structures.Length;

        if (totalBuildingsNum != 0)
            slider.value = 1 - (remainingBuildingsNum / totalBuildingsNum);


    }

}
