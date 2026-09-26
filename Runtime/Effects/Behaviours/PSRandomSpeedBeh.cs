using UnityEngine;

public class PSRandomSpeedBeh : PSBehaviour
{
    [SerializeField] private Vector2 range;

    public override void OnParticleSpawn(ref PSParticle particle)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);
        float speed = Random.Range(min, max);

        Vector2 velocity = particle.velocity.normalized * speed;

        particle.velocity = velocity;
        particle.startVelocity = velocity;
    }
}
