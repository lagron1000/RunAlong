using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicScript : MonoBehaviour
{
    private static MusicScript instance;

    public AudioClip opening;
    public AudioClip menuLoop;
    public AudioClip raceLoop;

    private AudioSource audioSource;

    private bool playing = true;
    private Scene currentScene;


    /**************************************************************
     * Awake is calles only once during the lifetime of the script instance
     **************************************************************/
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get the AudioSource component attached to the same GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    /**************************************************************
     * Starting menu music
     **************************************************************/
    private void PlayMenuClip()
    {
        // Play the second audio clip in a loop
        audioSource.Stop();
        audioSource.clip = menuLoop;
        audioSource.loop = true;
        audioSource.Play();
    }


    /**************************************************************
     * Starting race music
     **************************************************************/
    private void PlayRaceMusic()
    {
        audioSource.Stop();
        audioSource.clip = raceLoop;
        audioSource.loop = true;
        audioSource.Play();
    }


    /**************************************************************
     * Switches music on\off
     **************************************************************/
    public void SwitchMusic()
    {
        if (playing)
        {
            StopMusic();
        }
        else
        {
            StartMusic();
        }
    }


    /**************************************************************
     * Starts track according to scene
     **************************************************************/
    private void StartMusic()
    {
        playing = true;
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            PlayMenuClip();
        }
        else if (currentScene.name == "Track")
        {
            PlayRaceMusic();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }


    /**************************************************************
     * Stops music
     **************************************************************/
    private void StopMusic()
    {
        playing = false;
        audioSource.Stop();
        audioSource.clip = null;
    }


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        if (playing)
        {
            StartMusic();
        }
    }


    /**************************************************************
     * Update is called once per frame
     **************************************************************/
    private void Update()
    {
        Scene newScene = SceneManager.GetActiveScene();

        if (currentScene.name != newScene.name && playing)
        {
            StartMusic();
        }
    }
}
