using System.Collections;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager Instance { get; private set; }

    [Header("Speed")]
    [SerializeField] private float phase2SpeedMultiplier = 1.5f;

    [Header("Timer")]
    [SerializeField] private float totalTime = 60f;

    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    private PlayerController player;

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
        GameEvents.OnPlayerRespawned += OnPlayerRespawned;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= OnPlayerRespawned;
    }

    public void StartPhase(PlayerController playerController)
    {
        player = playerController;
        RestartTimer();
        player.SetSpeedMultiplier(phase2SpeedMultiplier);
    }

    private void OnPlayerRespawned()
    {
      
        RestartTimer();
    }

    private void RestartTimer()
    {
        StopAllCoroutines();
        TimeRemaining = totalTime;
        IsRunning = true;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            GameEvents.TriggerTimerUpdated(TimeRemaining);
            yield return null;
        }

        TimeRemaining = 0;
        GameEvents.TriggerTimerUpdated(0);
        IsRunning = false;
        GameEvents.TriggerTimerExpired();
    }

    public void StopTimer()
    {
        IsRunning = false;
        StopAllCoroutines();
    }
}