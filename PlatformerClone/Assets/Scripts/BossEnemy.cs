using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * Author: [Nguyen, Kanyon] & [Vrablick, Calihan]
 * Last Updated: [11/11/2023]
 * [Handles the functionality of the Boss Enemy]
 */
public class BossEnemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] public int contactDamage = 0;
    [SerializeField] public int health = 0;

    public GameObject laserPrefab;



    //game objects to determine how far top/bottom the Thwomp will go
    public GameObject topPoint;
    public GameObject bottomPoint;

    //boundary points for top/bottom
    private Vector3 topPos;
    private Vector3 bottomPos;

    //side to side movement speed
    private int verticalSpeed = 8;

    //direction the Thwomp is going-up
    public bool goingUp;
    //check to see if the Thwomp is waiting
    public bool waiting;

    private bool shootingDebounce = false;
    public float shootingCooldown = 3;




    // Start is called before the first frame update
    void Start()
    {
        topPos = topPoint.transform.position;
        bottomPos = bottomPoint.transform.position;

        GameObject[] allGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject thisObject in allGameObjects)
        {
            if (thisObject.name == "Player")
            {
                player = thisObject;
            }
        }
    }



    /// <summary>
    /// shoots lasers at the player if the player is within proximity
    /// </summary>
    void Shoot()
    {
        if (shootingDebounce == true)
            return;

        GameObject projectile = Instantiate(laserPrefab, transform.position, laserPrefab.transform.rotation);
        if (projectile.GetComponent<EnemyLaser>())
        {
            // get direction of player

            bool shootLeft;
            if (player.transform.position.x > gameObject.transform.position.x)
                shootLeft = false;
            else
                shootLeft = true;

            //print("boss shooting");
            StartCoroutine(ShootDebounce());

            projectile.GetComponent<EnemyLaser>().goingLeft = shootLeft;
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



    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // send signal to playercontroller to switch scenes
            SceneManager.LoadScene(6);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (Vector3.Distance(player.transform.position, transform.position) > 20)
        {
            //print("player too far");
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        ThwompMovement();
        //Shoot();
    }



    /// <summary>
    /// rests the thwomp at the bottom for 2 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitToGoUp()
    {
        //Start the wait timer
        yield return new WaitForSeconds(2f);
        //After 2 seconds, now thwomp is going up and waiting is false
        goingUp = true;
        waiting = false;
    }



    /// <summary>
    /// makes the thwomp move up and down
    /// </summary>
    private void ThwompMovement()
    {
        if (goingUp)
        {
            //once the Thwomp reaches the topPos - goingUp is false
            if (transform.position.y >= topPos.y)
            {
                goingUp = false;
            }
            else
            {
                //translate the Thwomp up by speed using Time.deltaTime
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (transform.position.y <= bottomPos.y)
            {
                if (!waiting)
                {
                    waiting = true;
                    StartCoroutine(WaitToGoUp());
                }
            }
            else
            {
                //translate the Thwomp down by speed using Time.deltaTime
                transform.position += Vector3.down * verticalSpeed * Time.deltaTime;
            }
        }
    }
}