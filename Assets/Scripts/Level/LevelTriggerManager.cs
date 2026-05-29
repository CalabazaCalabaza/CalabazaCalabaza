using UnityEngine;

public class LevelTriggerManager : MonoBehaviour
{
    public static LevelTriggerManager Instance { get; private set; }

    private LevelTrigger[] allTriggers;

    private void Awake()
    {
        Instance = this;
        allTriggers = FindObjectsByType<LevelTrigger>(FindObjectsSortMode.None);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += ResetTriggersAfterCheckpoint;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= ResetTriggersAfterCheckpoint;
    }

    private void ResetTriggersAfterCheckpoint()
    {
        Vector3 checkpoint = GameManager.Instance.RespawnPosition;

        foreach (var trigger in allTriggers)
        {
            // Reset only triggers that are ahead of the current checkpoint
            if (trigger.transform.position.x > checkpoint.x)
                trigger.ResetTrigger();
        }
    }
}