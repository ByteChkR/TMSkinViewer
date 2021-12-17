using System;

using UI.Settings;

using UnityEngine;

[SettingsCategory( "Testing/TestSettings" )]
public class TestSettings : ISettingsObject
{

    [SettingsProperty]
    public string TestString { get; set; }

    [SettingsProperty]
    public bool TestBool { get; set; }

    [SettingsProperty]
    public float TestDecimal { get; set; }

    [SettingsProperty]
    public int TestInteger { get; set; }

    [SettingsProperty]
    public Color TestColor { get; set; } = Color.white;

    [SettingsProperty]
    public Vector2 TestVector2 { get; set; }

    [SettingsProperty]
    public Vector3 TestVector3 { get; set; }

    [SettingsProperty]
    public SettingsValueInspectorType TestEnum { get; set; }

    [SettingsProperty]
    public string[] TestStringArray { get; set; } = Array.Empty < string >();

    [SettingsProperty]
    public bool[] TestBoolArray { get; set; } = Array.Empty < bool >();

    [SettingsProperty]
    public float[] TestDecimalArray { get; set; } = Array.Empty < float >();

    [SettingsProperty]
    public int[] TestIntegerArray { get; set; } = Array.Empty < int >();

    [SettingsProperty]
    public Color[] TestColorArray { get; set; } = Array.Empty < Color >();

    [SettingsProperty]
    public Vector2[] TestVector2Array { get; set; } = Array.Empty < Vector2 >();

    [SettingsProperty]
    public Vector3[] TestVector3Array { get; set; } = Array.Empty < Vector3 >();

    [SettingsProperty]
    public SettingsValueInspectorType[] TestEnumArray { get; set; } = Array.Empty < SettingsValueInspectorType >();

    #region Public

    public void OnSettingsChanged()
    {
        Debug.Log( "Test Settings Changed" );
    }

    #endregion

}
