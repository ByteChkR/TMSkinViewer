using System;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings.ColorPicker
{

    public class ColorPickerWindow : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private Slider m_AlphaSlider;

        [SerializeField]
        private Slider m_RedSlider;

        [SerializeField]
        private Slider m_GreenSlider;

        [SerializeField]
        private Slider m_BlueSlider;

        [SerializeField]
        private InputField m_AlphaInput;

        [SerializeField]
        private InputField m_RedInput;

        [SerializeField]
        private InputField m_GreenInput;

        [SerializeField]
        private InputField m_BlueInput;

        [SerializeField]
        private Color m_Color;

        [SerializeField]
        private Image m_PreviewImage;

        private void Start()
        {
            OnColorChanged += SetImageColor;
            m_AlphaInput.onValueChanged.AddListener( x => m_AlphaSlider.SetValueWithoutNotify( float.Parse( x ) ) );
            m_AlphaSlider.onValueChanged.AddListener( x => m_AlphaInput.SetTextWithoutNotify( x.ToString() ) );

            m_RedInput.onValueChanged.AddListener( x => m_RedSlider.SetValueWithoutNotify( float.Parse( x ) ) );
            m_RedSlider.onValueChanged.AddListener( x => m_RedInput.SetTextWithoutNotify( x.ToString() ) );

            m_GreenInput.onValueChanged.AddListener( x => m_GreenSlider.SetValueWithoutNotify( float.Parse( x ) ) );
            m_GreenSlider.onValueChanged.AddListener( x => m_GreenInput.SetTextWithoutNotify( x.ToString() ) );

            m_BlueInput.onValueChanged.AddListener( x => m_BlueSlider.SetValueWithoutNotify( float.Parse( x ) ) );
            m_BlueSlider.onValueChanged.AddListener( x => m_BlueInput.SetTextWithoutNotify( x.ToString() ) );
        }

        public void Initialize( Color color )
        {
            m_Color = color;
            UpdateSliders();
            UpdateInputText();
        }

        private void SetImageColor( Color color )
        {
            m_PreviewImage.color = color;
        }

        private void UpdateSliders()
        {
            m_AlphaSlider.SetValueWithoutNotify( m_Color.a );
            m_RedSlider.SetValueWithoutNotify( m_Color.r );
            m_GreenSlider.SetValueWithoutNotify( m_Color.g );
            m_BlueSlider.SetValueWithoutNotify( m_Color.b );
        }

        private void UpdateInputText()
        {
            m_AlphaInput.SetTextWithoutNotify( m_Color.a.ToString() );
            m_RedInput.SetTextWithoutNotify( m_Color.r.ToString() );
            m_GreenInput.SetTextWithoutNotify( m_Color.g.ToString() );
            m_BlueInput.SetTextWithoutNotify( m_Color.b.ToString() );
        }

        public void SetColor( Color color )
        {
            m_Color = color;
            OnColorChanged?.Invoke( m_Color );
        }

        public event Action < Color > OnColorChanged;

        public void SetRed( string value )
        {
            SetRed( float.Parse( value ) );
        }

        public void SetRed( float value )
        {
            m_Color.r = value;
            OnColorChanged?.Invoke( m_Color );
        }

        public void SetGreen( string value )
        {
            SetGreen( float.Parse( value ) );
        }

        public void SetGreen( float value )
        {
            m_Color.g = value;
            OnColorChanged?.Invoke( m_Color );
        }

        public void SetBlue( string value )
        {
            SetBlue( float.Parse( value ) );
        }

        public void SetBlue( float value )
        {
            m_Color.b = value;
            OnColorChanged?.Invoke( m_Color );
        }

        public void SetAlpha( string value )
        {
            SetAlpha( float.Parse( value ) );
        }

        public void SetAlpha( float value )
        {
            m_Color.a = value;
            OnColorChanged?.Invoke( m_Color );
        }

    }

}
