using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Respawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 1.5f;  // Time to wait before respawning

    private GameObject player;

    private Vector3 respawnPosition;
    public Vector3 RespawnPosition => respawnPosition;

    private int checkpointLives;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        respawnPosition = spawnPoint.position;
        checkpointLives = player != null ? player.GetComponent<HealthSystem>().CurrentLives : 3;
        Debug.Log($"Spawn position set to: {respawnPosition}");


    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += OnPlayerDied;
        GameEvents.OnGoalReached += OnGoalReached;
        GameEvents.OnTimerExpired += OnTimerExpired;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= OnPlayerDied;
        GameEvents.OnGoalReached -= OnGoalReached;
        GameEvents.OnTimerExpired -= OnTimerExpired;
    }

    public void RegisterPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    private void OnPlayerDied()
    {
        Debug.Log("GameManager received death event");
        player.GetComponent<Animator>().SetBool("IsDead", true);
        StartCoroutine(RespawnRoutine());
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPosition = position;
        checkpointLives = player.GetComponent<HealthSystem>().CurrentLives;

        Debug.Log($"Checkpoint set at {position}");
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (player == null) return;

        player.transform.position = respawnPosition;

        var rb = player.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        player.GetComponent<Animator>().SetBool("IsDead", false);

        player.GetComponent<PlayerController>().Revive();
        player.GetComponent<HealthSystem>().SetLives(checkpointLives);
        GameEvents.TriggerPlayerRespawned();

    }

    private void OnGoalReached()
    {
        Debug.Log("Goal reached");
        // Freeze player input while the map reveal plays
        player.GetComponent<PlayerController>().SetInputEnabled(false);
        GameEvents.TriggerMapReveal();
    }

    private void OnTimerExpired()
    {
        Debug.Log("Time is up");
        player.GetComponent<HealthSystem>().InstantKill();
    }

}