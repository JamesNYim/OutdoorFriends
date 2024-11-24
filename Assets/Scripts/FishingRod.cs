using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class FishingRod : MonoBehaviour
{
    // I think eventually we should use the player as the transform for the fishing rod
    public Transform playerTransform;
    public bool isEquipped;
    public bool isFishingAvailable;

    public bool isCasted;
    public bool isReeling;

    private int castDistance = 10;

    Animator animator;
    public GameObject bobber;
    private Transform baitPosition;
    // Rope maybe?
    
    void Start()
    {
        animator = GetComponent<Animator>();
        isEquipped = true;
        Debug.Log("isEquipped: " + isEquipped);
    }

    // Update is called once per frame
    void Update()
    {
        canCast(isEquipped);
        if (isCasted && Input.GetKeyDown(KeyCode.R)) { ReelRod(); }
    }
    void canCast(bool isEquipped) {
        if (!isEquipped) {
            isFishingAvailable = false;
            return;
        }
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, castDistance)) {
            if (hit.collider.CompareTag("Water")) {
                isFishingAvailable = true;
                if (Input.GetMouseButtonDown(0) && !isCasted && !isReeling) {
                    StartCoroutine(CastRod(hit.point));
                }
            }
            else {
                isFishingAvailable = false;
            }
        }
        else {
            isFishingAvailable = false;
        }
    }

    IEnumerator CastRod(Vector3 castPosition) {
        Debug.Log("Is Casting");
        isCasted = true;
        animator.SetTrigger("CastingAnimation");
        
        // Delay between animation and bait appearing in water
        yield return new WaitForSeconds(1f);
        GameObject instantiatedBobber = Instantiate(bobber);
        instantiatedBobber.transform.position = castPosition;

        // Fish biting logic
    }

    private void ReelRod() {
        Debug.Log("Is Reeling");
        animator.SetTrigger("Reel");
        isCasted = false;
        isReeling = true;

        // Minigame logic
    }
}
