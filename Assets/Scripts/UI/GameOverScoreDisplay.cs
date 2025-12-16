using UnityEngine;
using TMPro;

public class GameOverScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestText;

    private void OnEnable()
    {
        // Static access used to avoid dependency on score lifecycle after game over
        int currentScore = ScoreModel.LatestScore;

        // Loaded here to keep persistence logic local to the results screen
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Updated only on this screen to avoid unnecessary writes during gameplay
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        // UI updated on enable to support screen reactivation without extra hooks
        scoreText.text = $"Score: {currentScore}";
        bestText.text = $"Best: {bestScore}";
    }
}
