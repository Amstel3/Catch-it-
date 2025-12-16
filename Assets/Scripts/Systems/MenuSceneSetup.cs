using UnityEngine;

public class MenuSceneSetup : MonoBehaviour
{
    private void Awake()
    {
        // Reset enforced here to avoid carrying paused state across scene transitions
        Time.timeScale = 1f;
    }
}
