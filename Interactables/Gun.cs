using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;
using System.Timers;
using System;

public class Gun : MonoBehaviour
{
    // Get the location where shots come from

    public GameObject impact;
    public GameObject muzzleFlash;

    public Transform shotOrigin;
    public Transform muzzleFlashOrigin;

    public float shotRange = 15f;

    public int currentBullets = 10;
    public int magazineSize = 10;
    public float reloadLength;

    public bool reloading;

    public float timeBetweenShots = 0.2f;
    
    // this only matters for the player
    public bool continuousFire = false;

    public LayerMask shootable = -1;

    public float damage;

    [Range(-5, 5)]
    public float damageDropoffModifier = 0;

    public float weaponSpread = 0;
    public bool canShoot;
    private float shotTimer;

    public bool autoReload = true;
    // Should return the transform of what it hit

    public virtual void Shoot(){
        CheckShotTimer();
        if(!canShoot || reloading){
            return;
        }
        if(currentBullets <= 0) {
            if(autoReload && !reloading){
                this.Reload();
            }
            return;
        }

        // We are going to fire
        // Muzzle flash
        this.OnShootEffect();
        this.StartShotTimer();

        RaycastHit hit = new RaycastHit();
        IHittable hittable = null;
        Vector3 randomFactor = UnityEngine.Random.insideUnitCircle * weaponSpread / shotRange;
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
        currentBullets -=1;
        if(Globals.GlobalVars.debugMode){
            if( hit.collider ){
                Debug.DrawRay(shotOrigin.position, 
                direction * shotRange, 
                Color.yellow, 
                5);

            }
            else{
                Debug.DrawRay(shotOrigin.position, 
                direction * shotRange, 
                Color.red, 
                5);
            }
        }

        // generalOnHit
            // Might want to separate this into its own thing

        if (hit.transform && (hittable = hit.transform.GetComponent<IHittable>()) != null){
            this.Damage(hittable, hit);
        }
        else if(hit.collider){
            this.OnHitEffect(hit);
        }
    }

    public virtual void OnShootEffect()
    {
        Vector3 pos = muzzleFlashOrigin.position;
        GameObject.Destroy(Spawn(
                muzzleFlash,
                pos, 
                Quaternion.LookRotation(muzzleFlashOrigin.forward),
                muzzleFlashOrigin), 
            0.02f);
    }

    public virtual void OnHitEffect(RaycastHit hit){
        Collider coll = hit.collider;
        Destroy(
            this.Spawn(
                impact, 
                hit.point, 
                Quaternion.LookRotation(hit.normal),
                hit.transform),
            5f
        );
        //Make a sound
    }
    public virtual void Damage(IHittable hittable, RaycastHit hit){
        float distance = (hit.transform.position - shotOrigin.transform.position).magnitude;

        // The totalDamage is equal to the damage 
        float totalDamage = damage - ((distance/shotRange) 
        * damage 
        * damageDropoffModifier);
        // Cause the hittable to get hit;
        hittable.GetHit(hit.point, shotOrigin.forward, totalDamage);
    }
    public virtual void Reload(){
        reloading = true; 
        canShoot = false;
        // This timer will run asynchronously
        System.Timers.Timer timer = 
            new System.Timers.Timer(reloadLength * 1000);
        timer.Start();

        timer.Elapsed += delegate{
            reloading = false;
            canShoot = true;
            currentBullets = magazineSize;
            timer.Stop();
        };
    }

    public virtual GameObject Spawn (GameObject obj, Vector3 position, Quaternion direction, Transform parent = null) {
        GameObject go = Instantiate(obj, position, direction, parent);
        if(parent && parent != null){
            go.transform.localScale = new Vector3(
                1/go.transform.lossyScale.x, 
                1/go.transform.lossyScale.y, 
                1/go.transform.lossyScale.z
            );
        }
        return go;
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
}
