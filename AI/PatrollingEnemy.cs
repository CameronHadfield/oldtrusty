using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : HealthController
{
    // From the health controller, we already get a lot of the stuff
    // To take care of taking damage

    public Gun gun;

    public float sightRange = 20;

    [Range(0,90)]
    public float visionCone = 25;

    // This is the list of all places this guard will patrol to
    // If empty, the guard will remain stationary
    public Transform[] patrolNodes;

    // Something about sound listening behaviour

    public float distance;
    public float angleBetween;

    public bool IsPlayerInView(){
        Transform playerPos = PlayerSettings.thePlayer.transform;
        Vector3 vectorBetween = playerPos.position - transform.position;
        distance = vectorBetween.magnitude ;

        if(distance > sightRange) return false;

        angleBetween = Vector3.Angle(transform.forward, vectorBetween);

        if(Mathf.Abs(angleBetween) > visionCone) return false;

        if(Physics.Raycast(transform.position, 
            vectorBetween, 
            distance-1, 
            -1, 
            QueryTriggerInteraction.Ignore)){
                return false;
        }

        return true;
    }
    // Once the player is in view, we want to enable a reactive state, so we don't need to keep doing this entire calculation.
    // We might do a check to see if the player is behind somethign though, or out of range.

    // The AI should also try to move to the very edge of its shot range
    // Whenever possible
    void FixedUpdate(){
        if(IsPlayerInView()) {
            gun.Shoot(); 
        }
    }

}
