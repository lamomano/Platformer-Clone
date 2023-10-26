using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles the spawners of laser projectiles]
 */
/*
public class Spawner : MonoBehaviour
{

    //projectile variables
    public bool goingLeft;

    //spawner variables
    public GameObject projectilePrefab;
    public float timeBetweenShots;
    public float startDelay;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnProjectile", startDelay, timeBetweenShots);
    }


    public void SpawnProjectile()
    {
        //print("Spawning");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        if (projectile.GetComponent<Laser>())
        {
            projectile.GetComponent<Laser>().goingLeft = goingLeft;
        }
    }
}
*/