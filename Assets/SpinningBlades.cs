using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBlades : MonoBehaviour
{
    public float spinSpeed;

    void Update()
    {
        // spin object around z axis
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
