using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private AudioClip activateClip;

    private bool isActive;
    private bool playerIsNear;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (isActive) return;
        if (!playerIsNear) return;

        if (Input.GetButtonDown("Jump"))
            Activate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerIsNear = false;
    }

    private void Activate()
    {
        isActive = true;
        GameManager.Instance.SetCheckpoint(transform.position);
        AudioManager.Instance.PlaySFX(activateClip);
        GameEvents.TriggerCheckpointActivated();
        OnActivated();
    }

    // Hook para el artista — cambiar sprite, activar partículas, etc.
    private void OnActivated()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
