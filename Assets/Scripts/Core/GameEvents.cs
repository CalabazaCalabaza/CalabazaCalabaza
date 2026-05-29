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

    public static void TriggerNarratorLine(AudioClip clip) => OnNarratorLine?.Invoke(clip);
    public static void TriggerPlayerDied() => OnPlayerDied?.Invoke();
    public static void TriggerPlayerHit() => OnPlayerHit?.Invoke();
    public static void TriggerLivesChanged(int current) => OnLivesChanged?.Invoke(current);
    public static void TriggerGoalReached() => OnGoalReached?.Invoke();
    public static void TriggerCheckpointActivated() => OnCheckpointActivated?.Invoke();
    public static void TriggerMapReveal() => OnMapReveal?.Invoke();
}