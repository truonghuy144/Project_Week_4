using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    
    public float speedShooting = 5f;
    private Rigidbody missileRb;
    
    // Start is called before the first frame update
    void Start()
    {
        missileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > MeteorManager.Instance.meteorSpawnDistance)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speedShooting);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            other.gameObject.GetComponent<MeteorController>().DestroyMeteor();
            ScoreManager.instance.AddPoint();
            Destroy(gameObject);
        }
    }
}
