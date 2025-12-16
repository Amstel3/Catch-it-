using UnityEngine;

public class Apple : FallingObject, IFruit
{
    public override void OnGroundHit()
    {
        // Guarded to prevent score changes outside active gameplay
        if (!GameManager.instance.IsPlaying())
            return;

        if (scoreController != null)
            scoreController.IncreaseScore(1);

        ReturnToPool();
    }

    public override void OnPlayerCollision()
    {
        ReturnToPool();
    }

    public override void OnSpawn()
    {
        // Base reset kept here to ensure pooled objects never leak previous state
        base.OnSpawn();
    }

    protected override float GetSpeedMultiplier()
    {
        return 1f;
    }
}






















