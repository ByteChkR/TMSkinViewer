using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public class SettingsObjectWrapper
{

    private readonly object m_Object;

    private readonly SettingsPropertyWrapper[] m_Properties;

    public event Action OnObjectChanged;

    public IEnumerable < SettingsPropertyWrapper > Properties => m_Properties;

    #region Public

    public SettingsObjectWrapper( object o )
    {
        m_Object = o ??
                   throw new ArgumentNullException(
                                                   nameof( o ),
                                                   "Settings Object Wrapper can not be created on null object"
                                                  );

        m_Properties = CreateProperties();

        if ( o is ISettingsObject so )
        {
            OnObjectChanged += so.OnSettingsChanged;
        }
    }

    #endregion

    #region Private

    private SettingsPropertyWrapper[] CreateProperties()
    {
        List < SettingsPropertyWrapper > properties = new List < SettingsPropertyWrapper >();

        foreach ( PropertyInfo info in m_Object.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
        {
            SettingsPropertyAttribute attribute = info.GetCustomAttribute < SettingsPropertyAttribute >();

            if ( attribute != null )
            {
                properties.Add( CreateProperty( info, attribute ) );
            }
        }

        return properties.ToArray();
    }

    private SettingsPropertyWrapper CreateProperty( PropertyInfo info, SettingsPropertyAttribute attribute )
    {
        string name = attribute.Name ?? info.Name;

        Debug.Log( $"Adding Property '{name}'" );

        SettingsPropertyWrapper wrapper = new SettingsPropertyWrapper( name, m_Object, info );

        wrapper.OnPropertyChanged += OnPropertyChanged;

        return wrapper;
    }

    private void OnPropertyChanged( object obj )
    {
        OnObjectChanged?.Invoke();
    }

    #endregion

}
