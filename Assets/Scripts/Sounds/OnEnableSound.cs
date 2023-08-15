using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

public class OnEnableSound : MonoBehaviour
{
    [SerializeField] private Sound m_Sound;

    private void OnEnable()
    {  
        m_Sound.Play();
    }
}
