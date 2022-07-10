#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgBlock : MonoBehaviour
{
    [SerializeField] private int m_Dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health collisionHealth = collision.gameObject.GetComponent<Health>();
        if (collisionHealth)
        {
            collisionHealth.TakeDmg(m_Dmg);

        }
    }
}