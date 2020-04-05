using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // Start is called before the first frame update

    public Gun heldGun;

    // Update is called once per frame
    void Update()
    {
        if(heldGun.continuousFire){
            if(Input.GetButton("Fire1")){
                heldGun.Shoot();
            }
        }
        else{ 
            if(Input.GetButtonDown("Fire1")){
                heldGun.Shoot();
            }
        }
        if(Input.GetButtonDown("Reload")){
            // Begin the loading routine
            heldGun.Reload();
        }
    }
}
