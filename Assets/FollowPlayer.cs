using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ultimate2d.combat
{
    

    public class FollowPlayer : MonoBehaviour
    {
        //public float followSpeed;
        private Vector3 offset = new Vector3(30f, 08f, 1f);
        private float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;
        [SerializeField] private Transform target;

        public float rotateSpeed;
        public float zoomSpeed;

        void Update()
        {
            Vector3 targetPosition = target.position + offset;
            //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // maintain max/min distance for camera
            if(Vector3.Distance(transform.position, target.position) > 40)
                transform.position = Vector3.MoveTowards(transform.position, target.position, 100f * Time.deltaTime);


            // rotate around target
            transform.LookAt(target);
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime);
            
            // zoom controls for camera
            if(Input.GetAxis("Vertical") != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, Input.GetAxis("Vertical") * zoomSpeed * Time.deltaTime);

            }

            if(Input.GetKey(KeyCode.Z))
            {
                // move camera up
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), zoomSpeed * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.X))
            {
                // move camera down
                transform.position = Vector3.MoveTowards(transform.position, transform.position - new Vector3(0, 1, 0), zoomSpeed * Time.deltaTime);
            }
            
        }
    }

}