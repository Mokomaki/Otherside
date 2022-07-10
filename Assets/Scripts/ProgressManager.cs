using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SavePoint"))
        {
            SaveData data = new SaveData
            {
                PlayerHealth = other.gameObject.GetComponent<Health>().m_HP,
                PlayerPosition = other.transform.position
            };

            MenuManager.menuManager.SaveProgress(data);
        }
    }
}
