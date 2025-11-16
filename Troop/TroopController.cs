using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class TroopController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject spawnParticlePrefab;

    // The troop's NavMesh component
    public NavMeshAgent agent;
    public float distanceFromStructure;
    public GameObject nearestStructure;

    public NavMeshSurface surface;
    float nearestDistance = 10000f;

    public float maxHealth = 1500f;
    public float health;
    public float damage = 100f;
    [SerializeField] private GameObject troop;
    private bool dead;

    void Awake()
    {
        health = maxHealth;
        agent.enabled = false;
    }

    void DamageBuilding(GameObject[] structures)
    {
        if (structures.Length > 0)
        {
            if (nearestDistance <= 0.03f)
            {
                if (!dead)
                {
                    nearestStructure.GetComponentInChildren<BuildingController>().health -= damage * Time.deltaTime;
                }
                agent.enabled = false;
                animator.SetBool("isAttacking", true);
            }

            if (nearestStructure.GetComponentInChildren<BuildingController>().health <= 0f)
            {
                nearestDistance = 1000f;
                agent.enabled = true;
                animator.SetBool("isAttacking", false);
            }
        }
    }

    IEnumerator death()
    {
        dead = true;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
        agent.enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        

        // Wait for 5 seconds before running next section of code
        yield return new WaitForSeconds(5);
        Instantiate(spawnParticlePrefab, troop.transform.position, troop.transform.rotation);
        Destroy(troop);
    }


    void Update()
    {

        // Makes a "structures" list of all gameobjects with the tag "structures"
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");

        
        // Iterates through the structures list
        for (int i = 0; i < structures.Length; i++)
        {
            // Sets the distance from the structure as a float number
            distanceFromStructure = Vector3.Distance(this.transform.position, structures[i].transform.position);

            // If the distance of the current structure is less than the nearest distance that has been found so far
            if (distanceFromStructure < nearestDistance)
            {
                // Set nearest structure to current structure
                nearestStructure = structures[i];
                // Set nearest distance to current distance
                nearestDistance = distanceFromStructure;
                if (agent.enabled)
                {
                    // Sets the destination of the NavMesh agent to the position of the building
                    agent.SetDestination(nearestStructure.transform.position);
                }
            }
        }


        if (structures.Length == 0)
        {
            nearestDistance = 0f;
            animator.SetBool("isGameWon", true);
            agent.enabled = false;
        }

        if (structures.Length > 0)
        {
            troop.transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(nearestStructure.transform.position - transform.position).eulerAngles.y, 0); //Set rotation to look at nearest structure
            animator.SetBool("isGameWon", false);
        }

        DamageBuilding(structures);

        if (health <= 0)
        {
            StartCoroutine(death());
        }

    }
}
