using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{

    [SerializeField] private GameObject building;
    [SerializeField] private GameObject muzzleFlashParticles;
    public float maxHealth;
    public float health;

    public float distanceFromTroop;
    public GameObject nearestTroop;
    public float nearestDistance = 1000f;
    public GameObject explosionPrefab;

    void Awake()
    {
        health = maxHealth;
    }

    void DamageTroop(GameObject[] troops)
    {
        if (troops.Length > 0) //If any troops are on the map
        {
            if (nearestDistance <= 0.1f) //If the troop is in range
            {
                Quaternion targetRotation = Quaternion.LookRotation(nearestTroop.transform.position - transform.position, Vector3.up); //Get target rotation
                Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime*10); //Smooth between current rotation and target rotation
                transform.eulerAngles = new Vector3(0, smoothRotation.eulerAngles.y ,0); //Setting the y rotation

                muzzleFlashParticles.SetActive(true);
                nearestTroop.GetComponentInChildren<TroopController>().health -= 1000f * Time.deltaTime; //Damage the troop
            }
            if (nearestTroop.GetComponentInChildren<TroopController>().health <= 0f) //If the troop is dead
            {
                nearestDistance = 1000f; //Reset the nearest distance
                muzzleFlashParticles.SetActive(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        GameObject[] troops = GameObject.FindGameObjectsWithTag("Troop");

        for (int i = 0; i < troops.Length; i++)
        {
             // Sets the distance from the troop as a float number
            distanceFromTroop = Vector3.Distance(this.transform.position, troops[i].transform.position);

            // If the distance of the current troop is less than the nearest distance that has been found so far and the troop is alive
            if (distanceFromTroop < nearestDistance && troops[i].GetComponentInChildren<TroopController>().health > 0)
            {
                // Set nearest troop to current troop
                nearestTroop = troops[i];
                // Set nearest distance to current distance
                nearestDistance = distanceFromTroop;

            }
        }

        DamageTroop(troops);

        if (health <= 0f)
        {
            Instantiate(explosionPrefab, building.transform.position, building.transform.rotation);
            Destroy(building);
        }
    }
}
