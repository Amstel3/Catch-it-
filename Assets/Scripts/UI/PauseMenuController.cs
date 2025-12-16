using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void GoToMenu()
    {
        // State switched before scene load to keep global systems consistent
        GameStateMachine.Instance.SetState(GameState.MainMenu);

        SceneManager.LoadScene("MainMenu");
    }
}
