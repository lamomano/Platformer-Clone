using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Author: [Nguyen, Kanyon] & [Vrablick, Calihan]
 * Last Updated: [11/12/2023]
 * [Handles the linear movement of enemy laser projectiles]
 */
public class EnemyLaser : MonoBehaviour
{

    // projectile variables
    public float speed;
    public bool goingLeft;

    public int damage = 0;

    public GameObject thisLaser;

    // Start is called before the first frame update
    void Start()
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
    /// Handles the collisions of the laser. if it collides with the player or something else, destroy the laser
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "BossEnemy")
        {
            // do damage if the target hit was an enemy
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                print("damaged hooman");
            }

            //print(other.transform.parent.name);
            //print(gameObject.name);
            //print(other.gameObject.name);
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
