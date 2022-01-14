using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UI.LoadingWindow;

using UnityEngine;
using UnityEngine.Networking;

namespace UI.SkinEditorMainWindow
{

    [SettingsCategory( "Car Skin Settings" )]
    public class SkinDatabase : MonoBehaviour
    {

        private static SkinDatabase s_Instance;

        [SerializeField]
        private List < CarSkin > m_Skins = new List < CarSkin >();

        [SerializeField]
        private CarSkin m_DefaultSkin;

        private bool m_Initialize = false;

        [SettingsProperty]
        [SettingsHeader( "Skin Settings" )]
        public CarSkin DefaultSkin
        {
            get => m_DefaultSkin;
            set => m_DefaultSkin = value;
        }

        public static IEnumerable < CarSkin > LoadedSkins => s_Instance.m_Skins;

        [SettingsProperty]
        public CarSkin[] Skins
        {
            get => m_Skins.ToArray();
            set => m_Skins = new List < CarSkin >( value );
        }

        public static CarSkin Default => s_Instance.DefaultSkin;

        private void Awake()
        {
            s_Instance = this;
            SettingsManager.AddSettingsObject( this );
            ResourceSystem.AddOrigin( SkinImporter.SkinImporterResources );

            PrefabInitializeHelper helper = GetComponent < PrefabInitializeHelper >();

            helper.OnInitialized += TickInitialized;
        }

        private void Update()
        {
            if ( m_Initialize )
            {
                m_Initialize = false;
                ProcessImports();
            }
        }

        public event Action OnSkinDatabaseLoaded;

        private void TickInitialized()
        {
            m_Initialize = true;
        }

        private void ProcessImports()
        {
            AppStartArgs args = AppStartArgs.Args;

            if ( args.ContainsKey( "skin_imports" ) )
            {
                IEnumerable<SkinUrlImport> skinImports = SkinUrlImportContainer.
                                                            FromUrlArgument( args["skin_imports"] );

                ProcessImports( skinImports, OnSkinDatabaseLoaded );
            }
            else
            {
                OnSkinDatabaseLoaded?.Invoke();
            }
        }

        public static void ProcessImports( IEnumerable <SkinUrlImport> imports, Action onComplete = null )
        {
            s_Instance.StartCoroutine( ProcessImportsRoutine( imports, onComplete ) );
        }

        private static IEnumerator ProcessImportsRoutine(
            IEnumerable <SkinUrlImport> imports,
            Action onComplete = null )
        {
            LoadingWindow.LoadingWindow window = LoadingWindowBuilder.CreateWindow();
            List < SkinImporterArgs > importerArgs = new List < SkinImporterArgs >();

            foreach ( SkinUrlImport import in imports )
            {
                CarSkin skin = CreateSkin( import.Name, Default, true );
                UnityWebRequestAsyncOperation request = UnityWebRequest.Get( import.Url ).SendWebRequest();

                while ( request.webRequest.result == UnityWebRequest.Result.InProgress )
                {
                    window.SetStatus(
                                     ( int )( request.progress * 100 ),
                                     100,
                                     $"[{Math.Round( request.progress, 2 ) * 100}%] Downloading {import.Url}"
                                    );

                    yield return new WaitForEndOfFrame();
                }

                if ( request.webRequest.result != UnityWebRequest.Result.Success )
                {
                    Debug.LogError( request.webRequest.error );

                    continue;
                }

                importerArgs.Add(
                                 new SkinImporterArgs(
                                                      skin,
                                                      new MemoryStream( request.webRequest.downloadHandler.data ),
                                                      import.Name,
                                                      import.ImportAsDefault
                                                     )
                                );
            }

            TaskCollection tasks = new TaskCollection();

            SkinImporter.Import( importerArgs.ToArray(), tasks );

            foreach ( object o in window.ProcessRoutine( tasks ) )
            {
                yield return o;
            }

            onComplete?.Invoke();
        }

        public static event Action OnSkinDatabaseChanged;

        public static CarSkin CreateSkin( string name )
        {
            return CreateSkin( name, s_Instance.m_DefaultSkin );
        }

        public static CarSkin CreateSkin( string name, CarSkin template, bool createNewMaterials = false )
        {
            CarSkin skin = Instantiate( template );

            if ( createNewMaterials )
            {
                skin.Details = MaterialDatabase.CreateMaterial( $"{name}_Details", skin.Details );
                skin.Skin = MaterialDatabase.CreateMaterial( $"{name}_Skin", skin.Skin );
                skin.Glass = MaterialDatabase.CreateMaterial( $"{name}_Glass", skin.Glass );
                skin.Wheel = MaterialDatabase.CreateMaterial( $"{name}_Wheels", skin.Wheel );
            }

            s_Instance.m_Skins.Add( skin );
            skin.SkinName = name;
            OnSkinDatabaseChanged?.Invoke();

            return skin;
        }

    }

}
