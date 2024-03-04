using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisonHandler : MonoBehaviour
{
    [SerializeField] AudioClip sucsessSound;
    [SerializeField] AudioClip failureSound;
    [SerializeField] float nextLevelDelay = 2f;
    [SerializeField] ParticleSystem sucsessParticles;
    [SerializeField] ParticleSystem failureParticles;
    
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisonDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisonDisabled = !collisonDisabled;
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisonDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This object is friendly!");
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            case "Fuel":
                Debug.Log("This object is the fuel!");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartNextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(sucsessSound);
        sucsessParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", nextLevelDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(failureSound);
        failureParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
