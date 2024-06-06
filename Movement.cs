using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody RocketRigidBody;
    [SerializeField] float RocketThrust = 100f;
    [SerializeField] float RocketRotation = 1f;

    // Start is called before the first frame update
    void Start()
    {
        RocketRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      ProcessThurst();
      ProcessRotation();
    }

    void ProcessThurst()
    {
       if (Input.GetKey(KeyCode.Space)) // can thrust while pressing space bar
       {
           RocketRigidBody.AddRelativeForce(Vector3.up * RocketThrust * Time.deltaTime); // Makes the rocket go up but also frame rate independent
       }
        
    }
    void ProcessRotation()
    {
       if (Input.GetKey(KeyCode.A)) 
        {
            ApplyRotation(RocketRotation);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-RocketRotation);
        } 
    }

    void ApplyRotation(float rotationThisFrame)
    {
        RocketRigidBody.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        RocketRigidBody.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
