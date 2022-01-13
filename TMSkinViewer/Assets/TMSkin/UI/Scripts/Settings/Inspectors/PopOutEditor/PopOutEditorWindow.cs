using UI.Settings;

using UnityEngine;

public class PopOutEditorWindow : SettingsValueInspector
{

    [SerializeField]
    protected SettingsInspector m_Inspector;

    public void SetTarget( SettingsCategory category )
    {
        m_PropertyName.text = category.Name;
        m_Inspector.SetCategory( category );
    }

    public override void SetProperty( SettingsPropertyWrapper prop )
    {
    }

}
