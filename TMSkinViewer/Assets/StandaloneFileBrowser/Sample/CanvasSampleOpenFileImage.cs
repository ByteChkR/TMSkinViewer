using System;
using System.Collections;

using SFB;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent( typeof( Button ) )]
public class CanvasSampleOpenFileImage : MonoBehaviour, IPointerDownHandler
{

    public RawImage output;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".png, .jpg", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else

    //
    // Standalone platforms & editor
    //
    public void OnPointerDown( PointerEventData eventData )
    {
    }

    private void Start()
    {
        Button button = GetComponent < Button >();
        button.onClick.AddListener( OnClick );
    }

    private void OnClick()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel( "Title", "", ".png", false );

        if ( paths.Length > 0 )
        {
            StartCoroutine( OutputRoutine( new Uri( paths[0] ).AbsoluteUri ) );
        }
    }
#endif

    private IEnumerator OutputRoutine( string url )
    {
        UnityWebRequest loader = new UnityWebRequest( url );

        yield return loader;

        Texture2D texture = new Texture2D( 1, 1 );
        texture.LoadImage( loader.downloadHandler.data );
        output.texture = texture;
    }

}
