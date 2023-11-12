using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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



    //game objects to determine how far top/bottom the Thwomp will go
    public GameObject topPoint;
    public GameObject bottomPoint;

    //boundary points for top/bottom
    private Vector3 topPos;
    private Vector3 bottomPos;

    //side to side movement speed
    private int verticalSpeed = 4;

    //direction the Thwomp is going-up
    public bool goingUp;
    //check to see if the Thwomp is waiting
    public bool waiting;




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

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        ThwompMovement();

        if (health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
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
