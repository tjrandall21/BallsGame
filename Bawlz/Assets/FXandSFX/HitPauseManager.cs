using System.Collections;
using UnityEngine;

public class HitPauseManager : MonoBehaviour
{
    public static HitPauseManager Instance;

    [SerializeField, Tooltip("How long the balls freeze in seconds")] float pauseDuration = 0.1f;

    private void Awake() => Instance = this;

    public void TriggerHitPause(BallController a, BallController b)
    {
        StartCoroutine(HitPause(a.GetComponent<Rigidbody2D>(), b.GetComponent<Rigidbody2D>()));
    }

    private IEnumerator HitPause(Rigidbody2D rbA, Rigidbody2D rbB)
    {
        if (rbA == null || rbB == null) yield break;

        Vector2 velA = rbA.linearVelocity;
        Vector2 velB = rbB.linearVelocity;

        rbA.linearVelocity = Vector2.zero;
        rbB.linearVelocity = Vector2.zero;
        rbA.bodyType = RigidbodyType2D.Kinematic;
        rbB.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSecondsRealtime(pauseDuration);

        rbA.bodyType = RigidbodyType2D.Dynamic;
        rbB.bodyType = RigidbodyType2D.Dynamic;
        rbA.linearVelocity = velA;
        rbB.linearVelocity = velB;
    }
}