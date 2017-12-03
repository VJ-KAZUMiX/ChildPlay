using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面端の壁生成用
/// </summary>
public class WallFactory : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject wallPrefab;

    private void Awake ()
    {
        create ();
    }

    private void create ()
    {
        Vector2 min = mainCamera.ViewportToWorldPoint (Vector2.zero);
        Vector2 max = mainCamera.ViewportToWorldPoint (Vector2.one);

        GameObject wall;
        float offsetX = 0.25f;
        float offsetY = 0.25f;
        float scaleX = max.x - min.x;
        float scaleY = max.y - min.y;
        // 左
        wall = Instantiate (wallPrefab) as GameObject;
        wall.transform.SetParent (transform);
        wall.transform.position = new Vector3 (min.x - offsetX, 0);
        wall.transform.localScale = new Vector3 (1, scaleY, 1);
        // 右
        wall = Instantiate (wallPrefab) as GameObject;
        wall.transform.SetParent (transform);
        wall.transform.position = new Vector3 (max.x + offsetX, 0);
        wall.transform.localScale = new Vector3 (1, scaleY, 1);
        // 上
        wall = Instantiate (wallPrefab) as GameObject;
        wall.transform.SetParent (transform);
        wall.transform.position = new Vector3 (0, max.y + offsetY);
        wall.transform.localScale = new Vector3 (scaleX, 1, 1);
        // 舌
        wall = Instantiate (wallPrefab) as GameObject;
        wall.transform.SetParent (transform);
        wall.transform.position = new Vector3 (0, min.y - offsetY);
        wall.transform.localScale = new Vector3 (scaleX, 1, 1);
    }
}
