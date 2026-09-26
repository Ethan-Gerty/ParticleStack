using UnityEngine;

public struct PSParticle
{
    public Vector4 uvRect;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 startVelocity;

    public float zRotation;
    public float angularVelocity;

    public Vector2 scale;
    public Vector2 startScale;

    public Color colour;
    public Color startColour;

    public float lifeTime;
    public float age;

    public bool isAlive => age < lifeTime;
}
