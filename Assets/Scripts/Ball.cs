using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボール的なもの。直径を x, y のスケールとする。
/// </summary>
public class Ball : MonoBehaviour
{
    private float _radius;
    
    /// <summary>
    /// 半径
    /// </summary>
    public float Radius
    {
        get
        {
            return _radius;
        }
        private set
        {
            _radius = value;
            _area = _radius * _radius * Mathf.PI;
        }
    }


    private float _area;

    /// <summary>
    /// 面積
    /// </summary>
    public float Area
    {
        get
        {
            return _area;
        }
        private set
        {
            _area = value;
            _radius = Mathf.Sqrt (_area / Mathf.PI);
        }
    }

    /// <summary>
    /// 指定半径で初期化
    /// </summary>
    /// <param name="radius"></param>
    public void Init(float radius)
    {
        Radius = radius;
        transform.localScale = Vector3.one * 2 * Radius;
    }
}
