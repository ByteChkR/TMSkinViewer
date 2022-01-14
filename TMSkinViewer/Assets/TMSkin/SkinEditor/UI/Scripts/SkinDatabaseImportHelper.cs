using System.Linq;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinDatabaseImportHelper : MonoBehaviour
    {

        public SkinUrlImport[] Items;

        private void Awake()
        {
            GetComponent < PrefabInitializeHelper >().OnInitialized += () =>
                                                                       {
                                                                           SkinDatabase.ProcessImports(
                                                                                Items
                                                                               );
                                                                       };
        }

    }

}
