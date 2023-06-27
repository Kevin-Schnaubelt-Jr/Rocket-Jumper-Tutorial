using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float mainBoosters = 500f;
    [SerializeField] float rotationalBoosters = 200f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints =  RigidbodyConstraints.FreezePositionZ;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("THRUSTERS!");
            rb.AddRelativeForce(Vector3.up * mainBoosters * Time.deltaTime);
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Going Right!");
            ApplyRotation(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Going Left!");
            ApplyRotation(Vector3.forward);
        }  
    }

    void ApplyRotation(Vector3 direction)
    {
        rb.freezeRotation = true;
        transform.Rotate(direction * rotationalBoosters * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
