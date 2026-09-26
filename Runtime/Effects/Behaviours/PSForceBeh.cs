using UnityEngine;

public class PSForceBeh : PSBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float force;

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        particle.velocity += direction.normalized * force * deltaTime;
    }
}