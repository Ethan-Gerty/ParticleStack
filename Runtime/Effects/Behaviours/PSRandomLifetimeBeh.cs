using UnityEngine;

public class PSRandomLifetimeBeh : PSBehaviour
{
    [SerializeField] private Vector2 range;

    public override void OnParticleSpawn(ref PSParticle particle)
    {
        float min = Mathf.Min(range.x, range.y);
        float max = Mathf.Max(range.x, range.y);

        particle.lifeTime = Mathf.Max(0f, Random.Range(min, max));
    }
}
