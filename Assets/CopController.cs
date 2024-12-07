using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CopController : MonoBehaviour
{
    private float power;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //StartCoroutine(CompleteHole("Hole1"));
    }

    IEnumerator CompleteHole(string hole)
    {
        //yield return new WaitForSeconds(1.5f);
        yield return null;

        // fill list with waypoints from object which name matches passed string
        List<Vector3> waypoints = new List<Vector3>(); 
        //  find game object that matches string
        try
        {
            var parentObject = GameObject.Find("Hole1");
            for(int i=0; i<parentObject.transform.childCount; i++)
            {
                if(parentObject.transform.GetChild(i).name.Contains("waypoint"))
                    waypoints.Add(parentObject.transform.GetChild(i).position);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        // for each waypoint, perform stroke 
        foreach(Vector3 waypoint in waypoints)
        {
            // hit the ball in that direction with power appropriate to distance
            // gauge distance to waypoint
            var distanceToHit = Vector3.Distance(transform.position, waypoint);

            if(distanceToHit > 10)
                power = 3;
            if(distanceToHit > 5)
                power = 2;
            else
                power = 1;

            // hit the ball towards waypoint
            var direction = (waypoint - transform.position).normalized;
            direction.y = 0;
            rb.AddForce(direction * power, ForceMode.Impulse);

        }



    }
}
