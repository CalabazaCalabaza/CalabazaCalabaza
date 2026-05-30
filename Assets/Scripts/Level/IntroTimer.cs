using System.Collections;
using UnityEngine;

public class IntroTimer : MonoBehaviour
{
    public static IntroTimer Instance { get; private set; }

    [Header("Timer")]
    [SerializeField] private float duration = 67f;

    [Header("Narrator")]
    [SerializeField] private AudioClip onExpiredClip;

    public float TimeRemaining { get; private set; }
    private float checkpointTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnCheckpointActivated += SaveTime;
        GameEvents.OnPlayerRespawned += RestoreTime;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckpointActivated -= SaveTime;
        GameEvents.OnPlayerRespawned -= RestoreTime;
    }

    private void Start()
    {
        TimeRemaining = duration;
        checkpointTime = duration;
        StartCoroutine(TimerRoutine());
    }

    private void SaveTime()
    {
        checkpointTime = TimeRemaining;
    }

    private void RestoreTime()
    {
        StopAllCoroutines();
        TimeRemaining = checkpointTime;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            yield return null;
        }

        TimeRemaining = 0;
        GameEvents.TriggerNarratorLine(onExpiredClip);
    }
}