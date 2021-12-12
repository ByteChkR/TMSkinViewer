using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    [SerializeField]
    private float m_RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround( transform.parent.position, Vector3.up, m_RotationSpeed * Time.deltaTime );
        transform.LookAt(transform.parent.position);
    }
}
