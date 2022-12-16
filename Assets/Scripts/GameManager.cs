using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<string, GameObject> prefabs = new();
    public static Dictionary<string, AudioClip> sounds = new();

    public AudioMixer main;
    public AudioMixerGroup bgm;
    public AudioMixerGroup sfx;

    public static void LoadAllResources<T>(string dir, Dictionary<string, T> dict) where T : Object
    {
        foreach (var item in Resources.LoadAll<T>(dir))
            dict.Add(item.name, item);
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllResources("_Prefabs", prefabs);
            LoadAllResources("Audio", sounds);

            main.SetFloat("volume", -80);
            AudioManager.PlayBGM(sounds["bgm"], 1);
        }
    }

    public void PlayMenuSelect()
    {
        GameObject.DontDestroyOnLoad(AudioManager.Play(GameManager.sounds["menu_generic"], 1f));
    }

    IEnumerator FadeIn(float t)
    {
        for (float i = 0; i < t; i+= 1/60f)
        {
            main.SetFloat("volume", 10 * Mathf.Log10(i / t));
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    public void Start()
    {
        StartCoroutine(FadeIn(2));
    }

    public static IEnumerator Delay(System.Action action, float t)
    {
        yield return new WaitForSeconds(t);
        action?.Invoke();
    }

    public static IEnumerator DelayUntil(System.Action action, System.Func<bool> condition)
    {
        yield return new WaitUntil(condition);
        action?.Invoke();
    }
}
