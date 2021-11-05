using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopFixer : MonoBehaviour
{
    [SerializeField] private float waitTime = 0.05f;
    private AudioClip clip;
    private AudioSource audioSource;
    private bool playing = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clip = audioSource.clip;
    }

    void Update()
    {
        if (!audioSource.isPlaying && !playing)
        {
            playing = true;
            audioSource.volume = 0;
            StartCoroutine(StartFade(audioSource, waitTime, 0.08f));
        }
    }

    private IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        audioSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        playing = false;
        yield break;
        
    }
}
