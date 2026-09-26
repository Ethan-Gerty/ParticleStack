using UnityEngine;

public class PSDragBeh : PSBehaviour
{
    [SerializeField, Min(0f)] private float drag = 1f;

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        float safeDrag = Mathf.Max(0f, drag);
        particle.velocity *= Mathf.Exp(-safeDrag * deltaTime);
    }
}
