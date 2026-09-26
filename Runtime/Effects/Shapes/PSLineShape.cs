using UnityEngine;

public class PSLineShape : PSShape
{
    [SerializeField] private float lineSize;

    public override void GetSpawnData(out Vector2 position, out Vector2 direction)
    {
        float half = Mathf.Abs(lineSize) * 0.5f;
        float randomSpawn = Random.Range(-half, half);

        position = (Vector2)transform.position + (Vector2)transform.right * randomSpawn;
        direction = transform.up;
    }
}
