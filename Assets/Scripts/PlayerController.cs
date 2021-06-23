using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float strafeSpeed = 10f, 
                 hoverSpeed = 7.5f;

    private float activeStrafeSpeed,
                  activeHoverSpeed;

    private float strafeAcceleration = 2f,
                  hoverAcceleration = 2f;

    private Rigidbody playerRb;
    private float minX, maxX, minY, maxY;

    private float maxRotation = 25f; 
    
    public GameObject misslesPrefabs;
    public Transform missleSpawnPos1, missleSpawnPos2;

    public int maxHealth = 5;
    private int currentHealth;
    public InGameManager inGameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        SetUpBoundries();
        currentHealth = maxHealth;
    }

    void Update()
    {
        ControlPlayer();
        RotatePlayer();
        
        CalculateBoundries();
        FireRockets();
    }
    
    //Fire Rockets
    private void FireRockets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(misslesPrefabs, missleSpawnPos1.position, misslesPrefabs.transform.rotation);
            Instantiate(misslesPrefabs, missleSpawnPos2.position, misslesPrefabs.transform.rotation);
        }
    }

    // Rotate Player
    private void RotatePlayer()
    {
        float currentX = transform.position.x;
        float newRotationZ;

        if (currentX < 0)
        {
            //Rotate negative
            newRotationZ = Mathf.Lerp(0f, - maxRotation, currentX / minX);
        }
        else
        {
            //Rotate positive
            newRotationZ = Mathf.Lerp(0f, maxRotation, currentX / maxX);
        }
        
        Vector3 currentRotationVector3 = new Vector3(0f,0f,newRotationZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;
    }
    
    // Calculate Bound Screen
    private void CalculateBoundries()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        transform.position = currentPosition;
    }

    // Set Bound
    private void SetUpBoundries()
    {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorners = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, camDistance));
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, camDistance));

        //calculate the size of GameObject
        Bounds gameObjectBounds = GetComponent<Collider>().bounds;
        float objectWidth = gameObjectBounds.size.x;
        float objectHeight = gameObjectBounds.size.y;
        
        minX = bottomCorners.x + objectWidth;
        maxX = topCorners.x - objectWidth;

        minY = bottomCorners.y - objectHeight;
        maxY = topCorners.y - objectHeight * 4.5f;
       
    }
    
    // Update is called once per frame
    
    private void ControlPlayer()
    {
        //
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,Input.GetAxisRaw("Horizontal"), strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,Input.GetAxisRaw("Hover"), hoverAcceleration * Time.deltaTime);
        
        //
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime * strafeSpeed
                               + transform.up * activeHoverSpeed * Time.deltaTime * hoverSpeed);
    }

    public void OnMeteorImpact()
    {
        currentHealth--;
        
        //change health bar
        inGameManager.ChangeHealthBar(maxHealth, currentHealth);
        
        
        if (currentHealth == 0) // called once
        {
            
        }
    }

    public void OnPlayerDeath()
    {
        //play animation
        Debug.Log("Player Died");
    }
    
}
