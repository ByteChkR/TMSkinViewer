using System;
using System.Collections;
using System.Linq;

using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve m_TransitionCurve = AnimationCurve.Linear( 0, 0, 1, 1 );

    [SerializeField]
    private CameraControllerPosition[] m_CameraPositions;

    private int m_CameraIndex = 0;

    private Func < bool > m_EnableKeyboardInput;

    private bool m_IsTransitioning;
    private Transform m_Position;

    public Func < bool > EnableKeyboardInput
    {
        get => m_EnableKeyboardInput;
        set => m_EnableKeyboardInput = value;
    }

    private void Start()
    {
        NextCamera();
    }

    private void Update()
    {
        if ( !m_IsTransitioning && m_Position != null )
        {
            transform.SetPositionAndRotation( m_Position.position, m_Position.rotation );
        }

        bool enableKeyboardInput = m_EnableKeyboardInput?.Invoke() ?? true;

        if ( !enableKeyboardInput )
        {
            return;
        }

        CameraControllerPosition pos = m_CameraPositions.FirstOrDefault( x => Input.GetKeyDown( x.ActionKey ) );

        if ( pos != null )
        {
            ChangePosition( pos.TargetTransform );
        }
    }

    private IEnumerator CameraTransition( Transform target )
    {
        m_IsTransitioning = true;
        float t = 0;

        while ( t < 1 )
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp( transform.position, target.position, m_TransitionCurve.Evaluate( t ) );

            transform.rotation = Quaternion.Lerp(
                                                 transform.rotation,
                                                 target.rotation,
                                                 m_TransitionCurve.Evaluate( t )
                                                );

            yield return null;
        }

        m_IsTransitioning = false;
    }

    public void ChangePosition( Transform pos )
    {
        if ( m_IsTransitioning )
        {
            return;
        }

        m_Position = pos;
        StartCoroutine( CameraTransition( pos ) );
    }

    public void NextCamera()
    {
        int i = m_CameraIndex;
        m_CameraIndex++;

        if ( m_CameraIndex >= m_CameraPositions.Length )
        {
            m_CameraIndex = 0;
        }

        ChangePosition( m_CameraPositions[i].TargetTransform );
    }

}
