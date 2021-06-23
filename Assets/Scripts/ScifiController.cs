using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScifiController : MonoBehaviour
{
    public float scifiSpeed = 20f;
    private Vector3 randomScifiRotation;
    private float minRotate = 0f, maxRotate = 100f;
    private float removeScifiPosZ;
    
    private Rigidbody scifiRb;
    // Start is called before the first frame update
    void Start()
    {
        scifiRb = GetComponent<Rigidbody>();
        removeScifiPosZ = Camera.main.transform.position.z - 1000f;
        randomScifiRotation = new Vector3(25,-36 ,Random.Range(minRotate,maxRotate));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < removeScifiPosZ)
        {
            Destroy(gameObject);
        }
        
        transform.Translate(Vector3.back * Time.deltaTime * scifiSpeed);
        transform.Rotate(randomScifiRotation * Time.deltaTime);
        
    }
}
