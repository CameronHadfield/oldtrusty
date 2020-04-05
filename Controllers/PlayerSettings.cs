using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings thePlayer;
    // Start is called before the first frame update
    void Awake()
    {
        // Set a static reference to the player at all times
        thePlayer = this; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
