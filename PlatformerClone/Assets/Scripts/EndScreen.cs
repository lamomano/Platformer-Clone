using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * Author: [Nguyen, Kanyon]
 * Last Updated: [10/02/2023]
 * [Handles the main menu and game over screen button functionalities to restart or quit the game.]
 */

public class EndScreen : MonoBehaviour
{

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Changes the current scene to the scene with a matching index
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
