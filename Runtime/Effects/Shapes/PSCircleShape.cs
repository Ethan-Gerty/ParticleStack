using UnityEngine;

public class PSCircleShape : PSShape
{
    [SerializeField] private Vector2 radiusRange;

    public override void GetSpawnData(out Vector2 position, out Vector2 direction)
    {
        float minRadius = Mathf.Min(radiusRange.x, radiusRange.y);
        float maxRadius = Mathf.Max(radiusRange.x, radiusRange.y);
        float radius = Random.Range(minRadius, maxRadius);

        float angle = Random.Range(0f, Mathf.PI * 2f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        position = (Vector2)transform.position + direction * radius;
    }
}
