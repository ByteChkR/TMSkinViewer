using UnityEditor;

using UnityEngine;

[CustomEditor( typeof( NVWaterShaders ) )]
[CanEditMultipleObjects]
public class NVWaterShaderEditor : Editor
{

    private SerializedProperty garbageCollection;
    private SerializedProperty mirrorOn, mirrorBackSide, textureSize, clipPlaneOffset, reflectLayers;

    private SerializedProperty rotateSpeed, rotateDistance, depthTextureModeOn, windZone, waterSyncWind;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private Color smLineColor = Color.HSVToRGB( 0, 0, 0.55f ), bgLineColor = Color.HSVToRGB( 0, 0, 0.3f );
    private int smLinePadding = 20, bgLinePadding = 35;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void OnEnable()
    {
        //--------------

        rotateSpeed = serializedObject.FindProperty( "rotateSpeed" );
        rotateDistance = serializedObject.FindProperty( "rotateDistance" );
        depthTextureModeOn = serializedObject.FindProperty( "depthTextureModeOn" );
        waterSyncWind = serializedObject.FindProperty( "waterSyncWind" );
        windZone = serializedObject.FindProperty( "windZone" );
        mirrorOn = serializedObject.FindProperty( "mirrorOn" );
        mirrorBackSide = serializedObject.FindProperty( "mirrorBackSide" );
        textureSize = serializedObject.FindProperty( "textureSize" );
        clipPlaneOffset = serializedObject.FindProperty( "clipPlaneOffset" );
        reflectLayers = serializedObject.FindProperty( "reflectLayers" );
        garbageCollection = serializedObject.FindProperty( "garbageCollection" );

        //--------------
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void OnInspectorGUI()
    {
        //--------------

        serializedObject.Update();

        //--------------

        EditorGUI.BeginChangeCheck();
        NVWaterMaterials.Header();
        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );

        //--------------

        EditorGUILayout.LabelField( "Water Movement:", EditorStyles.boldLabel );
        NVWaterMaterials.DrawUILine( smLineColor, 1, smLinePadding );

        EditorGUILayout.PropertyField( rotateSpeed, new GUIContent( "Rotate Speed" ) );
        EditorGUILayout.PropertyField( rotateDistance, new GUIContent( "Movement Distance" ) );

        //--------------

        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );

        EditorGUILayout.LabelField( "Depth Texture Mode:", EditorStyles.boldLabel );
        NVWaterMaterials.DrawUILine( smLineColor, 1, smLinePadding );

        EditorGUILayout.PropertyField( depthTextureModeOn, new GUIContent( "Depth Texture Mode" ) );

        EditorGUILayout.HelpBox(
                                "!!! For working shaders on mobile platforms with Forward Rendering. If you use mobile platforms, enable HDR for proper operation (Project Settings / Graphics).",
                                MessageType.None
                               );

        //--------------

        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );

        EditorGUILayout.LabelField( "Wind Zone:", EditorStyles.boldLabel );
        NVWaterMaterials.DrawUILine( smLineColor, 1, smLinePadding );

        EditorGUILayout.PropertyField( waterSyncWind, new GUIContent( "Water Sync With Wind" ) );

        EditorGUILayout.PropertyField( windZone, new GUIContent( "Wind Zone Object" ) );

        EditorGUILayout.HelpBox(
                                "Optional. To synchronize the wind direction with the direction of water movement.",
                                MessageType.None
                               );

        //--------------

        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );

        EditorGUILayout.LabelField( "Mirror Reflection:", EditorStyles.boldLabel );
        NVWaterMaterials.DrawUILine( smLineColor, 1, smLinePadding );

        EditorGUILayout.PropertyField( mirrorOn, new GUIContent( "Mirror Reflection Enable" ) );
        EditorGUILayout.PropertyField( mirrorBackSide, new GUIContent( "Mirror Back Side" ) );
        EditorGUILayout.PropertyField( textureSize, new GUIContent( "Mirror Texture Size" ) );
        EditorGUILayout.PropertyField( clipPlaneOffset, new GUIContent( "Clipping plane offset" ) );
        EditorGUILayout.PropertyField( reflectLayers, new GUIContent( "Reflection Layers" ) );

        //--------------

        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );

        EditorGUILayout.PropertyField( garbageCollection, new GUIContent( "Garbage Collection" ) );

        //--------------

        serializedObject.ApplyModifiedProperties();

        NVWaterMaterials.DrawUILine( bgLineColor, 2, bgLinePadding );
        NVWaterMaterials.Information();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //--------------
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
