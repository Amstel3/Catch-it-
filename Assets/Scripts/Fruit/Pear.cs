using UnityEngine;

public class Pear : FallingObject, IFruit
{
    [SerializeField, Range(1f, 3f)]
    private float speedMultiplier = 1.5f;

    public override void OnGroundHit()
    {
        // Guarded to prevent score changes outside active gameplay
        if (!GameManager.instance.IsPlaying())
            return;

        if (scoreController != null)
            scoreController.IncreaseScore(2);

        ReturnToPool();
    }

    public override void OnPlayerCollision()
    {
        ReturnToPool();
    }

    public override void OnSpawn()
    {
        // Base reset kept to avoid inheriting pooled state
        base.OnSpawn();
    }

    protected override float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }
}








