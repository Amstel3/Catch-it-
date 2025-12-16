using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private ScoreModel scoreModel;
    private ScoreView scoreView;
    private int lastConfettiMilestone = 0;

    private void Start()
    {
        // Model created locally to keep score state independent from scene objects
        scoreModel = new ScoreModel();

        // View resolved at runtime to avoid tight coupling with UI hierarchy
        if (scoreView == null)
            scoreView = FindObjectOfType<ScoreView>();

        UpdateView();
    }

    public void IncreaseScore(int points)
    {
        scoreModel.IncreaseScore(points);
        UpdateView();

        int score = scoreModel.GetScore();
        int milestone = score / 50;

        // Milestone tracked to prevent repeated triggers within the same range
        if (milestone > lastConfettiMilestone)
        {
            lastConfettiMilestone = milestone;
            GameManager.instance.PlayConfetti();
        }
    }

    public void ResetScore()
    {
        // Explicit reset keeps retries visually and logically consistent
        scoreModel.ResetScore();
        UpdateView();
    }

    private void UpdateView()
    {
        // View updated centrally to keep rendering logic in one place
        if (scoreView != null)
            scoreView.UpdateScoreText(scoreModel.GetScore());
        else
            Debug.LogError("ScoreView not found");
    }

    public void DestroyAllFallingObjects()
    {
        // Delegated to GameManager to avoid direct dependency on pooling logic
        GameManager.instance.ReturnAllFallingObjectsToPool();
    }

    private void OnEnable()
    {
        // Subscribed to keep score lifecycle aligned with game sessions
        GameEvents.OnGameStart += ResetScore;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= ResetScore;
    }
}
















