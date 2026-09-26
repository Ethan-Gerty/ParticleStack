using UnityEngine;

public class PSScaleOverTimeBeh : PSBehaviour
{
    [SerializeField] private Vector2 scaleTo = Vector2.zero;

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        if (particle.lifeTime <= 0f)
            return;

        float percentage = Mathf.Clamp01(particle.age / particle.lifeTime);
        particle.scale = Vector2.Lerp(particle.startScale, scaleTo, percentage);
    }
}
