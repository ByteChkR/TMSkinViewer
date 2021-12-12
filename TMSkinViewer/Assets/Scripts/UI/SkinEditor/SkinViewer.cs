﻿using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinViewer : MonoBehaviour
    {

        [SerializeField]
        private MapController m_MapController;

        [SerializeField]
        private CarController m_CarController;

        [SerializeField]
        private CameraController m_CameraController;
        [SerializeField]
        private Renderer m_WheelRenderer;
        [SerializeField]
        private Renderer m_DetailRenderer;
        [SerializeField]
        private Renderer m_SkinRenderer;
        [SerializeField]
        private Renderer m_GlassRenderer;

        public MapController MapController => m_MapController;

        public CarController CarController => m_CarController;

        private void Awake()
        {
            m_WheelRenderer.material = Instantiate(m_WheelRenderer.material);
            m_DetailRenderer.material = Instantiate(m_DetailRenderer.material);
            m_SkinRenderer.material = Instantiate(m_SkinRenderer.material);
            m_GlassRenderer.material = Instantiate(m_GlassRenderer.material);
        }

        private void ApplyMaterial( Material material, CarMaterial carMaterial )
        {
            material.SetTexture("_MainTex", carMaterial.Albedo);
            material.SetTexture("_AO", carMaterial.AmbientOcclusion);
            material.SetTexture("_Normals", carMaterial.Normal);
            material.SetTexture("_Emission", carMaterial.Emissive);
            material.SetTexture("_DirtMask", carMaterial.DirtMask);
            material.SetTexture("_Roughness", carMaterial.Roughness);
        }
        
        public void SetSkin( CarSkin skin )
        {
            ApplyMaterial(m_WheelRenderer.material, skin.Wheel);
            ApplyMaterial(m_DetailRenderer.material, skin.Details);
            ApplyMaterial(m_SkinRenderer.material, skin.Skin);
            ApplyMaterial(m_GlassRenderer.material, skin.Glass);
        }

        public void NextCamera()
        {
            m_CameraController.NextCamera();
        }
    }

}