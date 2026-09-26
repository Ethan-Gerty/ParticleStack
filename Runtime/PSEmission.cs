using UnityEngine;

[RequireComponent(typeof(PSEmitter))]
public abstract class PSEmission : MonoBehaviour
{
    protected PSEmitter emitter;
    protected PSShape shape;

    protected virtual void Awake()
    {
        emitter = GetComponent<PSEmitter>();
        shape = GetComponent<PSShape>();

        if (shape == null)
        {
            Debug.LogError($"{GetType().Name} requires a PSShape on {gameObject.name}.", this);
        }
    }

    protected void EmitParticle()
    {
        if (shape == null)
            return;

        shape.GetSpawnData(out Vector2 position, out Vector2 direction);

        Vector2 velocity = direction.normalized * emitter.particleSpeed;

        PSParticle particle = new PSParticle
        {
            position = position,
            velocity = velocity,
            startVelocity = velocity,

            zRotation = emitter.startRotation,
            angularVelocity = emitter.startAngularVelocity,

            scale = emitter.particleScale,
            startScale = emitter.particleScale,

            colour = emitter.particleColour,
            startColour = emitter.particleColour,

            lifeTime = emitter.lifetime,
            age = 0f
        };

        emitter.Emit(particle);
    }
}
