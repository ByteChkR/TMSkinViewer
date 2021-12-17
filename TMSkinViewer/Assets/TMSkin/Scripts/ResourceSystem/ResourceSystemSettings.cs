using System;
using System.Linq;

using UnityEngine;

[Serializable]
[SettingsCategory("Resources")]
public class ResourceSystemSettings
{

    public event Action OnReloadOrigins;
    
    [SerializeField]
    private string[] m_Origins = Array.Empty<string>();
    [SettingsProperty]
    public string[] Origins
    {
        get => m_Origins;
        set => m_Origins = value;
    }
    
    [SettingsButton]
    public void ReloadOrigins()
    {
        OnReloadOrigins?.Invoke();
    }
    public Uri[] Uris => Origins.Select(x=>new Uri(x)).ToArray();

    
}