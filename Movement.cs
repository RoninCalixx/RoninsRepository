using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    Rigidbody RocketRigidBody;
    AudioSource RocketAudio;
    [SerializeField] float RocketThrust = 100f;
    [SerializeField] float RocketRotation = 1f;
    [SerializeField] AudioClip RocketThrustSound; 
    [SerializeField] ParticleSystem RocketThrustParticles;
    [SerializeField] ParticleSystem RightThrustParticles;
    [SerializeField] ParticleSystem LeftThrustParticles;

    // Start is called before the first frame update
    void Start()
    {
        RocketRigidBody = GetComponent<Rigidbody>();
        RocketAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      ProcessThrust();
      ProcessRotation();
      DebugKeys();
    }

    void ProcessThrust()
    {
       if (Input.GetKey(KeyCode.Space)) // can thrust while pressing space bar
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
       if (Input.GetKey(KeyCode.A))
        {
            ApplyLeftRotation();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRightRotation();
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) // stops the particles when the A or D key is not pressed
        {
            StopRotating();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
           LoadNextLevel(); 
        }
    }    




    void StartThrusting()
    {
        RocketRigidBody.AddRelativeForce(Vector3.up * RocketThrust * Time.deltaTime); // Makes the rocket go up but also frame rate independent
        if (!RocketAudio.isPlaying) // so it doesn't layer
        {
            RocketAudio.PlayOneShot(RocketThrustSound); // plays the audio when the space bar is pressed
        }
        if (!RocketThrustParticles.isPlaying)
        {
            RocketThrustParticles.Play(); // plays the particles when the space bar is pressed
        }
    }

    void StopThrusting()
    {
        RocketAudio.Stop(); // stops the audio when the space bar is not pressed
        RocketThrustParticles.Stop(); // stops the particles when the space bar is not pressed
    }

    void ApplyLeftRotation()
    {
        ApplyRotation(RocketRotation);
        if (!RightThrustParticles.isPlaying)
        {
            RightThrustParticles.Play();
        }
    }

    void ApplyRightRotation()
    {
        ApplyRotation(-RocketRotation);
        if (!LeftThrustParticles.isPlaying)
        {
            LeftThrustParticles.Play();
        }
    }

    void StopRotating()
    {
        RightThrustParticles.Stop();
        LeftThrustParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        RocketRigidBody.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        RocketRigidBody.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }

public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // gets the current scene index
            int nextSceneIndex = currentSceneIndex + 1; // gets the next scene index
            if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) // checks if the next scene index is the last scene
            {
                nextSceneIndex = 0; // sets the next scene index to the first scene
            }
            SceneManager.LoadScene(nextSceneIndex); // loads the next scene when the rocket collides with the finish line
        }


}
