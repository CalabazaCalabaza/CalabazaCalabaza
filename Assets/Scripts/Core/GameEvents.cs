using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<AudioClip> OnNarratorLine;
    public static event Action OnPlayerDied;
    public static event Action OnPlayerHit;
    public static event Action<int> OnLivesChanged;
    public static event Action OnGoalReached;
    public static event Action OnCheckpointActivated;
    public static event Action OnMapReveal;
    public static event Action<float> OnTimerUpdated;
    public static event Action OnTimerExpired;
    public static event Action OnPlayerRespawned;


    public static void TriggerNarratorLine(AudioClip clip) => OnNarratorLine?.Invoke(clip);
    public static void TriggerPlayerDied() => OnPlayerDied?.Invoke();
    public static void TriggerPlayerHit() => OnPlayerHit?.Invoke();
    public static void TriggerLivesChanged(int current) => OnLivesChanged?.Invoke(current);
    public static void TriggerGoalReached() => OnGoalReached?.Invoke();
    public static void TriggerCheckpointActivated() => OnCheckpointActivated?.Invoke();
    public static void TriggerMapReveal() => OnMapReveal?.Invoke();
    public static void TriggerTimerUpdated(float time) => OnTimerUpdated?.Invoke(time);
    public static void TriggerTimerExpired() => OnTimerExpired?.Invoke();
    public static void TriggerPlayerRespawned() => OnPlayerRespawned?.Invoke();

}