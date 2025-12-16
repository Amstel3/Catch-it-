using UnityEngine; 

  public  class Apple : FallingObject
    {
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnGroundHit()
    {
        Debug.Log("Apple hit the ground, increasing score.");
        //Добавляем одно очко за яблоко
        ScoreManager.instance.IncreaseScore(1);
    }

    public override void OnPlayerCollision()
    {

        base.OnPlayerCollision();
        Destroy(gameObject);
    }
}









