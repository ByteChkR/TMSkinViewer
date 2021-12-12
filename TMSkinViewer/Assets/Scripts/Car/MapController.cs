using UnityEngine;

public class MapController : MonoBehaviour
{

    [SerializeField]
    private CarController m_CarController;
    
    private Light m_SunLight;

    [SerializeField]
    private Material m_GroundMaterial;
    [SerializeField]
    private MapPreset m_MapPreset;
    [SerializeField]
    private SurfacePreset m_SurfacePreset;
    [SerializeField]
    private CarPreset m_CarPreset;
    
    public void SetMapPreset(MapPreset preset)
    {
        m_SunLight.color = preset.LightColor;
        m_CarController.SetMapPreset(preset);
    }
    
    public void SetSurfacePreset(SurfacePreset preset)
    {
        m_GroundMaterial.color = preset.SurfaceColor;
        m_CarController.SetSurfacePreset(preset);
    }
    
    public void SetCarPreset(CarPreset preset) => m_CarController.SetCarPreset(preset);

    private void Awake()
    {
        m_SunLight = GameObject.Find( "SunLight" ).GetComponent < Light >();
    }

    private void Start()
    {
        UpdatePresets();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            UpdatePresets();
    }
    
    private void UpdatePresets()
    {
        SetMapPreset(m_MapPreset);
        SetSurfacePreset(m_SurfacePreset);
        SetCarPreset(m_CarPreset);
    }

}