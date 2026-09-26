using UnityEngine;

public class PSOngoingEmission : PSEmission
{
    [SerializeField, Min(0f)] private float spawnRate = 10f;

    private float spawnAccumulator;

    private void Update()
    {
        if (spawnRate <= 0f)
            return;

        spawnAccumulator += spawnRate * Time.deltaTime;

        int particlesToSpawn = Mathf.FloorToInt(spawnAccumulator);

        if (particlesToSpawn <= 0)
            return;

        spawnAccumulator -= particlesToSpawn;

        for (int i = 0; i < particlesToSpawn; i++)
        {
            EmitParticle();
        }
    }
}
