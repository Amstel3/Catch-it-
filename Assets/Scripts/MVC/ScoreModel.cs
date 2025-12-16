public class ScoreModel
{
    public static int LatestScore;
    private int score = 0;

    public delegate void OnScoreChanged(int newScore);
    public event OnScoreChanged ScoreChanged;

    public void IncreaseScore(int points)
    {
        score += points;

        // Cached statically to allow access from systems without model reference
        LatestScore = score;

        // Event used to keep model free from any rendering or UI concerns
        ScoreChanged?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        LatestScore = 0;

        // Reset propagated explicitly to keep listeners in sync
        ScoreChanged?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }
}