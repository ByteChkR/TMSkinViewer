using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

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
                                                                                Items.ToDictionary(
                                                                                     x => x.Name,
                                                                                     x => x.Url
                                                                                    )
                                                                               );
                                                                       };
        }

    }

}