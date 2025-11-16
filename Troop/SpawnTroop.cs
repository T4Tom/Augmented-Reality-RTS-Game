using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System;
using Unity.VisualScripting;


public class SpawnTroop : MonoBehaviour
{

    [SerializeField] private Camera Camera;
    [SerializeField] private GameObject spawnParticlePrefab;

    public float maxHealth;
    public float health;

    [SerializeField] private GameObject troopAPrefab, troopBPrefab, troopCPrefab;
    public ARTapToPlaceEnvironment aRTapToPlaceEnvironment;
    public PlayerSelector playerSelector;
    private Vector2 touchPosition; //The position of the player's touch

    private Ray ray;

    private GameObject troop1, troop2, troop3;


    void Awake()
    {
        troopAPrefab.GetComponent<TroopController>().surface = aRTapToPlaceEnvironment.spawnedObject.GetComponentInChildren<NavMeshSurface>();
        troopBPrefab.GetComponent<TroopController>().surface = aRTapToPlaceEnvironment.spawnedObject.GetComponentInChildren<NavMeshSurface>();
        troopCPrefab.GetComponent<TroopController>().surface = aRTapToPlaceEnvironment.spawnedObject.GetComponentInChildren<NavMeshSurface>();   
    }


    bool IsTouchOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void InstantiateTroop(GameObject troopVariable, GameObject troopPrefab)
    {
        RaycastHit hit;
        ray = Camera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (troopVariable == null) //If the troop has not yet been spawned
            {
                troopVariable = Instantiate(troopPrefab, aRTapToPlaceEnvironment.spawnedObject.transform); //Spawn the troop as a child of the environment
                troopVariable.GetComponent<TroopController>().surface = aRTapToPlaceEnvironment.spawnedObject.GetComponentInChildren<NavMeshSurface>(); //Assigning the navmesh to the troop
                troopVariable.transform.rotation = aRTapToPlaceEnvironment.spawnedObject.transform.rotation; //Set rotation to environment rotation
                troopVariable.transform.position = hit.point; //Set position to tap position
                troopVariable.GetComponent<NavMeshAgent>().enabled = true;
                Instantiate(spawnParticlePrefab, hit.point, default);
            }
        }
    }


    bool TryGetTouchPosition() //Test to see if there is a touch, and if there is give the position of the touch
    {
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true; //Return true if there is a touch
        }
    
        return false; //Return false if there is no touch
    }


    void Update()
    {

        if (!IsTouchOverUI())
        {
            
            if (aRTapToPlaceEnvironment.toggleAREditMode == false)
            {


                if (!TryGetTouchPosition())
                    return;

                touchPosition = Input.GetTouch(0).rawPosition;

                if (playerSelector.buttonsSelected[0] == true)
                {
                    InstantiateTroop(troop1, troopAPrefab);
                }
                if (playerSelector.buttonsSelected[1] == true)
                {
                    InstantiateTroop(troop2, troopBPrefab);
                }
                if (playerSelector.buttonsSelected[2] == true)
                {
                    InstantiateTroop(troop3, troopCPrefab);
                }
            }

        }

    }

}