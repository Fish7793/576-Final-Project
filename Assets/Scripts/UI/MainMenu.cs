using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.DontDestroyOnLoad(AudioManager.Play(GameManager.sounds["menu_accept"], 0.8f));
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        AudioManager.Play(GameManager.sounds["menu_back"], 0.8f);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
