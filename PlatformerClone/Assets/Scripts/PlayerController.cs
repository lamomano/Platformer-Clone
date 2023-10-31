using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the functionality of the player, including collision interaction, movement, and raycasting]
 */
public class PlayerController : MonoBehaviour
{

    public int speed = 0; 
    public int jumpForce = 10;


    public int totalCoins = 0;


    public int lives = 3;
    public int health = 99;
    public int fallDepth;
    private Vector3 startPosition;

    public float stunTimer;

    private Rigidbody rigidBody;
    private Transform gunBarrel;
    public GameObject projectilePrefab;
    public bool isGrounded;
    public bool facingLeft;
    public bool shootingDebounce = false;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; 
        rigidBody= GetComponent<Rigidbody>();
        gunBarrel = transform.Find("gun_barrel");
        facingLeft = false;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        SpaceJump();
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
            add_position += Vector3.left * speed * Time.deltaTime;
            facingLeft = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //print("Move the player right");
            add_position += Vector3.right * speed * Time.deltaTime;
            facingLeft = false;
        }

        transform.position += add_position;

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

        // make sure to wrap ShootDebounce() in StartCoroutine() in order for the coroutines to work

        if (Input.GetKey(KeyCode.Return))
        {
            StartCoroutine(ShootDebounce());

            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            if (projectile.GetComponent<PlayerLaser>())
            {
                projectile.GetComponent<PlayerLaser>().goingLeft = facingLeft;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(ShootDebounce());

            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            if (projectile.GetComponent<PlayerLaser>())
            {
                projectile.GetComponent<PlayerLaser>().goingLeft = true;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(ShootDebounce());
            
            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            if (projectile.GetComponent<PlayerLaser>())
            {
                projectile.GetComponent<PlayerLaser>().goingLeft = false;
            }
        }
    }



    IEnumerator ShootDebounce()
    {
        shootingDebounce = true;
        yield return new WaitForSeconds(0.5f);
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

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Spike")
        {
            Respawn();
        }

        if (other.gameObject.tag == "Portal")
        {
            //reset the startPos to the spawnPoint position
            startPosition = other.gameObject.GetComponent<Portal>().spawnPoint.transform.position;
            //bring the player back to the start position
            transform.position = startPosition;
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