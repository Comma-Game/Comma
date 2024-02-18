using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Canvas _canvas;

    [SerializeField]
    GameObject _forceField;

    public void DisableRawImage()
    {
        gameObject.GetComponent<RawImage>().enabled = false;
    }

    public void SetInvincible()
    {
        gameObject.GetComponent<RawImage>().color = Color.red;
    }

    public void SetPoison()
    {
        gameObject.GetComponent<RawImage>().color = Color.green;
    }

    public void SetSkill()
    {
        _forceField.SetActive(true);
    }

    public void PrepareToSkill()
    {
        gameObject.GetComponent<RawImage>().color = Color.blue;
    }

    public void ResetColor()
    {
        ResetSkill();
        gameObject.GetComponent<RawImage>().color = Color.white;
    }

    public void ResetSkill()
    {
        _forceField.SetActive(false);
    }
}
