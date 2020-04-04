using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;

public class Gun : MonoBehaviour
{

    // Get the location where shots come from
    public Transform shotOrigin;

    public float shotRange = 15f;

    public int magazineSize;
    public float reloadLength;

    public bool reloading;

    public float timeBetweenShots = 0.2f;
    public bool continuousFire = false;

    public LayerMask shootable = -1;

    public float damage;

    [Range(-5, 5)]
    public float damageDropoffModifier = 0;

    public float weaponSpread = 0;
    private bool canShoot;
    private float shotTimer;

    // Should return the transform of what it hit

    public virtual void Shoot(){
        RaycastHit hit = new RaycastHit();
        IHittable hittable = null;
        Vector3 randomFactor = Random.insideUnitCircle * weaponSpread / shotRange;
        Vector3 direction = 
            shotOrigin.forward;

        Physics.Raycast(
            shotOrigin.position,
            direction,
            out hit,
            shotRange,
            shootable,
            QueryTriggerInteraction.Ignore
        );
        if(Globals.GlobalVars.debugMode){
            if( hit.collider ){
                Debug.DrawRay(shotOrigin.position, 
                direction * shotRange, 
                Color.yellow, 
                5000);

            }
            else{
                Debug.DrawRay(shotOrigin.position, 
                direction * shotRange, 
                Color.red, 
                5000);
            }
        }

        if (hit.transform && (hittable = hit.transform.GetComponent<IHittable>()) != null){

            float distance = (hit.transform.position - shotOrigin.transform.position).magnitude;

            // The totalDamage is equal to the damage 
            float totalDamage = damage - ((distance/shotRange) 
            * damage 
            * damageDropoffModifier);
            // Cause the hittable to get hit;
            hittable.GetHit(hit.point, shotOrigin.forward, totalDamage);
        }
    }
    public virtual async void Reload(){
        reloading = true; 
        // This timer will run asynchronously
        System.Timers.Timer timer = 
            new System.Timers.Timer(reloadLength);
        timer.Start();

        reloading = false;
    }

    public void CheckShotTimer(){
        if(Time.time >= shotTimer)  {
            canShoot = true;
        }
    }
    public void StartShotTimer(){
        shotTimer = Time.time + timeBetweenShots;
        canShoot = false;
    }

    protected virtual void FixedUpdate()
    {
        this.CheckShotTimer();
        if(!reloading){
            if(continuousFire){
                if(canShoot && Input.GetButton("Fire1")){
                    this.Shoot();
                    this.StartShotTimer();
                }
            }
            else{ 
                if(canShoot && Input.GetButtonDown("Fire1")){
                    this.Shoot();
                    this.StartShotTimer();
                }
            }
            if(Input.GetButtonDown("Reload")){
                // Begin the loading routine
                this.Reload();
            }
        }
    }
}
