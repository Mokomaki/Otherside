using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SavePoint : MonoBehaviour
{
    [SerializeField] GameObject m_gameObjectToLoad;
    public void SpawnObjects()
    {
        m_gameObjectToLoad.SetActive(true);
    }
}