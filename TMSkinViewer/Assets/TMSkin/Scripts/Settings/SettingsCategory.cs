using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SettingsCategory
{

    private readonly SettingsCategory m_Parent;

    private readonly List < SettingsCategory > m_Categories = new List < SettingsCategory >();
    private readonly List < SettingsObjectWrapper > m_Objects = new List < SettingsObjectWrapper >();

    public string Name { get; }

    public string Path => m_Parent == null ? Name : $"{m_Parent.Path}/{Name}";

    public IEnumerable < SettingsCategory > Categories => m_Categories;

    public IEnumerable < SettingsObjectWrapper > Objects => m_Objects;

    #region Public

    public SettingsCategory( string name, SettingsCategory parent )
    {
        Name = name;
        m_Parent = parent;
    }

    public SettingsCategory AddChild( string name )
    {
        if ( HasChild( name ) )
        {
            throw new Exception( $"Settings Category {Path}/{name} does already exist" );
        }

        SettingsCategory c = new SettingsCategory( name, this );
        m_Categories.Add( c );

        return c;
    }

    public void AddSettingsObject( object o )
    {
        SettingsObjectWrapper wrapper = new SettingsObjectWrapper( o );
        m_Objects.Add( wrapper );
        wrapper.OnObjectLoaded();

    }

    public SettingsCategory GetChild( string name )
    {
        return m_Categories.FirstOrDefault( x => x.Name == name );
    }

    public bool HasChild( string name )
    {
        return m_Categories.Any( x => x.Name == name );
    }

    public void PrintDebug()
    {
        StringBuilder sb = new StringBuilder();

        foreach ( SettingsObjectWrapper wrapper in m_Objects )
        {
            foreach ( SettingsPropertyWrapper property in wrapper.Properties )
            {
                sb.AppendLine( $"{property.Name}: {property.Value ?? "NULL"}" );
            }
        }

        Debug.Log( $"Category: {Path}\n{sb}" );

        foreach ( SettingsCategory category in m_Categories )
        {
            category.PrintDebug();
        }
    }

    #endregion

}
