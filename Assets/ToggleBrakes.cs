using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBrakes : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Slope"))
        {
            Debug.Log("brakes off");
            GetComponent<PerformStroke>().stopVelocityThreshold = 0;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Slope"))
        {
            Debug.Log("brakes on");
            GetComponent<PerformStroke>().stopVelocityThreshold = 0.5f;
        }
    }
}
