using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    [HideInInspector] public Vector3 m_velocity;
    private Vector3 m_lastPoint;

    private void Start()
    {
        m_lastPoint = transform.localPosition;
    }

    void FixedUpdate()
    {
        m_velocity = (transform.localPosition - m_lastPoint);
    }
}