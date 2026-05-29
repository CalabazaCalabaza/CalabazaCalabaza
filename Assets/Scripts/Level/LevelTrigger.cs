using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip narratorLine;
    [SerializeField] private bool triggerOnce = true; 
    private bool triggered;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && triggered) return;

        triggered = true;
        GameEvents.TriggerNarratorLine(narratorLine);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0f, 1f, 0.3f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}