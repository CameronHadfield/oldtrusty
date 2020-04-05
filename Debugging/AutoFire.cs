using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFire : MonoBehaviour
{
    public Gun gun;
    // Update is called once per frame
    void FixedUpdate()
    {
        gun.Shoot();
    }
}
