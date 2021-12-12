using System;
using System.Collections;
using System.Collections.Generic;

using SFB;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent( typeof( Button ) )]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler
{

    public Text output;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
    }

    // Called from browser
    public void OnFileUpload(string urls) {
        StartCoroutine(OutputRoutine(urls.Split(',')));
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
        // var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", true);
        string[] paths = StandaloneFileBrowser.OpenFilePanel( "Open File", "", "", true );

        if ( paths.Length > 0 )
        {
            List < string > urlArr = new List < string >( paths.Length );

            for ( int i = 0; i < paths.Length; i++ )
            {
                urlArr.Add( new Uri( paths[i] ).AbsoluteUri );
            }

            StartCoroutine( OutputRoutine( urlArr.ToArray() ) );
        }
    }
#endif

    private IEnumerator OutputRoutine( string[] urlArr )
    {
        string outputText = "";

        for ( int i = 0; i < urlArr.Length; i++ )
        {
            UnityWebRequest loader = new UnityWebRequest( urlArr[i] );

            yield return loader;

            outputText += loader.downloadHandler.text;
        }

        output.text = outputText;
    }

}
