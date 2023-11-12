using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Author: [Vrablick, Calihan] & [Nguyen, Kanyon]
 * Last Updated: [11/11/2023]
 * [Controls the movement of the hard enemy]
 */

public class HardEnemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] public int contactDamage = 0;
    [SerializeField] public int health = 0;



    private void Start()
    {
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
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}