using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボール的なもの。直径を x, y のスケールとする。
/// </summary>
public class Ball : MonoBehaviour
{
    /// <summary>
    /// 半径
    /// </summary>
    private float _radius;

    /// <summary>
    /// 半径アクセサー
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

    /// <summary>
    /// 面積
    /// </summary>
    private float _area;

    /// <summary>
    /// 面積アクセサー
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
    /// 現在の半径
    /// </summary>
    private float _currentRadius;

    /// <summary>
    /// 現在の半径アクセサー
    /// </summary>
    public float CurrentRadius
    {
        get
        {
            return _currentRadius;
        }
        private set
        {
            _currentRadius = value;
            transform.localScale = Vector3.one * 2 * CurrentRadius;
        }
    }

    /// <summary>
    /// アニメーション期間
    /// </summary>
    private float animationDuration;

    /// <summary>
    /// アニメーション進行時間
    /// </summary>
    private float animationProgressTime;

    /// <summary>
    /// アニメーション開始時半径
    /// </summary>
    private float animationStartRadiusValue;

    /// <summary>
    /// アニメーション量
    /// </summary>
    private float animationChangeInRadiusValue;

    /// <summary>
    /// 指定半径で初期化
    /// </summary>
    /// <param name="initialRadius"></param>
    public void Init(float initialRadius)
    {
        CurrentRadius = Radius = initialRadius;
    }

    /// <summary>
    /// 初期半径と最終半径で初期化
    /// </summary>
    /// <param name="initialRadius"></param>
    /// <param name="targetRadius"></param>
    public void Init(float initialRadius, float targetRadius, float animationTime)
    {
        CurrentRadius = initialRadius;
        Radius = targetRadius;
        animationDuration = animationTime;

        animationProgressTime = 0;
        animationStartRadiusValue = initialRadius;
        animationChangeInRadiusValue = targetRadius - initialRadius;
    }

    /// <summary>
    /// Unity FixedUpdate
    /// </summary>
    private void FixedUpdate ()
    {
        if (Radius == CurrentRadius) {
            return;
        }

        animationProgressTime += Time.fixedDeltaTime;

        // 指定時間経過につき終了
        if (animationProgressTime >= animationDuration) {
            CurrentRadius = Radius;
            return;
        }

        CurrentRadius = ease (animationProgressTime, animationStartRadiusValue, animationChangeInRadiusValue, animationDuration);
    }

    private float ease (float currentTime, float startValue, float changeInValue, float duration)
    {
        return changeInValue * (-Mathf.Pow (2, -10 * currentTime / duration) + 1) + startValue;
    }
}
