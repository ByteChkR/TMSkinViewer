using System;
using System.Collections.Generic;
using System.Reflection;

public class SettingsObjectWrapper
{

    private readonly object m_Object;

    private readonly SettingsPropertyWrapper[] m_Properties;
    private readonly (string, MethodInfo)[] m_Methods;

    public object Instance => m_Object;

    public event Action OnObjectChanged;

    public IEnumerable < SettingsPropertyWrapper > Properties => m_Properties;

    public IEnumerable < (string, MethodInfo) > Methods => m_Methods;

    #region Public

    public SettingsObjectWrapper( object o )
    {
        m_Object = o ??
                   throw new ArgumentNullException(
                                                   nameof( o ),
                                                   "Settings Object Wrapper can not be created on null object"
                                                  );

        m_Properties = CreateProperties();
        m_Methods = CreateMethods();

        if ( o is ISettingsObject so )
        {
            OnObjectChanged += so.OnSettingsChanged;
        }
    }

    public void OnObjectLoaded()
    {
        if ( m_Object is ISettingsObject so )
        {
            so.OnObjectLoaded();
        }
    }

    #endregion

    #region Private

    private (string, MethodInfo)[] CreateMethods()
    {
        List < (string, MethodInfo) > methods = new List < (string, MethodInfo) >();

        foreach ( MethodInfo method in m_Object.GetType().GetMethods( BindingFlags.Instance | BindingFlags.Public ) )
        {
            SettingsButtonAttribute attribute = method.GetCustomAttribute < SettingsButtonAttribute >();

            if ( attribute != null )
            {
                methods.Add( ( attribute.Name ?? method.Name, method ) );
            }
        }

        return methods.ToArray();
    }

    private SettingsPropertyWrapper[] CreateProperties()
    {
        List < SettingsPropertyWrapper > properties = new List < SettingsPropertyWrapper >();

        foreach ( PropertyInfo info in m_Object.GetType().
                                                GetProperties(
                                                              BindingFlags.Public |
                                                              BindingFlags.Instance |
                                                              BindingFlags.FlattenHierarchy
                                                             ) )
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

        SettingsPropertyWrapper wrapper = new SettingsPropertyWrapper(
                                                                      name,
                                                                      m_Object,
                                                                      info,
                                                                      info.GetCustomAttribute <
                                                                          SettingsHeaderAttribute >()
                                                                     );

        wrapper.OnPropertyChanged += OnPropertyChanged;

        return wrapper;
    }

    private void OnPropertyChanged( object obj )
    {
        OnObjectChanged?.Invoke();
    }

    #endregion

}
