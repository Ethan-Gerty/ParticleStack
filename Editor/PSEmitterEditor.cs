using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PSEmitter))]
public class PSEmitterEditor : Editor
{
    private bool showEmitter = true;
    private bool showParticle = true;
    private bool showRendering = true;
    private bool showRuntime = true;

    private SerializedProperty maxParticles;

    private SerializedProperty lifetime;
    private SerializedProperty particleSpeed;
    private SerializedProperty particleScale;
    private SerializedProperty particleColour;
    private SerializedProperty startRotation;
    private SerializedProperty startAngularVelocity;

    private SerializedProperty sprite;
    private SerializedProperty material;

    private const int MaxInstancesPerBatch = 1023;



    private void OnEnable()
    {
        maxParticles = serializedObject.FindProperty("<maxParticles>k__BackingField");
        lifetime = serializedObject.FindProperty("lifetime");
        particleSpeed = serializedObject.FindProperty("particleSpeed");
        particleScale = serializedObject.FindProperty("particleScale");
        particleColour = serializedObject.FindProperty("particleColour");
        startRotation = serializedObject.FindProperty("startRotation");
        startAngularVelocity = serializedObject.FindProperty("startAngularVelocity");
        sprite = serializedObject.FindProperty("sprite");
        material = serializedObject.FindProperty("material");
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PSEmitter emitter = (PSEmitter)target;

        showEmitter = EditorGUILayout.Foldout(showEmitter, "Emitter Settings", true);

        if (showEmitter)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(maxParticles);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();


        showParticle = EditorGUILayout.Foldout(showParticle, "Particle Settings", true);

        if (showParticle)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(lifetime);
            EditorGUILayout.PropertyField(particleSpeed);
            EditorGUILayout.PropertyField(particleScale);
            EditorGUILayout.PropertyField(particleColour);
            EditorGUILayout.PropertyField(startRotation);
            EditorGUILayout.PropertyField(startAngularVelocity);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();


        showRendering = EditorGUILayout.Foldout(showRendering, "Rendering Settings", true);

        if (showRendering)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(sprite);
            EditorGUILayout.PropertyField(material);

            EditorGUI.indentLevel--;
        }

        DrawAddButtons(emitter);


        if (Application.isPlaying)
        {
            EditorGUILayout.Space();

            showRuntime = EditorGUILayout.Foldout(
                showRuntime,
                "Runtime Info",
                true
            );

            if (showRuntime)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField(
                    "Active Particles",
                    $"{emitter.activeParticleCount} / {emitter.maxParticles}"
                );

                int batches = Mathf.CeilToInt(
                    emitter.activeParticleCount /
                    (float)MaxInstancesPerBatch
                );

                EditorGUILayout.LabelField(
                    "GPU Batches",
                    batches.ToString()
                );

                EditorGUI.indentLevel--;
            }

            Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }



    private void DrawAddButtons(PSEmitter emitter)
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Shape"))
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();

            Vector2 screenPosition =
                GUIUtility.GUIToScreenPoint(buttonRect.position);

            Rect screenRect = new Rect(
                screenPosition,
                buttonRect.size
            );

            ShowComponentMenu<PSShape>(
                emitter.gameObject,
                screenRect
            );
        }

        if (GUILayout.Button("Add Emission"))
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();

            Vector2 screenPosition =
                GUIUtility.GUIToScreenPoint(buttonRect.position);

            Rect screenRect = new Rect(
                screenPosition,
                buttonRect.size
            );

            ShowComponentMenu<PSEmission>(
                emitter.gameObject,
                screenRect
            );
        }

        if (GUILayout.Button("Add Behaviour"))
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();

            Vector2 screenPosition =
                GUIUtility.GUIToScreenPoint(buttonRect.position);

            Rect screenRect = new Rect(
                screenPosition,
                buttonRect.size
            );

            ShowComponentMenu<PSBehaviour>(
                emitter.gameObject,
                screenRect
            );
        }
    }

    private void ShowComponentMenu<T>(
    GameObject targetObject,
    Rect buttonRect
) where T : MonoBehaviour
    {
        GenericMenu menu = new GenericMenu();

        var types = TypeCache
            .GetTypesDerivedFrom<T>()
            .Where(type => !type.IsAbstract && !type.IsGenericType)
            .OrderBy(type => type.Name);

        bool foundType = false;

        foreach (System.Type type in types)
        {
            foundType = true;

            string displayName =
                ObjectNames.NicifyVariableName(type.Name);

            menu.AddItem(
                new GUIContent(displayName),
                false,
                () =>
                {
                    Undo.AddComponent(
                        targetObject,
                        type
                    );
                }
            );
        }

        if (!foundType)
        {
            menu.AddDisabledItem(
                new GUIContent("No Components Found")
            );
        }

        menu.DropDown(buttonRect);
    }
}