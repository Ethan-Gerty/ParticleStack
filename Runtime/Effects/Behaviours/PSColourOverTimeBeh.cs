using UnityEngine;

public class PSColourOverTimeBeh : PSBehaviour
{
    [SerializeField] private Color colourTo = Color.clear;

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        if (particle.lifeTime <= 0f)
            return;

        float percentage = Mathf.Clamp01(particle.age / particle.lifeTime);
        particle.colour = Color.Lerp(particle.startColour, colourTo, percentage);
    }
}
