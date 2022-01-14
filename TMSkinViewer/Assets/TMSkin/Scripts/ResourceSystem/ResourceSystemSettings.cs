using System;
using System.Linq;

using UnityEngine;

[Serializable]
[SettingsCategory( "Resources" )]
public class ResourceSystemSettings
{

    [SerializeField]
    private string[] m_Origins = Array.Empty < string >();

    [SettingsProperty]
    public string[] Origins
    {
        get => m_Origins;
        set => m_Origins = value;
    }

    public Uri[] Uris => Origins.Select( x => new Uri( x ) ).ToArray();

    public event Action OnReloadOrigins;

    [SettingsButton("Reload")]
    public void ReloadOrigins()
    {
        OnReloadOrigins?.Invoke();
    }

}
