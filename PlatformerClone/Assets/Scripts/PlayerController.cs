using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/31/2023]
 * [Handles the functionality of the player, including collision interaction, movement, and raycasting]
 */
public class PlayerController : MonoBehaviour
{

    public int speed = 0; 
    public int jumpForce = 10;
    public float shootingCooldown = 0.5f;


    public int totalCoins = 0;

 
    public int health = 99;
    public int fallDepth;
    private Vector3 startPosition;

    public float stunTimer;

    private Rigidbody rigidBody;
    private Transform gunBarrel;
    public GameObject regularProjectilePrefab;
    public GameObject heavyProjectilePrefab;
    private Renderer playerRenderer;

    public bool isInvincible = false;
    public bool isGrounded;
    public bool facingLeft;
    public bool shootingDebounce = false;
    public bool heavyLaserEnabled = false;



    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; 
        rigidBody= GetComponent<Rigidbody>();
        gunBarrel = transform.Find("gun_barrel");
        facingLeft = false;
        playerRenderer = GetComponent<Renderer>();
        SetupLaserCollisions();
    }





    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        SpaceJump();
    }





    /// <summary>
    /// adds a mesh collider and rigidbody to all untagged or "Terrain" tagged game objects in the scene.
    /// skips over objects with the name "Directional Light" and "SpawnPoint" because they are not visible objects.
    /// this is so that lasers delete when hitting the floors or walls.
    /// </summary>
    void SetupLaserCollisions()
    {
        GameObject[] allGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        int total_terrain_parts = 0;

        foreach (GameObject thisObject in allGameObjects)
        {
            if (thisObject.name == "Directional Light" || thisObject.name == "SpawnPoint")
            {
                continue;
            }
            // check to see if the object is a parent that has parts underneath it
            // if it has children, edit the children instead of the empty parent
            // otherwise know it's an independent object so we just edit the individual instead

            if (thisObject.transform.childCount > 0)
            {
                //print("found a parent instance");
                for (int i = 0; i < thisObject.transform.childCount; i++)
                {

                    GameObject child = thisObject.transform.GetChild(i).gameObject;
                    if (child.name == "Directional Light" || child.name == "SpawnPoint")
                    {
                        continue;
                    }
                    if (child.tag == "Untagged" || child.tag == "Terrain")
                    {
                        SetupCollisionFor(child);
                        total_terrain_parts++;
                    }
                }
            }
            else if (thisObject.tag == "Untagged" || thisObject.tag == "Terrain")
            {
                SetupCollisionFor(thisObject);
                total_terrain_parts++;
            }
        }

        print("Found "+ total_terrain_parts + " pieces of terrain");
    }





    /// <summary>
    /// takes the given gameobject and applies a mesh collider and rigidbody if the gameObject does not have them already.
    /// also turns on the IsTrigger() for the box collider
    /// this is for the player and laser collisions to work properly
    /// </summary>
    void SetupCollisionFor(GameObject givenObject)
    {
        if (givenObject.GetComponent<BoxCollider>() == null)
            givenObject.AddComponent<BoxCollider>();

        givenObject.GetComponent<BoxCollider>().isTrigger = true;


        if (givenObject.GetComponent<Rigidbody>() == null)
            givenObject.AddComponent<Rigidbody>();

        givenObject.GetComponent<Rigidbody>().useGravity = false;
        givenObject.GetComponent<Rigidbody>().isKinematic = true;
        givenObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


        if (givenObject.GetComponent<MeshCollider>() == null)
            givenObject.AddComponent<MeshCollider>();

        givenObject.GetComponent<MeshCollider>().convex = true;
    }





    /// <summary>
    /// handles the player movement
    /// also checks to see if the player fell off the map
    /// </summary>
    private void Move()
    {

        Vector3 add_position = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            //print("Move the player left");
            transform.position += Vector3.left * speed * Time.deltaTime;
            //add_position += Vector3.left * speed * Time.deltaTime;
            gunBarrel.localPosition = new Vector3(-1.125f, 0.25f, 0f);
            facingLeft = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            //print("Move the player right");
            transform.position += Vector3.right * speed * Time.deltaTime;
            //add_position += Vector3.right * speed * Time.deltaTime;
            gunBarrel.localPosition = new Vector3(1.125f, 0.25f, 0f);
            facingLeft = false;

        }

        //transform.position += add_position;

        if (transform.position.y < fallDepth) 
        {
            Respawn();
        }

        CheckForDanger();
    }





    /// <summary>
    /// shoots a laser. laser damage is stronger depending on pickup
    /// </summary>
    private void Shoot()
    {
        if (shootingDebounce == true)
        {
            return;
        }

        // getting the direction of where the laser must go
        // if the player is pressing enter, shoot where the character is facing
        // if the player is using left or right arrow keys, shoot correspondingly to those directions
        // if none of those keys are pressed, don't shoot

        bool shootingLeft = false;

        if (Input.GetKey(KeyCode.Return))
            shootingLeft = facingLeft;
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                shootingLeft = true;
            else if (Input.GetKey(KeyCode.RightArrow))
                shootingLeft = false;
            else
            {
                // no key was pressed
                return;
            }
        }

        StartCoroutine(ShootDebounce());

        if (heavyLaserEnabled == true)
        {
            GameObject projectile = Instantiate(heavyProjectilePrefab, transform.position, heavyProjectilePrefab.transform.rotation);
            if (projectile.GetComponent<PlayerLaser>())
            {
                projectile.GetComponent<PlayerLaser>().goingLeft = shootingLeft;
            }
        }
        else
        {
            GameObject projectile = Instantiate(regularProjectilePrefab, transform.position, regularProjectilePrefab.transform.rotation);
            if (projectile.GetComponent<PlayerLaser>())
            {
                projectile.GetComponent<PlayerLaser>().goingLeft = shootingLeft;
            }
        }
        
    }





    /// <summary>
    /// prevents the lasers from being spammed by setting debounce to true, which stops the bullets from shooting
    /// resets the debounce after x seconds has passed
    /// </summary>
    /// <returns></returns>
    IEnumerator ShootDebounce()
    {
        shootingDebounce = true;
        yield return new WaitForSeconds(shootingCooldown);
        shootingDebounce = false;
    }





    /// <summary>
    /// Handles collision between the player and tagged objects.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Coin thisCoin = other.GetComponent<Coin>();
            totalCoins += thisCoin.value;
            //thisCoin.value = 0;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "RegularEnemy")
        {
            
            int damage = other.gameObject.GetComponent<RegularEnemy>().contactDamage;
            TakeDamage(damage);
            

            //Respawn();
        }

        if (other.gameObject.tag == "Laser")
        {
            StartCoroutine(Stun());
        }
        if (other.gameObject.tag == "Teleporter")
        {
            //reset the startPosition to the spawnPoint location
            startPosition = other.gameObject.GetComponent<Teleporter>().spawnPoint.transform.position;
            //teleport the player to the "new" startPosition
            transform.position = startPosition;
        }
    }



    /// <summary>
    /// prevents the player from moving for x seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator Stun()
    {
        int currentPlayerSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(stunTimer);
        speed = currentPlayerSpeed;
    }





    /// <summary>
    /// Allows the player to jump using the space key as long as isGrounded is true
    /// </summary>
    public void SpaceJump()
    {
        RaycastHit hit;

        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.5f, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.5f))
        {
            isGrounded = true;
            //print("I am grounded");
        }
        else
        {
            isGrounded = false;
            //print("I am not grounded");
        }

        if (Input.GetKeyDown("space") && isGrounded) 
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }




    /// <summary>
    /// subtracts health from the player character and calls for the player to become invincible
    /// 
    /// if player is invincible, dont take damage
    /// </summary>
    /// <param name="damageToTake"></param>
    private void TakeDamage(int damageToTake)
    {

        if (isInvincible == false)
        {
            health -= damageToTake;
            StartCoroutine(TurnInvincible());
        }
    }





    /// <summary>
    /// prevents the player from taking damage for 5 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnInvincible()
    {
        if (isInvincible == false)
        {
            isInvincible = true;
            InvokeRepeating("toggleBlink", 0f, 0.25f);
            yield return new WaitForSeconds(5f);
            isInvincible = false;
            CancelInvoke();
            playerRenderer.enabled = true;
        }
    }

    private void toggleBlink()
    {
        //print(gameObject.name);
        if (playerRenderer.enabled == true)
        {
            playerRenderer.enabled = false;
        }
        else
        {
            playerRenderer.enabled = true;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    private void Respawn()
    {
        transform.position = startPosition;
        health--;

        if (health <= 0)
        {
            SceneManager.LoadScene(1);
            this.enabled = false;
        }
    }



    /// <summary>
    /// scans to see if the thwomp hit the player from the bottom or the top
    /// </summary>
    public void CheckForDanger()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, maxDistance: 1f))
        {
            //If player hits the collider and its tagged Thwomp then Respawn
            if (hit.collider.gameObject.tag == "Thwomp")
            {
                Respawn();
            }
        }
    }
}