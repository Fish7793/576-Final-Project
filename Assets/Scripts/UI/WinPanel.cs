using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Button nextLevelButton;
    public Button quitButton;
    public Button closeButton;

    public void Quit()
    {
        DontDestroyOnLoad(AudioManager.Play(GameManager.sounds["menu_back"], 0.8f));

        SceneManager.LoadScene("Title Screen");
    }

    public void Close()
    {
        AudioManager.Play(GameManager.sounds["menu_back"], 0.8f);

        Destroy(gameObject);
    }
}
