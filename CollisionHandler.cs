using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f; // sets the level load delay to 2 seconds
    [SerializeField] AudioClip FinishSound; 
    [SerializeField] AudioClip CrashSound;
    [SerializeField] ParticleSystem FinishParticles; 
    [SerializeField] ParticleSystem CrashParticles;
    AudioSource RocketAudio;

    bool isTransitioning = false;
    bool collisionDisabled = false;
    void Start()
    {
        RocketAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggles collision
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning ||collisionDisabled) { return; } // returns if the rocket is transitioning

        switch (other.gameObject.tag) // switch statement to check the tag of the object that the rocket collides with
        {
            case "Friendly":
                print("This thing is friendly!");
                break;
            case "Finish":
                LandingSequence(); // calls the LandingSequence method when the rocket collides with the finish line
                break;
            case "Fuel":
                print("This thing is fuel!");
                break;
            default:
                StartCrashSequence(); 
                break;
        }

    }

        void LandingSequence()
        {
            isTransitioning = true;
            RocketAudio.Stop(); 
            FinishParticles.Play(); // plays the finish particles when the rocket collides with the finish line
            GetComponent<Movement>().enabled = false; // disables the movement script when the rocket collides with the finish line
            RocketAudio.PlayOneShot(FinishSound); // plays the finish sound when the rocket collides with the finish line
            Invoke("LoadNextLevel", LevelLoadDelay); // loads the next level after 2 seconds when the rocket collides with the finish line
        }

        void StartCrashSequence()
        {
            isTransitioning = true; 
            RocketAudio.Stop();
            CrashParticles.Play(); // plays the crash particles when the rocket collides with anything other than the friendly object
            GetComponent<Movement>().enabled = false; // disables the movement script when the rocket collides with anything other than the friendly object
            RocketAudio.PlayOneShot(CrashSound); // plays the crash sound when the rocket collides with anything other than the friendly object
            Invoke("ReloadLevel", 1f); // reloads the level after 1 second when the rocket collides with anything other than the friendly object
        }


        void ReloadLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // gets the current scene index
            SceneManager.LoadScene(currentSceneIndex); // reloads the scene when the rocket collides with anything other than the friendly object
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



