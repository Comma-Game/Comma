using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _explainUI;

    public void EnableExplainUI() { _explainUI.transform.gameObject.SetActive(true); }
    public void DisableExplainUI() { _explainUI.transform.gameObject.SetActive(false); }
    public void SetDragText() { _explainUI.text = "�巡�� �ϼ���!"; }
    public void ResetExplainText() { _explainUI.text = "��ġ �ϼ���!"; }
}