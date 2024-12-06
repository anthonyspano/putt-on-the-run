using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Ground"))
        { 
            // stop momentum of ball
            GetComponent<Rigidbody>().velocity = Vector3.zero; 


            // set position to current hole's starting point
            transform.position = GetComponent<GoToNextHole>().holesReference["Hole" + GetComponent<GoToNextHole>().currentHole.ToString()] + Vector3.up;
        }
    }
}
