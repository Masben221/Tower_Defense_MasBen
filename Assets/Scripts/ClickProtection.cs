using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System;
using UnityEngine.EventSystems;

public class ClickProtection : MonoSingleton<ClickProtection>, IPointerClickHandler
{
    private Image blocker;

    [SerializeField] private GameObject m_TargetingCircle;
    [SerializeField] private GameObject m_SoundFire;    

    private void Start()
    {
        blocker = GetComponent<Image>();
        blocker.enabled = false;        
    }

    private void Update()
    {
        m_TargetingCircle.transform.position = Input.mousePosition;        
    }

    private Action<Vector2> m_OnClickAction;
    public void Activate(Action<Vector2> mouseAction)
    {
        blocker.enabled = true;
        
        m_TargetingCircle.SetActive(true);
        m_SoundFire.SetActive(false);
        
        m_OnClickAction = mouseAction; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        blocker.enabled = false;
        m_TargetingCircle.SetActive(false);
        m_SoundFire.SetActive(true);
                
        m_OnClickAction(eventData.position);
        m_OnClickAction = null;
    }       
}
