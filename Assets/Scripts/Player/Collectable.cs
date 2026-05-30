using UnityEngine;

public class Collectable : MonoBehaviour
{
    public AudioSource audio_;
    public GameObject morron;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (morron == null) return;
        audio_.Play();
        Destroy(morron);
    }
}
