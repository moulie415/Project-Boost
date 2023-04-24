using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelDelay = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    bool disableCollisions = false;

    AudioSource audioSource;

    bool isTransitioning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        RespondToDebugKeys();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || disableCollisions) return;
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartCoroutine(StartSuccessSequence());
                break;
            case "Fuel":
                break;
            default:
                StartCoroutine(StartCrashSequence());
                break;

        }
    }


    IEnumerator StartSuccessSequence()
    {
     
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Stop();
        successParticles.Play();
        // todo add partice FX
        GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(levelDelay);
        LoadNextLevel();
        
    }

    IEnumerator StartCrashSequence()
    {
 
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashParticles.Stop();
        crashParticles.Play();
        // todo add partice FX
        GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(levelDelay);
        ReloadLevel();
        
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }



    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            disableCollisions = !disableCollisions;
        }
    }
}
