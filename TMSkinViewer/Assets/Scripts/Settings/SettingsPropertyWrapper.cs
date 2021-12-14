using System;
using System.Reflection;

public class SettingsPropertyWrapper
{


    public readonly string Name;
    private readonly object m_Instance;
    private readonly PropertyInfo m_Info;

    public virtual Type Type => m_Info.PropertyType;

    public event Action < object > OnPropertyChanged;

    public virtual object Value
    {
        get => m_Info.GetValue( m_Instance );
        set {
            m_Info.SetValue( m_Instance, value );
            OnPropertyChanged?.Invoke(value);
        }
    }

    public SettingsPropertyWrapper( string name, object instance, PropertyInfo info )
    {
        Name = name;
        m_Info = info;
        m_Instance = instance;
    }


}