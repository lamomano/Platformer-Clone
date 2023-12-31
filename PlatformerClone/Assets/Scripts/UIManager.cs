using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



/*
 * Author: [Nguyen, Kanyon] & [Vrablick, Calihan]
 * Last Updated: [11/13/2023]
 * [Handles all the scores and lives gui in the game scene.]
 */
public class UIManager : MonoBehaviour
{

    public PlayerController playerController;
    public TMP_Text healthText;
    public TMP_Text coinsText;


    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + playerController.health;
        coinsText.text = "Coins: " + playerController.Coins;
    }
}
