using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SettingsPropertyWrapper
{

    public readonly string Name;
    private readonly object m_Instance;
    private readonly PropertyInfo m_Info;

    public SettingsHeaderAttribute Header { get; }

    public virtual Type Type => m_Info.PropertyType;

    public event Action < object > OnPropertyChanged;

    public virtual bool CanWrite => m_Info.CanWrite;

    public virtual object Value
    {
        get => m_Info.GetValue( m_Instance );
        set
        {
            m_Info.SetValue( m_Instance, value );
            OnPropertyChanged?.Invoke( value );
        }
    }

    #region Public

    public SettingsPropertyWrapper( string name, object instance, PropertyInfo info, SettingsHeaderAttribute header )
    {
        Name = name;
        m_Info = info;
        Header = header;
        m_Instance = instance;
    }

    public T GetCustomAttribute < T >() where T : Attribute
    {
        return GetCustomAttributes < T >().FirstOrDefault();
    }

    public virtual IEnumerable < T > GetCustomAttributes < T >() where T : Attribute
    {
        return m_Info.GetCustomAttributes < T >();
    }

    #endregion

}
