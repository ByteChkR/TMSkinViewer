using UnityEngine;

[SettingsCategory( "Skin Exporter" )]
public class SkinExporterSettings : ScriptableObject
{

    [SettingsProperty("Export Default Textures")]
    public bool ExportDefaultTextures { get; set; } = false;

}
