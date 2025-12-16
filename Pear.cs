using UnityEngine;

public class Pear : FallingObject
{
    [SerializeField] private float speedMultiplier = 0.3f;

    protected override void Start()
    {
        base.Start();
        //Груша падает быстрее
        fallSpeed *= speedMultiplier
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnGroundHit()
    {
        //Добавляем 2 очка за грушу
        ScoreManager.instance.IncreaseScore(2);
    }


    public override void OnPlayerCollision()
    {
        base.OnPlayerCollision();
        Destroy(gameObject);
    }
}





