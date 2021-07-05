using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Joystick inputJoystick;
    

    public float strafeSpeed = 10f, 
                 hoverSpeed = 7.5f;

    private float activeStrafeSpeed,
                  activeHoverSpeed;

    private float strafeAcceleration = 2f,
                  hoverAcceleration = 2f;

    private Rigidbody playerRigidbody;
    private float minX, maxX, minY, maxY;

    private float maxRotation = 25f; 
    
    public GameObject misslesPrefabs;
    public Transform[] missleSpawnPoints;

    public int maxHealth = 5;
    private int currentHealth;
    public InGameManager inGameManager;

    private Vector3 raycastDirection = new Vector3(0f, 0f, 1f);
    public float raycastDistance = 100f;
    private int layerMask;
    
    private List<GameObject> previousTagets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        SetUpBoundries();
        currentHealth = maxHealth;

        layerMask = LayerMask.GetMask("EnemyRaycastLayer");
    }

    void Update()
    {
        ControlPlayer();
        RotatePlayer();
        
        CalculateBoundaries();
        FireRockets();
        RaycastForMeteor();
    }

    private void RaycastForMeteor()
    {
        List<GameObject> currentTargets = new List<GameObject>();
        foreach (Transform misslesSpawnPoint in missleSpawnPoints)
        {
            RaycastHit hit;
            Ray ray = new Ray(misslesSpawnPoint.position, raycastDirection);

            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                GameObject target = hit.transform.gameObject;
                currentTargets.Add(target);
            }
        }

        bool listsChange = false;
        //Check if the previous and current are the same
        if (currentTargets.Count != previousTagets.Count)
        {
            listsChange = true;
        }
        else
        {
            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i] != previousTagets[i])
                {
                    listsChange = true;
                }
            }
        }

        if (listsChange == true)
        {
            //Update Meteor
            MeteorManager.Instance.UpdateMeteors(currentTargets);
            
            previousTagets = currentTargets;
        }
        
    }
    
    //Fire Rockets
    public void FireRockets()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Transform rocketsSpawnPoint in missleSpawnPoints)
            {
                Instantiate(misslesPrefabs, rocketsSpawnPoint.position, misslesPrefabs.transform.rotation);
            }
            
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
    private void CalculateBoundaries()
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
        maxY = topCorners.y - objectHeight;
       
    }
    
    // Update is called once per frame
    
    private void ControlPlayer()
    {
        // Input on mobile
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed,inputJoystick.Horizontal, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed,inputJoystick.Vertical, hoverAcceleration * Time.deltaTime);
        
        //Input on PC
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
            OnPlayerDeath();
        }
    }

    public void OnPlayerDeath()
    {
        //play animation
        Debug.Log("Player Died");
    }
    
}
