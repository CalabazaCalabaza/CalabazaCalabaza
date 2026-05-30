using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField] private AudioClip damageClip;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<HealthSystem>().TakeDamage();
        AudioManager.Instance.PlaySFX(damageClip);
    }
}