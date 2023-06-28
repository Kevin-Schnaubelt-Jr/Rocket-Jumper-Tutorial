using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CollisionHandler collisionHandler;
    Rigidbody rb;
    AudioSource[] audioSources;
    [SerializeField] ParticleSystem boostUpParticles;
    [SerializeField] ParticleSystem boostRightParticles;
    [SerializeField] ParticleSystem boostLeftParticles;
    [SerializeField] float mainBoosters = 500f;
    [SerializeField] float rotationalBoosters = 200f;
    [SerializeField] int crashDelta = 2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Movement Start");
        rb = GetComponent<Rigidbody>();
        rb.constraints =  RigidbodyConstraints.FreezePositionZ;
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the z-rotation of the vessel
        float zRotation = transform.eulerAngles.z;

        // Check if the z-rotation is close to 90 degrees
        if ((zRotation > 90 - crashDelta && zRotation < 270 + crashDelta) && collisionHandler.isCrashed == false && collisionHandler.isColliding)        
        {
            // If so, start the ReloadLevel coroutine in the CollisionHandler component
            Debug.Log("FELL!");
            if (collisionHandler.coroutineReloadEnabler == true)
            {
                collisionHandler.coroutineReloadEnabler = false;
                collisionHandler.InitializeReloadLevel();
            }
        }

        if (collisionHandler.isInputEnabled)
        {
            ProcessThrust();
            ProcessRotation();
        }
        else
        {
            boostUpParticles.Stop();
            boostRightParticles.Stop();
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            audioSources[0].Stop();
            boostUpParticles.Stop();
        }
    }

    private void Thrust()
    {
        // Debug.Log("THRUSTERS!");
        if (!audioSources[0].isPlaying)
        {
            audioSources[0].Play();
            boostUpParticles.Play();
        }
        rb.AddRelativeForce(Vector3.up * mainBoosters * Time.deltaTime);
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            // Debug.Log("Going Right!");
            ApplyRotation(Vector3.back);
            if (!boostRightParticles.isPlaying)
            {
                boostRightParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            // Debug.Log("Going Left!");
            ApplyRotation(Vector3.forward);
            if (!boostLeftParticles.isPlaying)
            {
                boostLeftParticles.Play();
            }
        }
        else
        {
            boostLeftParticles.Stop();
            boostRightParticles.Stop();
        }
    }

    void ApplyRotation(Vector3 direction)
    {
        rb.freezeRotation = true;
        transform.Rotate(direction * rotationalBoosters * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
