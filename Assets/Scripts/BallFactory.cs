using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;

    public Ball Create (float radius)
    {
        GameObject go = Instantiate (ballPrefab) as GameObject;
        go.transform.SetParent (transform);
        Ball ball = go.GetComponent<Ball> ();
        ball.Init (radius / 2, radius, 0.2f);
        return ball;
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
            Ball ball = Create (Random.Range (0.5f, 1.5f));
            ball.transform.position = pos;
        }
    }
}
