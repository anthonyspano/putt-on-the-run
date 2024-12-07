using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoToNextHole : MonoBehaviour
{
    [SerializeField]
    public Dictionary<string, Vector3> holesReference = new Dictionary<string, Vector3>();
    public int currentHole = 1;

    void Start()
    {
        var holes = GameObject.FindGameObjectsWithTag("start");
        foreach(GameObject hole in holes)
        {
            holesReference.Add(hole.transform.parent.parent.name, hole.transform.position);
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(MovePlayerToNextHole());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            MovePlayerToPreviousHole();
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("cup"))
        {
            StartCoroutine(MovePlayerToNextHole());
        }
    }

    IEnumerator MovePlayerToNextHole()
    {
        yield return null;
        
        if(currentHole == 6)
            currentHole = 0;
        transform.position = holesReference["Hole" + GetNextHole().ToString()] + Vector3.up;
        ResetStrokeCount();
    }

    int GetNextHole()
    {   
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentHole++;
        return currentHole;
        
    }

    void MovePlayerToPreviousHole()
    {
        try
        {
            transform.position = holesReference["Hole" + (currentHole - 1).ToString()] + Vector3.up;
            ResetStrokeCount();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
         
    }

    void ResetStrokeCount()
    {
        GetComponent<PerformStroke>().NumberOfStrokesOnCurrentHole = 0;
    }

}
