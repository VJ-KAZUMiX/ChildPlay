using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    private BallFactory ballFactory;

    /// <summary>
    /// Unity Start
    /// </summary>
    private void Start ()
    {
        Ball ball = ballFactory.Create (2);
        ball.transform.localPosition = Vector3.zero;
        ball.OnPointerDownHandler += SplitBall;
    }

    private void SplitBall(Ball ball)
    {
        float newArea = ball.Area / 2.0f;
        float newRadius = Mathf.Sqrt (newArea / Mathf.PI);
        ball.ChangeRadius (newRadius, 1);

        Ball newBall = ballFactory.Create (newRadius);
        newBall.OnPointerDownHandler += SplitBall;
        Vector3 pos = ball.transform.position;
        Vector2 randomPos = Random.insideUnitCircle * newRadius;
        pos.x += randomPos.x;
        pos.y += randomPos.y;
        newBall.transform.localPosition = pos;
    }

}
