using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// edit class so it only completes current hole when spawned 
public class CopController : MonoBehaviour
{
    private float power;
    private Rigidbody rb;

    public Dictionary<string, Vector3> holesReference = new Dictionary<string, Vector3>();
    public int currentHole = 1;

    bool scored;

    float stopVelocityThreshold = 0.5f;

    private GoToNextHole _goToNextHole;

    public GameObject failTextObject;

    void Start()
    {
        var holes = GameObject.FindGameObjectsWithTag("copstart");
        foreach(GameObject hole in holes)
        {
            holesReference.Add(hole.transform.parent.parent.name, hole.transform.position);
        }

        rb = GetComponent<Rigidbody>();
        _goToNextHole = GameObject.Find("Player").GetComponent<GoToNextHole>();
        currentHole = _goToNextHole.currentHole;
        StartCoroutine(CompleteHole(_goToNextHole.currentHole.ToString()));

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
            // if player.currentHole = currentHole, trigger fail state
            if(_goToNextHole.currentHole == currentHole)
            {
                Debug.Log("you lose");
                failTextObject = GameObject.Find("FailPrompt");
                failTextObject.GetComponent<Text>().enabled = true;
                StartCoroutine(FailState());
                
            }
            

            scored = true;
            
        }
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
            var parentObject = GameObject.Find("Hole" + hole.ToString());
            for(int i=0; i<parentObject.transform.childCount; i++)
            {
                if(parentObject.transform.GetChild(i).name.Contains("waypoint"))
                {
                    waypoints.Add(parentObject.transform.GetChild(i).position);
                    Debug.Log(parentObject.transform.GetChild(i).name + " is at " + parentObject.transform.GetChild(i).position);
                }
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
            Debug.Log("going to waypoint at: " + waypoint);

            var distanceToHit = Vector3.Distance(transform.position, waypoint);

            //Debug.Log("waypoint distance: " + distanceToHit);
            if(distanceToHit > 61)
                power = 1.5f;
            else if(distanceToHit > 42)
                power = 1.02f;
            else if(distanceToHit > 24)
                power = .73f;
            else if(distanceToHit > 10)
                power = .35f;
            else
                power = 0.1f;
         
            //Debug.Log(power);

            // hit the ball towards waypoint
            var direction = (waypoint - transform.position).normalized;
            direction.y = 0;
            Debug.Log(direction);
            rb.AddForce(direction * power, ForceMode.Impulse);

            yield return null;
            yield return null;

            yield return new WaitUntil(() => rb.velocity == Vector3.zero);


            lastPoint = waypoint;
            scored = false;
        }

        yield return new WaitForSeconds(2.5f);

        Debug.Log("finishing up");
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
                Debug.Log(direction);
                rb.AddForce(direction * power, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(3f);

            

        }

    }

    private Transform startDistance;
    private Transform endDistance;

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

    IEnumerator FailState()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        //yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        yield return new WaitForSeconds(2.5f);
    }


}

