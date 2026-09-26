using UnityEngine;

public class PSConeShape : PSShape
{
    [SerializeField, Range(0f, 360f)] private float angle;
    [SerializeField] private float radius;

    public override void GetSpawnData(out Vector2 position, out Vector2 direction)
    {
        float halfAngle = angle * 0.5f;
        float randomAngle = Random.Range(-halfAngle, halfAngle);

        direction = ((Vector2)(Quaternion.Euler(0f, 0f, randomAngle) * transform.up)).normalized;
        position = (Vector2)transform.position + direction * radius;
    }
}
