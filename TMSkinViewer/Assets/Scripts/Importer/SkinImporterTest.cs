using System.IO;

using JetBrains.Annotations;

using SFB;

using UI.SkinEditorMainWindow;

using UnityEngine;

public class SkinImporterTest : MonoBehaviour
{

    [SerializeField]
    [CanBeNull]
    private CarSkin m_SkinTemplate;

    private void Start()
    {
        string[] files = StandaloneFileBrowser.OpenFilePanel( "Open File", "", "zip", false );

        foreach ( string file in files )
        {
            Debug.Log( $"Importing File {file}" );
            CarSkin skin = SkinDatabase.CreateSkin( "Test", m_SkinTemplate, true );
            SkinImporter.Import( skin, File.OpenRead( file ) );
        }
    }

}
