using UnityEngine;

public class PSRandomColourBeh : PSBehaviour
{
    [SerializeField] private Color[] colours;

    public override void OnParticleSpawn(ref PSParticle particle)
    {
        if (colours == null || colours.Length == 0)
            return;

        Color colour = colours[Random.Range(0, colours.Length)];

        particle.colour = colour;
        particle.startColour = colour;
    }
}
