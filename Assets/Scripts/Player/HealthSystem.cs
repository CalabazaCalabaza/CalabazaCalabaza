using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;

    public int CurrentLives { get; private set; }

    private bool isInvulnerable;

    private void Awake()
    {
        CurrentLives = maxLives;
    }

    //public void TakeDamage()
    //{
    //if (isInvulnerable) return;

    //CurrentLives--;
    //GameEvents.TriggerLivesChanged(CurrentLives);

    //if (CurrentLives <= 0)
    //GameEvents.TriggerPlayerDied();
    //else
    //GameEvents.TriggerPlayerHit();
    //}

    public void TakeDamage()
    {
        if (isInvulnerable) return;

        CurrentLives--;
        Debug.Log($"Lives remaining: {CurrentLives}");
        GameEvents.TriggerLivesChanged(CurrentLives);

        if (CurrentLives <= 0)
        {
            Debug.Log("Player died");
            GameEvents.TriggerPlayerDied();
        }
        else
        {
            GameEvents.TriggerPlayerHit();
        }
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public void ResetLives()
    {
        CurrentLives = maxLives;
        GameEvents.TriggerLivesChanged(CurrentLives);
    }
    public void SetLives(int lives)
    {
        CurrentLives = lives;
        GameEvents.TriggerLivesChanged(CurrentLives);
    }


    public void InstantKill()
    {
        CurrentLives = 0;
        GameEvents.TriggerLivesChanged(CurrentLives);
        GameEvents.TriggerPlayerDied();
    }
}