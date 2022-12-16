public class GameMenuHelper : MainMenu
{
    public void HideAll()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        optionMenuCanvas.gameObject.SetActive(false);
        howToPlayCanvas.gameObject.SetActive(false);
        AudioManager.Play(GameManager.sounds["menu_back"], 1f);
    }

    public void ReturnToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        DontDestroyOnLoad(AudioManager.Play(GameManager.sounds["menu_back"], 1f));
    }
}