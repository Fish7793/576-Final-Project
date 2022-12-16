using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public float LToLog(float f)
    {
        if (f <= 0)
            return -80;
        return 10 * Mathf.Log10(f);
    }

    public void SetSFXVolume(float f)
    {
        GameManager.instance.main.SetFloat("sfx_volume", LToLog(f));
    }

    public void SetBGMVolume(float f)
    {
        GameManager.instance.main.SetFloat("bgm_volume", LToLog(f));
    }
}