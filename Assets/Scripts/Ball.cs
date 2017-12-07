using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボール的なもの。直径を x, y のスケールとする。
/// </summary>
public class Ball : MonoBehaviour
{
    public enum BallState
    {
        Idle = 0,
        Changing,
        Vanishing
    }

    public BallState CurrentBallState { get; private set; }

    /// <summary>
    /// タッチダウンデリゲート
    /// </summary>
    public UnityAction<Ball> OnPointerDownHandler;

    /// <summary>
    /// 解放デリゲート
    /// </summary>
    public UnityAction<Ball> OnReleaseHandler;

    /// <summary>
    /// 衝突してるボール
    /// </summary>
    public Ball CollisionBall { get; private set; }

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
    /// 初期半径と最終半径で初期化
    /// </summary>
    /// <param name="initialRadius"></param>
    /// <param name="targetRadius"></param>
    public void Init (float initialRadius, float targetRadius, float animationTime)
    {
        CurrentRadius = initialRadius;
        Radius = targetRadius;
        animationDuration = animationTime;

        animationProgressTime = 0;
        animationStartRadiusValue = initialRadius;
        animationChangeInRadiusValue = targetRadius - initialRadius;

        CurrentBallState = BallState.Changing;
    }

    /// <summary>
    /// 指定半径に変化
    /// </summary>
    /// <param name="targetRadius"></param>
    /// <param name="animationTime"></param>
    public void ChangeRadius (float targetRadius, float animationTime)
    {
        Init (CurrentRadius, targetRadius, animationTime);
    }

    /// <summary>
    /// 消去
    /// </summary>
    public void Vanish ()
    {
        CurrentBallState = BallState.Vanishing;

        // 解放
        if (OnReleaseHandler != null) {
            OnReleaseHandler.Invoke (this);
        }
    }

    /// <summary>
    /// Unity FixedUpdate
    /// </summary>
    private void FixedUpdate ()
    {
        if (Radius == CurrentRadius) {
            CurrentBallState = BallState.Idle;
            return;
        }

        animationProgressTime += Time.fixedDeltaTime;

        // 指定時間経過につき終了
        if (animationProgressTime >= animationDuration) {
            CurrentRadius = Radius;
            return;
        }

        CurrentRadius = Ease (animationProgressTime, animationStartRadiusValue, animationChangeInRadiusValue, animationDuration);
    }

    /// <summary>
    /// イージング
    /// </summary>
    /// <param name="currentTime"></param>
    /// <param name="startValue"></param>
    /// <param name="changeInValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private float Ease (float currentTime, float startValue, float changeInValue, float duration)
    {
        //return changeInValue * (-Mathf.Pow (2, -10 * currentTime / duration) + 1) + startValue;
        return EaseOutElastic (currentTime / duration) * changeInValue + startValue;
    }

    /// <summary>
    /// EaseOutElastic
    /// </summary>
    /// <param name="progress"></param>
    /// <returns></returns>
    private float EaseOutElastic (float progress)
    {
        if (progress == 0) {
            return 0.0f;
        }
        if (progress == 1) {
            return 1.0f;
        }
        float p = 0.3f;
        float a = 1.0f;
        float s = p / (2 * Mathf.PI);
        return a * Mathf.Pow (2, -10 * progress) * Mathf.Sin ((progress * 1 - s) * (2 * Mathf.PI) / p) + 1;
    }

    /// <summary>
    /// タッチダウン
    /// </summary>
    public void OnPointerDown ()
    {
        if (OnPointerDownHandler != null) {
            OnPointerDownHandler.Invoke (this);
        }
    }

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D (Collision2D collision)
    {
        // ボール以外は無視
        if (collision.gameObject.tag != "Ball") {
            return;
        }

        // 1個だけ保存
        CollisionBall = collision.gameObject.GetComponent<Ball> ();
    }
}
