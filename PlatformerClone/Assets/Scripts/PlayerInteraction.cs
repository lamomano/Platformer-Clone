using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public float coinsCollected = 0;
    public int Coins = 0;
    private Vector3 startPosition;


    /// <summary>
    /// handles all trigger collisions for picking up coinc
    /// </summary>
    /// <param name="other">the collider that was intereacted with</param>
    private void OnTriggerEnter(Collider other)
    {
        //if the object I collide with has the tag Coins, add one to the Coins count and set the interacted with object to off
        if (other.gameObject.tag == "Coins")
        {
            Coins++;
            other.gameObject.SetActive(false);
            Debug.Log("Hit a Coin");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the object that got collided with is tagged with FinalTeleporter
        if (collision.gameObject.tag == "FinalTeleporter")
        {
            Debug.Log("Collided with FinalTeleporter");
            //if Coins equal to the amount of Coins needed, stored in FinalTeleporter.cs then go into the if statement
            if (Coins == collision.transform.GetComponent<FinalTeleporter>().coinsNeeded)
            {
                //set door off
                collision.gameObject.SetActive(false);
                //remove amount of keys used
                Coins -= collision.transform.GetComponent<FinalTeleporter>().coinsNeeded;

                //reset the startPosition to the spawnPoint location
                startPosition = collision.transform.GetComponent<FinalTeleporter>().spawnPoint.transform.position;
                //teleport the player to the "new" startPosition
                transform.position = startPosition;
            }
            else
            {
                Debug.Log("Not enough coins to get to Boss Fight");
                SceneManager.LoadScene(4);
            }
        }
    }
}
