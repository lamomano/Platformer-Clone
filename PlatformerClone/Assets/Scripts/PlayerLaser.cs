using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the linear movement of laser projectiles]
 */
public class PlayerLaser : MonoBehaviour
{
    // projectile variables
    public float speed;
    public bool goingLeft;


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
    /// Handles the collisions of the laser. if it collides with anything other than the player, delete and do damage if possible
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        print("hit");
        if (other.gameObject.tag != "Player")
        {
            // do damage if the target hit was an enemy
            if (other.gameObject.tag == "Enemy")
            {

            }


            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
