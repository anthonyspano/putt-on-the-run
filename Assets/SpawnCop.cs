using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCop : MonoBehaviour
{
    private GoToNextHole _goToNextHole;
    public Dictionary<string, Vector3> copHolesReference = new Dictionary<string, Vector3>();

    public GameObject copPrefab;

    void Start()
    {
        _goToNextHole = GetComponent<GoToNextHole>();
        var holes = GameObject.FindGameObjectsWithTag("copstart");
        foreach(GameObject hole in holes)
        {
            copHolesReference.Add(hole.transform.parent.parent.name, hole.transform.position);
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("start"))
        {
            // get location of cop spawn
            string _currentHole = "Hole" + _goToNextHole.currentHole.ToString();
            Vector3 copSpawnPosition = copHolesReference[_currentHole] + Vector3.up;

            // spawn cop
            GameObject.Instantiate(copPrefab, copSpawnPosition, Quaternion.identity);

        }

    }
}
