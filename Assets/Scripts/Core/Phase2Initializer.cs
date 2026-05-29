using UnityEngine;

public class Phase2Initializer : MonoBehaviour
{
    private void Start()
    {
        var player = FindFirstObjectByType<PlayerController>();
        PhaseManager.Instance.StartPhase(player);
    }
}