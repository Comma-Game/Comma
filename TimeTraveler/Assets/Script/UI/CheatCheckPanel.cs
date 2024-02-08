using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCheckPanel : MonoBehaviour
{
    private bool m_IsButton1Downing;
    private bool m_IsButton2Downing;

    [SerializeField] public GameObject cheatPanel;

    void Update()
    {
        if(m_IsButton1Downing && m_IsButton2Downing)
        {
            cheatPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void PointerDownButton1()
    {
        m_IsButton1Downing = true;
    }

    public void PointerUpButton1()
    {
        m_IsButton1Downing = false;
    }

    public void PointerDownButton2()
    {
        m_IsButton2Downing = true;
    }

    public void PointerUpButton2()
    {
        m_IsButton2Downing = false;
    }
}
