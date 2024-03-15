using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Platform_Disappearing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("FlashPlatform",2,2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
