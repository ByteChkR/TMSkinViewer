using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class SettingsManager
{

    private static readonly List < SettingsCategory > s_Categories = new List < SettingsCategory >();

    public static IEnumerable < SettingsCategory > Categories => s_Categories;

    public static IEnumerable < SettingsCategory > AllCategories
    {
        get
        {
            foreach ( SettingsCategory category in s_Categories )
            {
                yield return category;

                foreach ( SettingsCategory child in GetAllCategories( category ) )
                {
                    yield return child;
                }
            }
        }
    }

    #region Public

    public static void AddSettingsObject( object o, string path = null )
    {
        SettingsCategoryAttribute attribute = o.GetType().GetCustomAttribute < SettingsCategoryAttribute >();

        if ( path == null )
        {
            if ( attribute == null )
            {
                throw new Exception( $"Object {o} does not have a SettingsCategoryAttribute" );
            }

            path = attribute.Path;
        }

        SettingsCategory cat = GetOrCreate( path );

        cat.AddSettingsObject( o );
    }

    public static SettingsCategory FindCategory( string path )
    {
        string[] parts = path.Split( '/' );

        SettingsCategory current = s_Categories.FirstOrDefault( x => x.Name == parts.First() );

        for ( int i = 1; i < parts.Length; i++ )
        {
            current = current?.GetChild( parts[i] );
        }

        return current;
    }

    public static bool HasCategory( string path )
    {
        SettingsCategory cat = FindCategory( path );

        return cat != null;
    }

    public static void PrintDebug()
    {
        foreach ( SettingsCategory category in s_Categories )
        {
            category.PrintDebug();
        }
    }

    #endregion

    #region Private

    private static IEnumerable < SettingsCategory > GetAllCategories( SettingsCategory category )
    {
        foreach ( SettingsCategory cat in category.Categories )
        {
            yield return cat;

            foreach ( SettingsCategory child in GetAllCategories( cat ) )
            {
                yield return child;
            }
        }
    }

    private static SettingsCategory GetOrCreate( string path )
    {
        string[] parts = path.Split( '/' );

        SettingsCategory current = s_Categories.FirstOrDefault( x => x.Name == parts.First() );

        if ( current == null )
        {
            current = new SettingsCategory( parts.First(), null );
            s_Categories.Add( current );
        }

        for ( int i = 1; i < parts.Length; i++ )
        {
            current = current.GetChild( parts[i] ) ?? current.AddChild( parts[i] );
        }

        return current;
    }

    #endregion

}
