using UnityEditor;
using UnityEngine;

public static class PSCreateParticleSystem
{
    [MenuItem("GameObject/ParticleStack/Particle System", false, 10)]
    private static void CreateParticleSystem(MenuCommand menuCommand)
    {
        GameObject particleSystem =
            new GameObject("PS ParticleSystem");

        // Parent it to the currently selected GameObject,
        // just like Unity's built-in GameObject creation tools.
        GameObjectUtility.SetParentAndAlign(
            particleSystem,
            menuCommand.context as GameObject
        );

        Undo.RegisterCreatedObjectUndo(
            particleSystem,
            "Create ParticleStack Particle System"
        );

        // Core ParticleStack components
        PSEmitter emitter =
            Undo.AddComponent<PSEmitter>(particleSystem);

        Undo.AddComponent<PSCircleShape>(particleSystem);
        Undo.AddComponent<PSOngoingEmission>(particleSystem);


        // Default ParticleStack resources
        Sprite defaultSprite =
            Resources.Load<Sprite>("Pixel");

        Material defaultMaterial =
            Resources.Load<Material>("ParticleMat");

        if (defaultSprite != null)
            emitter.sprite = defaultSprite;

        if (defaultMaterial != null)
            emitter.material = defaultMaterial;


        // Sensible starting particle values
        emitter.lifetime = 1f;
        emitter.particleSpeed = 1f;
        emitter.particleScale = Vector2.one;
        emitter.particleColour = Color.white;


        // maxParticles has a private setter,
        // so set its serialized backing field.
        SerializedObject serializedEmitter =
            new SerializedObject(emitter);

        SerializedProperty maxParticles =
            serializedEmitter.FindProperty(
                "<maxParticles>k__BackingField"
            );

        if (maxParticles != null)
            maxParticles.intValue = 1000;

        serializedEmitter.ApplyModifiedProperties();


        Selection.activeGameObject = particleSystem;
    }
}