#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_Lifetime;
    [SerializeField] private int m_Dmg;
    [SerializeField] private GameObject m_DestroyEffect;

    private void Start()
    {
        Invoke("Destroy", m_Lifetime);    
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            transform.Translate(new Vector3(m_Speed * Time.deltaTime, 0, 0), Space.Self);
        }
    }

    private void Destroy()
    {
        if(gameObject)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

            Health collisionHealth = collision.gameObject.GetComponent<Health>();
            if (collisionHealth)
            {
                collisionHealth.TakeDmg(m_Dmg);
            }
            if (m_DestroyEffect)
            {
                Destroy(Instantiate(m_DestroyEffect, transform.position, transform.rotation),3);
            }
            Destroy();
        
    }
}
