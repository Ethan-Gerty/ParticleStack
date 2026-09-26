using UnityEngine;

public class PSBoxShape : PSShape
{
    [SerializeField] private Vector2 boxSize = Vector2.one;

    public override void GetSpawnData(out Vector2 position, out Vector2 direction)
    {
        float halfWidth = Mathf.Abs(boxSize.x) * 0.5f;
        float halfHeight = Mathf.Abs(boxSize.y) * 0.5f;

        Vector2 localPosition;
        Vector2 localDirection;

        switch (Random.Range(0, 4))
        {
            case 0:
                localPosition = new Vector2(Random.Range(-halfWidth, halfWidth), halfHeight);
                localDirection = Vector2.up;
                break;

            case 1:
                localPosition = new Vector2(Random.Range(-halfWidth, halfWidth), -halfHeight);
                localDirection = Vector2.down;
                break;

            case 2:
                localPosition = new Vector2(halfWidth, Random.Range(-halfHeight, halfHeight));
                localDirection = Vector2.right;
                break;

            default:
                localPosition = new Vector2(-halfWidth, Random.Range(-halfHeight, halfHeight));
                localDirection = Vector2.left;
                break;
        }

        position = (Vector2)transform.position
                   + (Vector2)transform.right * localPosition.x
                   + (Vector2)transform.up * localPosition.y;

        direction = ((Vector2)transform.right * localDirection.x
                     + (Vector2)transform.up * localDirection.y).normalized;
    }
}
