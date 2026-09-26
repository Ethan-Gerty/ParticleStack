using UnityEngine;

public class PSBurstEmission : PSEmission
{
    [SerializeField, Min(0)] private int count = 10;

    private bool hasStarted;

    private void Start()
    {
        Burst();
        hasStarted = true;
    }

    private void OnEnable()
    {
        if (!hasStarted)
            return;

        Burst();
    }

    public void Burst()
    {
        int safeCount = Mathf.Max(0, count);

        for (int i = 0; i < safeCount; i++)
        {
            EmitParticle();
        }
    }
}
