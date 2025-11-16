using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    
    [SerializeField] private Slider slider;
    private Camera Camera;
    [SerializeField] private GameObject entity;
    public Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Start()
    {
        Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.transform.rotation; //Sets rotation of health bar to the rotation of the camera
        transform.position = entity.transform.position + offset; //Sets the position of the health bar to above the building
        
        try
        {
            UpdateHealthBar(entity.GetComponentInChildren<BuildingController>().health, entity.GetComponentInChildren<BuildingController>().maxHealth);
        }
        catch (System.Exception)
        {
            UpdateHealthBar(entity.GetComponentInChildren<TroopController>().health, entity.GetComponentInChildren<TroopController>().maxHealth);
        }
    }
}
