using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceEnvironment : MonoBehaviour
{

    public GameObject gameObjectToInstantiate; //The environment GameObject

    public GameObject spawnedObject; //The spawned prefab of the environment
    private ARRaycastManager _arRaycastManager;
    public GameObject gameController;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>(); //List of all raycasts

    public bool toggleAREditMode = true;

    void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition) //Test to see if there is a touch, and if there is give the position of the touch
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true; //Return true if there is a touch
        }

        touchPosition = default;
        return false; //Return false if there is no touch
    }

    bool IsTouchOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    public void ToggleAREdit()
    {
        if (toggleAREditMode)
        {
            toggleAREditMode = false;
            gameController.GetComponent<SpawnTroop>().enabled = true;
        }
        else
        {
            toggleAREditMode = true;
            gameController.GetComponent<SpawnTroop>().enabled = false;
        }
    }


    void Update()
    {

        if (!IsTouchOverUI() && toggleAREditMode)
        {

            if (!TryGetTouchPosition(out Vector2 touchPosition)) //If no touches, break so that the next section of code is missed
                return;
            
            if (_arRaycastManager.Raycast(touchPosition, hits)) //Takes a touch position and the raycast hits
            {
                var hitPose = hits[0].pose; //Store the pose of the first hit and save it as the pose of the instantiated object


                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation); //Instantiate the environment
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position; //If the environment has already been spawned, just move it around
                    spawnedObject.transform.rotation = hitPose.rotation;
                }

            }
        }

    }

}
