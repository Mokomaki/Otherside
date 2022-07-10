#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] float m_threshold;
    [SerializeField] Vector3[] m_pathNodes;
    [SerializeField] bool m_LookAtPlayer = true;
    private int m_curIndex = 0;
    private float m_startTime;
    private GameObject m_player;
    private float m_startScale;
    private Vector3 m_target;
    void Start()
    {
        m_startScale = transform.localScale.x;
        m_player = GameObject.FindWithTag("Player");
        for(int i = 0; i<m_pathNodes.Length; i++)
        {
            m_pathNodes[i] = transform.position + m_pathNodes[i];
        }

        m_startTime = Time.time;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (!m_player) return;

        if (m_curIndex == m_pathNodes.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position,m_pathNodes[0], m_speed*Time.deltaTime);
            if (Vector3.Distance(transform.position, m_pathNodes[0])<m_threshold)
            {
                m_curIndex = 0;
                m_startTime = Time.time;
                if (m_LookAtPlayer)
                {
                    m_target = m_player.transform.position;
                }
                else
                {
                    m_target = m_pathNodes[m_curIndex];
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,m_pathNodes[m_curIndex], m_speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_pathNodes[m_curIndex]) < m_threshold)
            {
                if (m_LookAtPlayer)
                {
                    m_target = m_player.transform.position;
                }
                else
                {
                    m_target = m_pathNodes[m_curIndex];
                }
                m_curIndex++;
                m_startTime = Time.time;
            }
        }

        LookAtPlayer();
    }


    void LookAtPlayer()
    {
        if(m_LookAtPlayer)
        {
            if (transform.position.x - m_target.x > 0)
            {
                transform.localScale = new Vector3(m_startScale, transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector3(-m_startScale, transform.localScale.y);
            }
        }
        else
        {
            if (transform.position.x - m_target.x > 0)
            {
                transform.localScale = new Vector3(-m_startScale, transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector3(m_startScale, transform.localScale.y);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            for (int i = 0; i < m_pathNodes.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(m_pathNodes[i], 0.02f);
                if (i > 0)
                {
                    Gizmos.DrawLine(m_pathNodes[i], m_pathNodes[i - 1]);
                }
                if (i == m_pathNodes.Length - 1)
                {
                    Gizmos.DrawLine(m_pathNodes[i], m_pathNodes[0]);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_pathNodes.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position + m_pathNodes[i], 0.02f);
                if (i > 0)
                {
                    Gizmos.DrawLine(transform.position + m_pathNodes[i], transform.position + m_pathNodes[i - 1]);
                }
                if (i == m_pathNodes.Length - 1)
                {
                    Gizmos.DrawLine(transform.position + m_pathNodes[i], transform.position + m_pathNodes[0]);
                }
            }
        }
        
    }
}
