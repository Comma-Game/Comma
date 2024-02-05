using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Texture _texture;

    [SerializeField]
    Canvas _canvas;

    [SerializeField]
    GameObject _skillEffect;

    [SerializeField]
    GameObject _forceField;

    private void Awake()
    {
        for (int i = 0; i < _skillEffect.transform.childCount; i++) _skillEffect.transform.GetChild(i).localScale *= _canvas.scaleFactor;
    }

    public void EnableSkillEffect()
    {
        _skillEffect.SetActive(true);
    }

    public void DisableSkillEffect()
    {
        _skillEffect.SetActive(false);
    }

    public void EnableRawImage()
    {
        gameObject.GetComponent<RawImage>().color = Color.white;
    }

    public void DisableRawImage()
    {
        gameObject.GetComponent<RawImage>().color = Color.white;
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

    public void ReSetColor()
    {
        _forceField.SetActive(false);

        gameObject.GetComponent<RawImage>().texture = _texture;
        gameObject.GetComponent<RawImage>().color = Color.white;
    }
}
