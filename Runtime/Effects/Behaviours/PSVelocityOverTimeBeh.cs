using UnityEngine;

public class PSVelocityOverTimeBeh : PSBehaviour
{
    [SerializeField] private Vector2 velocityTo;

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        if (particle.lifeTime <= 0f)
            return;

        float percentage = Mathf.Clamp01(particle.age / particle.lifeTime);
        particle.velocity = Vector2.Lerp(particle.startVelocity, velocityTo, percentage);
    }
}
