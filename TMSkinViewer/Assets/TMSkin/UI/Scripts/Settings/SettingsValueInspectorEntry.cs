using System;

using UnityEngine;

namespace UI.Settings
{

    [Serializable]
    public class SettingsValueInspectorEntry
    {

        public SettingsValueInspectorType Type;
        public GameObject InspectorPrefab;

        public virtual void OnCreate( GameObject inspector, object instance )
        {
        }

        public virtual bool IsValidInspectorPropertyType( Type t )
        {
            return t == typeof( string ) && Type == SettingsValueInspectorType.String ||
                   t == typeof( bool ) && Type == SettingsValueInspectorType.Boolean ||
                   t == typeof( int ) && Type == SettingsValueInspectorType.Integer ||
                   t == typeof( float ) && Type == SettingsValueInspectorType.Decimal ||
                   t.IsEnum && Type == SettingsValueInspectorType.Enum ||
                   t == typeof( Color ) && Type == SettingsValueInspectorType.Color ||
                   t == typeof( Vector2 ) && Type == SettingsValueInspectorType.Vector2 ||
                   t == typeof( Vector3 ) && Type == SettingsValueInspectorType.Vector3 ||
                   t == typeof( Vector4 ) && Type == SettingsValueInspectorType.Vector4 ||
                   t.IsArray && Type == SettingsValueInspectorType.Array ||
                   t == typeof( Texture2D ) && Type == SettingsValueInspectorType.Texture;
        }

    }

}
