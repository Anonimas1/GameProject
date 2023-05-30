using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public int Score;
    public int Multiplyer = 1;
    public float TimeUntilMulitplyerReset = 0f;
    public float MultiplyerResetTime = 5f;

    private void Update()
    {
        if (TimeUntilMulitplyerReset > 0)
        {
            TimeUntilMulitplyerReset -= Time.deltaTime;
        }

        if (TimeUntilMulitplyerReset <= 0)
        {
            TimeUntilMulitplyerReset = 0;
        }

        if (TimeUntilMulitplyerReset == 0)
        {
            Multiplyer = 1;
        }
    }

    public void EnemyKilled(GameObject gameObject)
    {
        if (gameObject.CompareTag("NonPlayerPlaced"))
        {
            if (gameObject.TryGetComponent<MeleeAttacker>(out var _))
            {
                Score += 1 * Multiplyer;
            }
            else if (gameObject.TryGetComponent<RangeAttacker>(out var _))
            {
                Score += 10 * Multiplyer;
            }

            Multiplyer += 1;
            TimeUntilMulitplyerReset = MultiplyerResetTime;
        }
    }
}