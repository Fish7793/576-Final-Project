using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Canvas optionMenuCanvas;
    public Canvas howToPlayCanvas;
    

    public void SetCanvasActive(Canvas canvas)
    {
        mainMenuCanvas.gameObject.SetActive(mainMenuCanvas == canvas);
        optionMenuCanvas.gameObject.SetActive(optionMenuCanvas == canvas);
        howToPlayCanvas.gameObject.SetActive(howToPlayCanvas == canvas);
    }

    public void StartGame()
    {
        GameObject.DontDestroyOnLoad(AudioManager.Play(GameManager.sounds["menu_accept"], 1f));
        SceneManager.LoadScene(1);
    }

    public void Main()
    {
        SetCanvasActive(mainMenuCanvas);
        GameManager.instance.PlayMenuSelect();
    }

    public void Options()
    {
        SetCanvasActive(optionMenuCanvas);
        GameManager.instance.PlayMenuSelect();
    }

    public void HowToPlay()
    {
        SetCanvasActive(howToPlayCanvas);
        GameManager.instance.PlayMenuSelect();
    }

    public void Quit()
    {
        AudioManager.Play(GameManager.sounds["menu_back"], 1f);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

[System.Serializable]
public struct HowToInfo
{
    public List<GameObject> items;
}

[System.Serializable]
public struct GlossaryInfo
{
    public string title;
    public string description;
}

[System.Serializable]
public struct Glossary
{
    List<GlossaryInfo> info;
}

