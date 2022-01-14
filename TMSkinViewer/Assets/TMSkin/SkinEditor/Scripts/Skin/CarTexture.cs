using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "New Car Texture", menuName = "TMSkin/Car Texture" )]
public class CarTexture : ScriptableObject
{

    [SerializeField]
    private byte[] m_TextureData;

    [SerializeField]
    private Texture2D m_Texture;

    [SerializeField]
    private bool m_IsDefault = false;

    public Texture2D Texture
    {
        get => m_Texture;
        set => m_Texture = value;
    }

    public byte[] TextureData
    {
        get => m_TextureData;
        set => m_TextureData = value;
    }
    
    public bool IsDefault
    {
        get => m_IsDefault;
        set => m_IsDefault = value;
    }

}
