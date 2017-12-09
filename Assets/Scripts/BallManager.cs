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
    /// Unity Awake
    /// </summary>
    private void Awake ()
    {
        Application.targetFrameRate = -1;
    }

    /// <summary>
    /// Unity Start
    /// </summary>
    private void Start ()
    {
        Ball ball = CreateBall (2);
        ball.transform.localPosition = Vector3.zero;
    }

    private Ball CreateBall(float radius)
    {
        Ball ball = ballFactory.Create ();
        ball.Init (radius / 2, radius, 0.7f);
        activeBallList.Add (ball);
        ball.OnPointerDownHandler += SplitBall;
        return ball;
    }

    private void SplitBall(Ball ball)
    {
        SoundBlaster.Instance.Play (SoundBlaster.SoundType.Split);

        float newArea = ball.Area / 2.0f;
        float newRadius = Mathf.Sqrt (newArea / Mathf.PI);
        ball.ChangeRadius (newRadius, 1);

        Ball newBall = CreateBall (newRadius);
        Vector3 pos = ball.transform.position;
        Vector2 randomPos = Random.insideUnitCircle * newRadius;
        pos.x += randomPos.x;
        pos.y += randomPos.y;
        newBall.transform.position = pos;

        // 50％の確率でもう一回呼ぶ
        if (Random.Range(0, 2) == 0) {
            SplitBall (ball);
        }
    }

    /// <summary>
    /// Unity FixedUpdate
    /// </summary>
    private void FixedUpdate ()
    {
        // 大きい順に処理
        activeBallList.Sort (CompareByArea);

        for (int i = 0, len = activeBallList.Count; i < len; i++) {
            Ball ball = activeBallList[i];
            if (ball.CurrentBallState == Ball.BallState.Vanishing) {
                continue;
            }
            if (ball.CollisionBall == null || ball.CollisionBall.CurrentBallState != Ball.BallState.Idle) {
                continue;
            }
            // 融合開始
            SoundBlaster.Instance.Play (SoundBlaster.SoundType.Merge);
            Ball collisionBall = ball.CollisionBall;
            float newArea = ball.Area + collisionBall.Area;
            float newRadius = Mathf.Sqrt (newArea / Mathf.PI);
            ball.ChangeRadius (newRadius, 1);

            // 取り込むボールの方向へ引かれる感じ
            // いまいち
            //float angle = Mathf.Atan2 (collisionBall.transform.position.y - transform.position.y, collisionBall.transform.position.x - transform.position.x);
            //Vector2 direction = new Vector2 (Mathf.Cos (angle), Mathf.Sign (angle));
            //ball.AddForce (direction * 10);

            // 取り込んだボールは消えてもらう
            collisionBall.OnPointerDownHandler -= SplitBall;
            collisionBall.Vanish ();
            vanishingBallList.Add (collisionBall);
        }

        for (int i = 0, len = vanishingBallList.Count; i < len; i++) {
            activeBallList.Remove (vanishingBallList[i]);
        }
        vanishingBallList.Clear ();
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
