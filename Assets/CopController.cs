using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CopController : MonoBehaviour
{
    private float power;
    private Rigidbody rb;

    public Dictionary<string, Vector3> holesReference = new Dictionary<string, Vector3>();
    public int currentHole = 1;

    bool scored;

    float stopVelocityThreshold = 0.5f;

    void Start()
    {
        var holes = GameObject.FindGameObjectsWithTag("copstart");
        foreach(GameObject hole in holes)
        {
            holesReference.Add(hole.transform.parent.parent.name, hole.transform.position);
        }

        rb = GetComponent<Rigidbody>();
        StartCoroutine(CompleteHole("Hole1"));
        //GetDistance();
    }

    void Update()
    {
        if(BallIsResting() && IsGrounded())
            rb.velocity = Vector3.zero;
    }

    bool BallIsResting()
    {
        return rb.velocity.magnitude < stopVelocityThreshold;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("cup"))
        {
            scored = true;
            StartCoroutine(MovePlayerToNextHole());
        }
    }

    IEnumerator MovePlayerToNextHole()
    {
        yield return null;
        
        if(currentHole == 6)
            currentHole = 0;
        transform.position = holesReference["Hole" + GetNextHole().ToString()] + Vector3.up;

        scored = false;
    }

    int GetNextHole()
    {   
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentHole++;
        return currentHole;
        
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
            var parentObject = GameObject.Find(hole);
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

        Vector3 lastPoint = Vector3.zero;

        // for each waypoint, perform stroke 
        foreach(Vector3 waypoint in waypoints)
        {
            // hit the ball in that direction with power appropriate to distance
            // gauge distance to waypoint
            var distanceToHit = Vector3.Distance(transform.position, waypoint);

            if(distanceToHit > 61)
                power = 1.5f;
            if(distanceToHit > 42)
                power = 1.02f;
            if(distanceToHit > 24)
                power = .75f;
            else
                power = 0.1f;
         
            Debug.Log(power);

            // hit the ball towards waypoint
            var direction = (waypoint - transform.position).normalized;
            direction.y = 0;
            rb.AddForce(direction * power, ForceMode.Impulse);

            yield return null;

            yield return new WaitUntil(() => rb.velocity == Vector3.zero);


            lastPoint = waypoint;
        }

        yield return null;

        while(!scored)
        {
            var distanceToHit = Vector3.Distance(transform.position, lastPoint);

            if(distanceToHit > 61)
                power = 1.5f;
            if(distanceToHit > 42)
                power = 1.02f;
            if(distanceToHit > 24)
                power = .75f;
            else
                power = 0.1f;

            if(rb.velocity == Vector3.zero)
            {
                var direction = (lastPoint - transform.position).normalized;
                direction.y = 0;
                rb.AddForce(direction * power, ForceMode.Impulse);
            }

            yield return null;

        }

        
        StartCoroutine(CompleteHole("Hole" + currentHole.ToString()));

    }

    public Transform startDistance;
    public Transform endDistance;

    void GetDistance()
    {
        Debug.Log(Vector3.Distance(startDistance.position, endDistance.position));
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Slope"))
        {
            Debug.Log("brakes off");
            stopVelocityThreshold = 0;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Slope"))
        {
            Debug.Log("brakes on");
            stopVelocityThreshold = 0.5f;
        }
    }


}

