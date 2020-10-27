using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    [Tooltip("The name of the scene you want to go to.")]
    public string destination;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ResetPlayer();
            GoToNextLevel();
        }
    }

    public void GoToNextLevel(int index = 0)
    {
        // if a number was passed through to this method, go to that level.
        if(index != 0){
            // we are going to a specific level
            UnityEngine.SceneManagement.SceneManager.LoadScene(destination);
            return; 
        }
        // if no index was given, and no destination was specified, go to main menu.
        else if(destination == "") {
            destination = "MainMenu";
        }
        // else, we are going to a specific level

        UnityEngine.SceneManagement.SceneManager.LoadScene(destination);
    }
}