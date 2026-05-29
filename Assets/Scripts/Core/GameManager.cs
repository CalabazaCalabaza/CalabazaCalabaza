using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Respawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 1.5f;  // Time to wait before respawning

    private GameObject player;

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
        GameEvents.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= OnPlayerDied;
    }

    public void RegisterPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    private void OnPlayerDied()
    {
        Debug.Log("GameManager received death event");
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (player == null) return;

        player.transform.position = spawnPoint.position;

        var rb = player.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        player.GetComponent<PlayerController>().Revive();
        player.GetComponent<HealthSystem>().ResetLives();
    }
}