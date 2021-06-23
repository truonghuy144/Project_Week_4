using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorController : MonoBehaviour
{
    public float meteorSpeed = 20f;
    private Vector3 randomMeteorRotation;
    private float minRotate = 0f, maxRotate = 100f;
    private float removeMeteorPosZ;
    
    private Rigidbody meteorRb;
    
    // Start is called before the first frame update
    void Start()
    {
        meteorRb = GetComponent<Rigidbody>();
        randomMeteorRotation = new Vector3(Random.Range(minRotate,maxRotate),Random.Range(minRotate,maxRotate),Random.Range(minRotate,maxRotate));
        
        removeMeteorPosZ = Camera.main.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < removeMeteorPosZ)
        {
            Destroy(gameObject);
        }
        
        Vector3 meteorMovementVector = new Vector3(0f,0f,-meteorSpeed * Time.deltaTime);
        meteorRb.velocity = meteorMovementVector;
        
        transform.Rotate(randomMeteorRotation * Time.deltaTime);
    }

    public void DestroyMeteor()
    {
        //play partical fx
        
        //disable movement
        
        //disable colliders
        
        //destroy gameobject with a delay
        Destroy(gameObject); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().OnMeteorImpact();
            DestroyMeteor();
        }
    }
}
