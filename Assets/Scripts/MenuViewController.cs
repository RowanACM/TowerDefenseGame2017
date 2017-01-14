using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuViewController : MonoBehaviour {

    private GameObject currentView;

    public GameObject mainView;
    public GameObject levelView;

    public bool canCloseMenu;
    public bool isOpen;

    public void Start()
    {
        mainView.active = true;
        currentView = mainView;
        levelView.active = false;
        isOpen = true;
    }

    public void Update()
    {
        if (canCloseMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            toggleMenu();
        }
    }

    public void toggleMenu()
    {
        currentView.active = !currentView.active;
        isOpen = !isOpen;
    }

    public void hideMenu()
    {
        currentView.active = false;
        isOpen = false;
    }

    public void showMenu()
    {
        currentView.active = true;
        isOpen = true;
    }

    public void changeView(GameObject newView)
    {
        
        if (currentView)
        {
            newView.active = currentView.active;
            currentView.active = false;
        }
        else if (isOpen)
        {
            newView.active = true;
        }
        currentView = newView;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
