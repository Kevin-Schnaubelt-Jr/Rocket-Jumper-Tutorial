using UnityEngine;
using UnityEngine.SceneManagement; // For SceneManager
using System.Collections; // For IEnumerator

public class CollisionHandler : MonoBehaviour
{
    Rigidbody rb;
    AudioSource[] audioSources;

    [SerializeField] float reloadLevelDelay = 5f;
    [SerializeField] float nextLevelDelay = 3f;

    [SerializeField] ParticleSystem crashParticles;

    public bool isCrashed = false;
    public bool isInputEnabled = true;
    public bool isColliding = false;
    public bool coroutineNextLevelEnabler = true;
    public bool coroutineReloadEnabler = true;
    
    void Start()
    {
        Debug.Log("CollisionHandler Start");
        rb = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
    }

    void OnCollisionEnter(Collision other) 
   {
        isColliding = true;
        
        if (isCrashed)
        {
            Debug.Log($"before sound, is crashed is {isCrashed}");
            PlayCrashSounds();
        }
        switch (other.gameObject.tag)
        {
            case "Good":
                Debug.Log("Launch pad collision");
                break;
            case "Finish":
                Debug.Log("Hit the finish line");
                if (coroutineNextLevelEnabler)
                {
                    coroutineNextLevelEnabler = false;
                    StartCoroutine(NextLevel());
                }
                break;
            default:
                if (coroutineReloadEnabler)
                {
                    coroutineReloadEnabler = false;
                    Debug.Log("IMPLOSION");
                    InitializeReloadLevel();
                }
                break;
        }
   }

    void OnCollisionExit(Collision other) 
    {
        isColliding = false;
    }

   IEnumerator ReloadLevel()
   {
        Debug.Log("Dying...");

        // Remove player control of the vessel, re-enable physics, init crash audio.
        isCrashed = true;
        Debug.Log($"Just set crashed to {isCrashed}");
        isInputEnabled = false;
        rb.constraints = RigidbodyConstraints.None;
        audioSources[0].Stop();
        audioSources[1].Play();
        crashParticles.Play();


        // Wait 3 seconds before reloading the level.
        yield return new WaitForSeconds(reloadLevelDelay);
        Debug.Log("Reloading level...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
   }

    public void InitializeReloadLevel()
   {
        StartCoroutine(ReloadLevel());
   }


   IEnumerator NextLevel()
   {
        yield return new WaitForSeconds(nextLevelDelay);
        Debug.Log($"RIGHT BEFORE THE DROP {isCrashed}");
        if (isCrashed == false && isColliding == true)
        {
            Debug.Log("Next level...");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log($"currentSceneIndex is {currentSceneIndex} and sceneCountInBuildSettings is {SceneManager.sceneCountInBuildSettings}");
            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(currentSceneIndex);
            }
            else
            {
                Debug.Log("You win!");
                SceneManager.LoadScene(0);
            }
        }
   }

   public void PlayCrashSounds()
   {
        System.Random rand = new System.Random();
        int index = rand.Next(2, 9);
        audioSources[index].Play();
   }
}
