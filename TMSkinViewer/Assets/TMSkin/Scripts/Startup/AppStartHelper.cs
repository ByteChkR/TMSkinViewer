using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using CSharpImageLibrary;
using CSharpImageLibrary.Headers;

using UI.Settings;

using UnityEngine;

public class AppStartHelper : MonoBehaviour
{

    public class TestSettings: ISettingsObject
    {
        [SettingsHeader("Boolean Tests")]
        [SettingsProperty]
        public bool Boolean { get; set; }
        [SettingsProperty]
        public bool[] BooleanArray { get; set; } = Array.Empty<bool>();
        [SettingsHeader("Integer Tests")]
        [SettingsProperty]
        public int Integer { get; set; }
        [SettingsProperty]
        public int[] IntegerArray { get; set; } = Array.Empty<int>();
        [SettingsHeader("Decimal Tests")]
        [SettingsProperty]
        public float Decimal { get; set; }
        [SettingsProperty]
        public float[] DecimalArray { get; set; } = Array.Empty<float>();
        [SettingsHeader("Texture Tests")]
        [SettingsProperty]
        public Texture2D Texture { get; set; } = Texture2D.whiteTexture;
        [SettingsProperty]
        public Texture2D[] TextureArray { get; set; } = Array.Empty<Texture2D>();
        [SettingsHeader("Enum Tests")]
        [SettingsProperty]
        public SettingsValueInspectorType Enum { get; set; } = SettingsValueInspectorType.String;
        [SettingsProperty]
        public SettingsValueInspectorType[] EnumArray { get; set; } = Array.Empty<SettingsValueInspectorType>();
        [SettingsProperty]
        [SettingsHeader("Color Tests")]
        public Color Color { get; set; } = Color.white;
        [SettingsProperty]
        public Color[] ColorArray { get; set; } = Array.Empty<Color>();
        
        [SettingsHeader("Vector Tests")]
        [SettingsProperty]
        public Vector2 Vector2 { get; set; }
        [SettingsProperty]
        public Vector3 Vector3 { get; set; }
        [SettingsProperty]
        public Vector4 Vector4 { get; set; }
        [SettingsProperty]
        public Vector2[] Vector2Array { get; set; } = Array.Empty<Vector2>();
        [SettingsProperty]
        public Vector3[] Vector3Array { get; set; } = Array.Empty<Vector3>();
        [SettingsProperty]
        public Vector4[] Vector4Array { get; set; } = Array.Empty<Vector4>();

        public void OnSettingsChanged()
        {
            Debug.Log("Test Settings Changed"  );
        }

        public void OnObjectLoaded()
        {
        }

    }
    [SerializeField]
    private AppStartSettings[] m_Apps;
    [SerializeField]
    private bool m_AddTestSettings;
    [SerializeField]
    private Texture2D m_TestTexture;

    [SerializeField]
    private string m_DefaultApp = "full";

    private void StartApp(AppStartSettings app, AppStartArgs args)
    {
        if ( m_AddTestSettings )
        {
            SettingsManager.AddSettingsObject(new TestSettings(), "Test");
        }
        
        GameObject instance = Instantiate( app.AppPrefab );
        
        app.StartApp(instance, args);

    }
    private void Start()
    {

        ImageEngineImage image = new ImageEngineImage( m_TestTexture.EncodeToTGA() );
        MemoryStream ms = new MemoryStream(); 
        image.Save(
                                                         ms,
                                                         new ImageFormats.ImageEngineFormatDetails(
                                                              ImageEngineFormat.DDS_DXT5
                                                             ),
                                                         MipHandling.Default
                                                        );
        AppStartArgs args = AppStartArgs.Args;

        string app;

        if ( args.ContainsKey( "app" ) )
        {
            app = args["app"];
        }
        else
        {
            app = m_DefaultApp;
        }
        
        foreach ( AppStartSettings appStartSettings in m_Apps )
        {
            if ( appStartSettings.AppName == app )
            {
                StartApp(appStartSettings, args);
                return;
            }
        }
    }

}
