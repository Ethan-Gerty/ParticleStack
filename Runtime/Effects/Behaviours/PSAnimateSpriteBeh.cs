using UnityEngine;

public class PSAnimateSpriteBeh : PSBehaviour
{
    [SerializeField] private Sprite[] frames;

    private Vector4[] frameUVs;

    private void Awake()
    {
        CacheFrameUVs();
    }

    private void CacheFrameUVs()
    {
        if (frames == null || frames.Length == 0)
        {
            frameUVs = null;
            return;
        }

        PSEmitter targetEmitter = GetComponent<PSEmitter>();

        if (targetEmitter == null || targetEmitter.sprite == null)
        {
            Debug.LogWarning(
                $"{nameof(PSAnimateSpriteBeh)} requires the emitter to have a sprite assigned.",
                this
            );

            frameUVs = null;
            return;
        }

        Texture sourceTexture = targetEmitter.sprite.texture;
        Vector4 fallbackUV = GetUVRect(targetEmitter.sprite);

        frameUVs = new Vector4[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            Sprite frame = frames[i];

            if (frame == null)
            {
                frameUVs[i] = i > 0 ? frameUVs[i - 1] : fallbackUV;

                Debug.LogWarning(
                    $"{nameof(PSAnimateSpriteBeh)} has a missing sprite at frame {i}. The previous valid frame will be used instead.",
                    this
                );

                continue;
            }

            if (frame.texture != sourceTexture)
            {
                frameUVs[i] = i > 0 ? frameUVs[i - 1] : fallbackUV;

                Debug.LogWarning(
                    $"{nameof(PSAnimateSpriteBeh)} frame '{frame.name}' uses a different texture from the emitter sprite. " +
                    "All animation frames must use the same texture as the emitter sprite.",
                    this
                );

                continue;
            }

            frameUVs[i] = GetUVRect(frame);
        }
    }

    private static Vector4 GetUVRect(Sprite sprite)
    {
        Vector2[] uvs = sprite.uv;

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < uvs.Length; i++)
        {
            min = Vector2.Min(min, uvs[i]);
            max = Vector2.Max(max, uvs[i]);
        }

        Vector2 size = max - min;

        return new Vector4(min.x, min.y, size.x, size.y);
    }

    public override void OnParticleSpawn(ref PSParticle particle)
    {
        if (frameUVs == null || frameUVs.Length == 0)
            return;

        particle.uvRect = frameUVs[0];
    }

    public override void UpdateParticle(ref PSParticle particle, float deltaTime)
    {
        if (frameUVs == null || frameUVs.Length == 0 || particle.lifeTime <= 0f)
            return;

        float frameRate = frameUVs.Length / particle.lifeTime;
        int frame = Mathf.FloorToInt(particle.age * frameRate);

        frame = Mathf.Clamp(frame, 0, frameUVs.Length - 1);
        particle.uvRect = frameUVs[frame];
    }
}
