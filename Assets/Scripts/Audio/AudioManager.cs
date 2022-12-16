using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager
{
    public static AudioSource Play(AudioClip clip, float volume=1, float pitch=1)
    {
        if (clip != null)
        {
            var src = new GameObject(clip.name).AddComponent<AudioSource>();
            src.outputAudioMixerGroup = GameManager.instance.sfx;
            src.clip = clip;
            src.volume = volume;
            src.pitch = pitch;
            src.playOnAwake = false;
            src.Play();
            GameObject.Destroy(src.gameObject, clip.length);
            return src;
        }

        return null;
    }

    public static IEnumerator PlaySequence(AudioClip clip, List<AudioSequenceData> data)
    {
        if (clip != null)
        {
            var src = new GameObject(clip.name + " sequence").AddComponent<AudioSource>();
            src.clip = clip;
            src.playOnAwake = false;
            src.outputAudioMixerGroup = GameManager.instance.sfx;

            foreach (var v in data)
            {
                yield return new WaitForSeconds(v.time);

                src.volume = v.volume;
                src.pitch = v.pitch;
                src.Play();
            }

            yield return new WaitForSeconds(clip.length);
            GameObject.Destroy(src.gameObject);
        }
    }

    public static AudioSource bgm;
    public static void PlayBGM(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            if (bgm != null)
            {
                GameObject.Destroy(bgm.gameObject);
            }

            bgm = new GameObject(clip.name).AddComponent<AudioSource>();
            bgm.outputAudioMixerGroup = GameManager.instance.bgm;
            bgm.clip = clip;
            bgm.volume = volume;
            bgm.loop = true;
            bgm.Play();
            GameObject.DontDestroyOnLoad(bgm.gameObject);
        }
    }
}
