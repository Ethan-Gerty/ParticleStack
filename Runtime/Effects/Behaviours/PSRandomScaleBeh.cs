using UnityEngine;

public class PSRandomScaleBeh : PSBehaviour
{
    [SerializeField] private Vector2 xRange = Vector2.one;
    [SerializeField] private Vector2 yRange = Vector2.one;

    public override void OnParticleSpawn(ref PSParticle particle)
    {
        float xMin = Mathf.Min(xRange.x, xRange.y);
        float xMax = Mathf.Max(xRange.x, xRange.y);
        float yMin = Mathf.Min(yRange.x, yRange.y);
        float yMax = Mathf.Max(yRange.x, yRange.y);

        Vector2 scale = new Vector2(
            Random.Range(xMin, xMax),
            Random.Range(yMin, yMax)
        );

        particle.scale = scale;
        particle.startScale = scale;
    }
}
