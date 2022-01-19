using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

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
