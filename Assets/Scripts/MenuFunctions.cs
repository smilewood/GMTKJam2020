using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartLevel( string level )
    {
        ResumeGame();
        SceneManager.LoadScene(level);
    }

    private float preMuteVolume = 100;
    public void Mute()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = preMuteVolume;
        }
        else
        {
            preMuteVolume = AudioListener.volume;
            AudioListener.volume = 0;
        }
    }

    public void SetVolume( float setting )
    {
        AudioListener.volume = setting;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
