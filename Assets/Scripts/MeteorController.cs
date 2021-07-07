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

    public Material targetMaterial;
    private Material baseMaterial;
    private Renderer[] renderers;

    public bool isGoldenMeteor = false;

    // Start is called before the first frame update
    void Start()
    {
        meteorRb = GetComponent<Rigidbody>();
        randomMeteorRotation = new Vector3(Random.Range(minRotate,maxRotate),Random.Range(minRotate,maxRotate),Random.Range(minRotate,maxRotate));
        
        removeMeteorPosZ = Camera.main.transform.position.z;

        renderers = GetComponentsInChildren<Renderer>();
        baseMaterial = renderers[0].material;
    }

    public void ResetMaterial()
    {
        if (renderers == null)
            return;

        foreach (Renderer rend in renderers)
        {
            rend.material = baseMaterial;
        }
    }

    public void SetTargetMaterial()
    {
        if (renderers == null)
            return;

        foreach (Renderer rend in renderers)
        {
            rend.material = targetMaterial;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < removeMeteorPosZ)
        {
            MeteorManager.Instance.aliveMeteor.Remove(gameObject);
            Destroy(gameObject);
        }
        
        Vector3 meteorMovementVector = new Vector3(0f,0f,-meteorSpeed * Time.deltaTime);
        meteorRb.velocity = meteorMovementVector;
        
        transform.Rotate(randomMeteorRotation * Time.deltaTime);
    }

    public void DestroyMeteor()
    {
        if (isGoldenMeteor == true && GameManager.Instance != null)
        {
            //add gold
            SaveManager.Instance.addGold();
        }
        
        //Remove from alive list
        MeteorManager.Instance.OnMeteorKill(gameObject);
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

    public void IncreaseSpeed(float speedIncrease)
    {
        meteorSpeed += speedIncrease;
    }
}
