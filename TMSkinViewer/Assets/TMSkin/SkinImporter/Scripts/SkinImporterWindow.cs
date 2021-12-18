using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using SFB;

using UI;
using UI.SkinEditorMainWindow;

using UnityEngine;
using UnityEngine.UI;

public class SkinImporterWindow : MonoBehaviour
{

    [SerializeField]
    private Window m_Window;

    [SerializeField]
    private InputField m_SkinName;

    [SerializeField]
    private Text m_Progress;

    [SerializeField]
    private Dropdown m_TemplateDropdown;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OpenDialog() {
        UploadFile(gameObject.name, "OnFileUpload", ".zip", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }

#else

    public void OpenDialog()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel( "Import Skin ZIP", "", "zip", true );

        if ( paths.Length > 0 )
        {
            ImportSkin( File.ReadAllBytes( paths[0] ) );
        }
    }

#endif
    private IEnumerator OutputRoutine( string url )
    {
        WWW loader = new WWW( url );

        yield return loader;

        if ( loader.error != null )
        {
            Debug.LogError( "Upload Error: " + loader.error );

            yield return new WaitForSeconds( 10 );

            m_Window.Close();
        }
        else
        {
            ImportSkin( loader.bytes );
        }
    }

    private void ImportSkin( byte[] data, TaskCollection collection ) =>
        ImportSkin( new MemoryStream( data ), collection );
    private void ImportSkin( Stream data, TaskCollection collection )
    {
        m_Progress.text = "Importing Skin...";

        CarSkin template = SkinDatabase.LoadedSkins.First(
                                                          x => x.SkinName ==
                                                               m_TemplateDropdown.options[m_TemplateDropdown.value].text
                                                         );

        CarSkin skin = SkinDatabase.CreateSkin( m_SkinName.text, template, true );
        SkinImporter.Import( skin, data, collection );
    }

    private void ImportSkin( byte[] data) =>
        ImportSkin( new MemoryStream( data ) );
    private void ImportSkin( Stream data )
    {
        m_Progress.text = "Importing Skin...";

        CarSkin template = SkinDatabase.LoadedSkins.First(
                                                          x => x.SkinName ==
                                                               m_TemplateDropdown.options[m_TemplateDropdown.value].text
                                                         );

        CarSkin skin = SkinDatabase.CreateSkin( m_SkinName.text, template, true );
        SkinImporter.Import( new SkinImporterArgs(skin, data, skin.SkinName), () => m_Window.Close() );

    }
    
    

    private void Start()
    {
        m_TemplateDropdown.options.AddRange( SkinDatabase.LoadedSkins.Select( x => new Dropdown.OptionData( x.SkinName ) ) );
    }

}
