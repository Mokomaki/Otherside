#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private List<TimedShoot> m_shooters = new List<TimedShoot>();
    [SerializeField] float TriggerDist;

    void Start()
    {
        GameObject[] shooterObjs = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < shooterObjs.Length; i++)
        {
            TimedShoot ts = shooterObjs[i].GetComponent<TimedShoot>();
            if (ts) m_shooters.Add(ts);
        }
    }

    void FixedUpdate()
    {
        for(int i = 0; i< m_shooters.Count; i++)
        {
            if(m_shooters[i])
            {
                if(Vector3.Distance(transform.position, m_shooters[i].transform.position)<TriggerDist)
                {
                    m_shooters[i].enabled=true;
                    m_shooters.RemoveAt(i);
                }
            }
            else
            {
                m_shooters.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TriggerDist);
    }
}
