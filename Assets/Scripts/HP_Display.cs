#pragma warning disable  0649

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class HP_Display : MonoBehaviour
{
    Stack<GameObject> hps = new Stack<GameObject>();

    [SerializeField] private float m_StartingPos = -500;
    [SerializeField] private float m_Ypos = 50;
    [SerializeField] private float m_offset = 50;

    [SerializeField] private GameObject hpPrefab;
    [SerializeField] private int m_maxHp = 10;
    private int m_runningCount;

    void Start()
    {
        //SetStartHP();
    }

    void SetStartHP()
    {
        hps.Clear();
        for (int i = 0; i < m_maxHp; i++)
        {
            GameObject hp = Instantiate(hpPrefab,this.transform);
            hp.GetComponent<RectTransform>().position = new Vector3(m_StartingPos + (i * m_offset), m_Ypos, 0);
            hps.Push(hp);
        }
        m_runningCount = m_maxHp;
    }
    public void AddHp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if(m_runningCount<m_maxHp)
            {
                GameObject hp = Instantiate(hpPrefab, this.transform);
                hp.GetComponent<RectTransform>().position = new Vector3(hps.Peek().GetComponent<RectTransform>().position.x + m_offset, m_Ypos, 0);
                hps.Push(hp);
                m_runningCount++;
            }
        }

    }

    public void RemoveHp(int amount)
    {
        //for (int i = 0; i < amount; i++)
        //{
        //    if(hps.Any())
        //    {
        //        Destroy(hps.Pop());
        //        m_runningCount--;
        //    }
        //}
    }

}
