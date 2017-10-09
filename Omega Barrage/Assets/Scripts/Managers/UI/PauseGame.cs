using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    InputManager myInputs;
    bool paused = false;
    public GameObject pauseMenu;
    public bool isMenuShowing;


    void Awake()
    {
        myInputs = GetComponent<InputManager>();                
    }

    void Update()
    {
        if(myInputs != null)
        {
            if (Input.GetKeyDown(myInputs.menu))
            {
                isMenuShowing = true;
            }
        }
        if(myInputs == null)
        {
            isMenuShowing = gameObject.activeSelf;
        }
        Debug.Log(isMenuShowing);
        paused = togglePause();
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
        Debug.Log(isMenuShowing + "1");
        isMenuShowing = false;
        pauseMenu.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log(isMenuShowing);
    }
}
