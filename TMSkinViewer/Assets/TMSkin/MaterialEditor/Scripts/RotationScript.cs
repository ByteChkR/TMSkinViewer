using UnityEngine;

public class RotationScript : MonoBehaviour
{

    [SerializeField]
    private float m_RotationSpeed = 1.0f;

    private void Update()
    {
        transform.Rotate( Vector3.up, m_RotationSpeed * Time.deltaTime );
    }

}
