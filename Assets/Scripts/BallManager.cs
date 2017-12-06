using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    private BallFactory ballFactory;

    private List<Ball> activeBallList = new List<Ball> () { Capacity = 32 };
    private List<Ball> vanishingBallList = new List<Ball> () { Capacity = 16 };

    /// <summary>
    /// Unity Start
    /// </summary>
    private void Start ()
    {
        Ball ball = RetainBall (2);
        ball.transform.localPosition = Vector3.zero;
    }

    private Ball RetainBall(float radius)
    {
        Ball ball = ballFactory.Create (radius);
        activeBallList.Add (ball);
        ball.OnPointerDownHandler += SplitBall;
        return ball;
    }

    private void SplitBall(Ball ball)
    {
        float newArea = ball.Area / 2.0f;
        float newRadius = Mathf.Sqrt (newArea / Mathf.PI);
        ball.ChangeRadius (newRadius, 1);

        Ball newBall = RetainBall (newRadius);
        Vector3 pos = ball.transform.position;
        Vector2 randomPos = Random.insideUnitCircle * newRadius;
        pos.x += randomPos.x;
        pos.y += randomPos.y;
        newBall.transform.position = pos;
    }

    /// <summary>
    /// Unity FixedUpdate
    /// </summary>
    private void FixedUpdate ()
    {
        activeBallList.Sort (CompareByArea);

        for (int i = 0, len = activeBallList.Count; i < len; i++) {
            Ball ball = activeBallList[i];
            if (ball.CurrentBallState == Ball.BallState.Vanishing) {
                continue;
            }
            if (ball.CollisionBall == null || ball.CollisionBall.CurrentBallState != Ball.BallState.Idle) {
                continue;
            }
            float newArea = ball.Area + ball.CollisionBall.Area;
            float newRadius = Mathf.Sqrt (newArea / Mathf.PI);
            ball.ChangeRadius (newRadius, 1);
            ball.CollisionBall.Vanish ();
            vanishingBallList.Add (ball.CollisionBall);
        }

        for (int i = 0, len = vanishingBallList.Count; i < len; i++) {
            activeBallList.Remove (vanishingBallList[i]);
        }
    }

    /// <summary>
    /// 面積の大きい順
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int CompareByArea(Ball a, Ball b)
    {
        return Mathf.RoundToInt (b.Area - a.Area);
    }

}
