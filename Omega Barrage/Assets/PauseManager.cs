using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    public GameObject player;
    public GameObject pauseMenu;
    public bool paused;
    public bool isMenuShowing;

    void Update()
    {
        if(player != null)
        {
            if (player.GetComponent<InputManager>() != null)
            {
                if (Input.GetKeyDown(player.GetComponent<InputManager>().menu))
                {
                    isMenuShowing = !isMenuShowing;
                    paused = togglePause();
                }
            }

        }
        //Debug.Log(isMenuShowing);
        pauseMenu.SetActive(isMenuShowing);

    }



    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }

    public void ResumeGame()
    {
        isMenuShowing = !isMenuShowing;
        paused = togglePause();
    }
}
