using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;

    /// <summary>
    /// 再利用待ちボールリスト
    /// </summary>
    private List<Ball> cachedBallList = new List<Ball> () { Capacity = 16 };

    private int counter = 0;

    public Ball Create ()
    {
        Ball ball;
        if (cachedBallList.Count > 0) {
            // キャッシュにある
            ball = cachedBallList[cachedBallList.Count - 1];
            cachedBallList.RemoveAt(cachedBallList.Count - 1);
            ball.gameObject.SetActive (true);
        } else {
            // ない
            GameObject go = Instantiate (ballPrefab) as GameObject;
            go.transform.SetParent (transform);
            ball = go.GetComponent<Ball> ();
            ball.OnReleaseHandler += OnReleaseBall;
            go.name = "Ball_" + counter.ToString ();
            counter++;
        }
        return ball;
    }

    /// <summary>
    /// ボール解放ハンドラ
    /// </summary>
    /// <param name="ball"></param>
    private void OnReleaseBall (Ball ball)
    {
        // キャッシュに入れとく
        cachedBallList.Add (ball);
        ball.gameObject.SetActive (false);
    }

    private void Update ()
    {
        //Test ();
    }

    private void Test ()
    {
        if (Input.GetMouseButtonDown (0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            pos.z = 0;
            Ball ball = Create ();
            ball.Init (1.0f, Random.Range (0.5f, 1.5f), 0.7f);
            ball.transform.position = pos;
        }
    }
}
