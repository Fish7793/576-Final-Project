using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager
{
    public static void Play(AudioClip clip, float volume=1, float pitch=0)
    {
        var src = new GameObject(clip.name).AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch;
        src.Play();
        GameObject.Destroy(src.gameObject, clip.length);
    }

    public static AudioSource bgm;
    public static void PlayBGM(AudioClip clip, float volume)
    {
        if (bgm != null)
        {
            GameObject.Destroy(bgm.gameObject);
        }

        bgm = new GameObject(clip.name).AddComponent<AudioSource>();
        bgm.clip = clip;
        bgm.volume = volume;
        bgm.loop = true;
        bgm.Play();
        GameObject.DontDestroyOnLoad(bgm.gameObject);
    }
}
