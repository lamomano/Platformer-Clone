using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the functionality of the player, including collision interaction, movement, and raycasting]
 */
public class PlayerController : MonoBehaviour
{


    public int speed = 2; 
    public int jumpForce = 10;


    public int totalCoins = 0;


    public int lives = 3;
    public int fallDepth;
    private Vector3 startPosition;

    public float stunTimer;

    private Rigidbody rigidBody;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; 
        rigidBody= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
        SpaceJump();
    }


    /// <summary>
    /// handles the player movement
    /// also checks to see if the player fell off the map
    /// </summary>
    private void Move()
    {

        Vector3 add_position = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            print("Move the player left");
            add_position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            print("Move the player right");
            add_position += Vector3.right * speed * Time.deltaTime;
        }

        transform.position += add_position;

        if (transform.position.y < fallDepth) 
        {
            Respawn();
        }

        CheckForDanger();
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




    private void Respawn()
    {
        transform.position = startPosition;
        lives--;

        if (lives <= 0)
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
