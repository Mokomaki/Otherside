using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPeffects : MonoBehaviour
{
    Volume m_volume;

    DepthOfField m_depthOfField;

    public bool BlurEffect { set { m_depthOfField.active = value; } }

    void Start()
    {
        Volume m_volume = gameObject.GetComponent<Volume>();
        DepthOfField tmp;

        if (m_volume.profile.TryGet<DepthOfField>(out tmp))
        {
            m_depthOfField = tmp;
        }
        else
        {
            Debug.LogError("Could not get 'depth of field' from volume");
        }
    }
}
