using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject pauseMenu;
    public GameObject gameovermenu;
    public GameObject gameWinMenu;

    // Use this for initialization
    void Start()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
        pauseMenu.SetActive(false);
        gameovermenu.SetActive(false);
        gameWinMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                GetComponent<MenuFunctions>().PauseGame();
            }
            else
            {
                pauseMenu.SetActive(false);
                GetComponent<MenuFunctions>().ResumeGame();
            }
        }
    }

    public void GameOver()
    {
        gameovermenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        gameWinMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
