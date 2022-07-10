#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedShoot : MonoBehaviour
{
    private Transform m_target;
    [SerializeField] Transform m_spawnPoint;
    [SerializeField] float m_fireRate;
    [SerializeField] GameObject m_projectile;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_target = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("Fire", m_fireRate, m_fireRate);
    }

    void Fire()
    {
        if(Time.timeScale!=0)
        {
            GameObject proj = Instantiate(m_projectile, m_spawnPoint.position, Quaternion.identity);
            proj.transform.right = m_target.position - transform.position;
            if(m_audioSource.isActiveAndEnabled)m_audioSource.Play();
        }
    }
}
