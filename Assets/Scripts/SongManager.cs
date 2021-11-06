using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    
    [SerializeField] private float duration;
    [SerializeField] private float musicVolume;
    [SerializeField] private float sfxVolume;
    [SerializeField] private bool sfxOn = true;
    [SerializeField] private bool musicOn = true;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI musicVolText;
    [SerializeField] private TextMeshProUGUI sfxVolText;
    [SerializeField] private AudioSource[] sfxSources;
    [SerializeField] private float maxVolume = 1;
    [SerializeField] private int currentVolumeMusic = 5;
    [SerializeField] private int currentVolumeSFX = 5;
    [SerializeField] private int maxVolumeNum = 10;


    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = 0;

        

        if(WorldSettings.songManager == null)
        {
            WorldSettings.songManager = this;
        }

        UpdateMusicVolume();
        StartCoroutine(StartFade(audioSource, duration, musicVolume));
    }

    private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void ToggleSFX()
    {
        SongManager.PlayButtonSound();
        sfxOn = !sfxOn;

        foreach (AudioSource source in sfxSources)
        {
            if (!sfxOn)
            {
                source.volume = 0;
            }
            else
            {
                source.volume = sfxVolume;
            }
            
        }

        UpdateMenuText();
    }

    public void ToggleMusic()
    {
        SongManager.PlayButtonSound();
        musicOn = !musicOn;

        if(!musicOn)
        {
            StartCoroutine(StartFade(audioSource, duration, 0));
        }
        else
        {
            StartCoroutine(StartFade(audioSource, duration, musicVolume));
        }

        UpdateMenuText();
    }

    private void UpdateMenuText()
    {
        sfxText.text = (sfxOn ? "SFX: On" : "SFX: Off");
        musicText.text = (musicOn ? "Music: On" : "Music: Off");
    }

    public static void PlayButtonSound()
    {
        GameObject child = WorldSettings.songManager.gameObject.transform.GetChild(0).gameObject;
        child.GetComponent<AudioSource>().Play();
    }

    public void UpdateMusicVolume()
    {
        musicVolText.text = currentVolumeMusic.ToString();
        float newVolume = Remap(currentVolumeMusic, 0, 10, 0, maxVolume);
        audioSource.volume = newVolume;
        musicVolume = newVolume;
    }

    public void UpdateSFXVolume()
    {
        sfxVolText.text = currentVolumeSFX.ToString();
        float newVolume = Remap(currentVolumeSFX, 0, 10, 0, maxVolume);
        sfxVolume = newVolume;
        
        foreach (AudioSource source in sfxSources)
        {
            source.volume = newVolume;
        }
    }

    public void MusicVolumeUp()
    {
        if(currentVolumeMusic < maxVolumeNum)
        {
            currentVolumeMusic ++;
            UpdateMusicVolume();
        }
        
    }

    public void MusicVolumeDown()
    {
        if (currentVolumeMusic > 0)
        {
            currentVolumeMusic--;
            UpdateMusicVolume();

        }
    }

    public void SFXVolumeUp()
    {
        if (currentVolumeSFX < maxVolumeNum)
        {
            currentVolumeSFX ++;
            UpdateSFXVolume();
        }
    }

    public void SFXVolumeDown()
    {
        if (currentVolumeSFX > 0)
        {
            currentVolumeSFX--;
            UpdateSFXVolume();

        }
    }

    public float Remap(float val, float in1, float in2, float out1, float out2)
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }
}
