using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private AudioClip activateClip;

    public SpriteRenderer sprite_;
    public Sprite activatedCheckpoint;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Activate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
    }

    private void Activate()
    {
        if (sprite_.sprite == activatedCheckpoint) return;
        GameManager.Instance.SetCheckpoint(transform.position);
        AudioManager.Instance.PlaySFX(activateClip);
        GameEvents.TriggerCheckpointActivated();
        OnActivated();
    }

    // Hook para el artista — cambiar sprite, activar partículas, etc.
    private void OnActivated()
    {
        sprite_.sprite = activatedCheckpoint;
        Debug.Log("sprite swapeado");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
