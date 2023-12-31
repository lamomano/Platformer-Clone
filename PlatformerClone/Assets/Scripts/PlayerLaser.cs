using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/31/2023]
 * [Handles the linear movement of laser projectiles]
 */
public class PlayerLaser : MonoBehaviour
{
    // projectile variables
    public float speed;
    public bool goingLeft;

    public int damage = 0;

    public GameObject thisLaser;


    private void Start()
    {
        thisLaser = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goingLeft == true)
        {
            transform.position += speed * Vector3.left * Time.deltaTime;           
        }
        else
        {
            transform.position += speed * Vector3.right * Time.deltaTime;
        }
    }

    


    /// <summary>
    /// Handles the collisions of the laser. if it collides with anything other than the player, delete and do damage if its an enemy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            // do damage if the target hit was an enemy
            if (other.gameObject.tag == "RegularEnemy")
            {
                other.gameObject.GetComponent<RegularEnemy>().health -= damage;
                print("damaged enemy");
            }
            if (other.gameObject.tag == "HardEnemy")
            {
                other.gameObject.GetComponent<HardEnemy>().health -= damage;
                print("damaged enemy");
            }
            if (other.gameObject.tag == "BossEnemy")
            {
                other.gameObject.GetComponent<BossEnemy>().health -= damage;
                print("damaged enemy");
            }


            //print(other.gameObject.tag);
            DestroyLaser();
        }
    }


    /// <summary>
    /// Destroys the laser gameobject.
    /// </summary>
    void DestroyLaser()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
