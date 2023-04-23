using System;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private string musicVolumeParameter = "MusicVolume";

    [SerializeField]
    private string effectsVolumeParameter = "EffectsVolume";

    public float MusicVolume => PlayerPrefs.GetFloat(musicVolumeParameter, 1f);

    public float EffectsVolume => PlayerPrefs.GetFloat(effectsVolumeParameter, 1f);


    private const float MinVolume = 0.0001f;
    
    void Start()
    {
        SetMusicVolume(MusicVolume);
        SetEffectsVolume(EffectsVolume);
    }

    public void SetEffectsVolume(float effectsVolume)
    {
        SetVolume(effectsVolumeParameter, effectsVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        SetVolume(musicVolumeParameter, musicVolume);
    }

    private void SetVolume(string key, float value)
    {
        var mixerVolume = value <= MinVolume ? -80 : MathF.Log(value) * 20;
        mixer.SetFloat(key, mixerVolume);
        PlayerPrefs.SetFloat(key, value);
    }
}