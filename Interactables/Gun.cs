using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    // Get the location where shots come from
    public Transform shotOrigin;

    // This is the current vector from which 
    // Shot direction is derived
    public Vector3 currentShotVector;

    public float shotRange;

    public int magazineSize;
    public float reloadLength;

    public bool reloading;

    public LayerMask shootable = -1;

    public float damage;

    [Range(-5, 5)]
    public float damageDropoffModifier = 0;

    public float weaponSpread = 0;

    // Should return the transform of what it hit
    public virtual void Shoot(){
        RaycastHit hit = new RaycastHit();
        IHittable hittable = null;
        Vector3 randomFactor = Random.insideUnitCircle;
        Vector3 direction = 
            shotOrigin.forward 
            + randomFactor;

        Physics.Raycast(
            shotOrigin.position,
            shotOrigin.forward,
            out hit,
            shotRange,
            shootable,
            QueryTriggerInteraction.Ignore
        );

        float distance = (hit.transform.position - shotOrigin.transform.position).magnitude;

        // The totalDamage is equal to the damage 
        float totalDamage = damage - ((distance/shotRange) 
        * damage 
        * damageDropoffModifier);

        if ((hittable = hit.transform.GetComponent<IHittable>()) != null){
            // Cause the hittable to get hit;
            hittable.GetHit(hit.point, shotOrigin.forward, totalDamage);
        }
    }
    public virtual async void Reload(){
        reloading = true; 
        // This timer will run asynchronously
        System.Timers.Timer timer = 
            new System.Timers.Timer(reloadLength);
        
        timer.Enabled = true;

        reloading = false;
    }

    protected virtual void Update()
    {
        if(!reloading){
            if(Input.GetButtonDown("Fire 1")){
                this.Shoot();
            }
            if(Input.GetButtonDown("Reload")){
                // Begin the loading routine
                this.Reload();
            }
        }
    }
}
