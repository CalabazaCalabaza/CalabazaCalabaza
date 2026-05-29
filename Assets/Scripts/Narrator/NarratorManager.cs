using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NarratorManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Queue<AudioClip> queue = new Queue<AudioClip>();
    private bool isPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnNarratorLine += EnqueueLine;
    }

    private void OnDisable()
    {
        GameEvents.OnNarratorLine -= EnqueueLine;
    }

    private void EnqueueLine(AudioClip clip)
    {
        if (clip == null) return;
        queue.Enqueue(clip);

        if (!isPlaying)
            StartCoroutine(PlayQueue());
    }

    private IEnumerator PlayQueue()
    {
        isPlaying = true;

        while (queue.Count > 0)
        {
            AudioClip clip = queue.Dequeue();
            audioSource.clip = clip;
            audioSource.Play();

            // Wait for the clip to finish before playing the next one
            yield return new WaitForSeconds(clip.length);
        }

        isPlaying = false;
    }
}