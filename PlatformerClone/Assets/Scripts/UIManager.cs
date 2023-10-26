using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/23/2023]
 * [Handles all the scores and lives gui in the game scene.]
 */
public class UIManager : MonoBehaviour
{

    public PlayerController playerController;
    public TMP_Text coinsText;
    public TMP_Text livesText;


    // Update is called once per frame
    void Update()
    {
        coinsText.text = "Coins: " + playerController.totalCoins;
        livesText.text = "Lives: " + playerController.lives;
    }
}
