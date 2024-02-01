using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Texture[] _texture;

    [SerializeField]
    Canvas _canvas;

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

    public void SetSkill()
    {
        gameObject.GetComponent<RawImage>().texture = _texture[1];
        gameObject.GetComponent<RawImage>().color = Color.blue;
    }

    public void PrepareToSkill()
    {
        gameObject.GetComponent<RawImage>().color = Color.blue;
    }

    public void ReSetColor()
    {
        gameObject.GetComponent<RawImage>().texture = _texture[0];
        gameObject.GetComponent<RawImage>().color = Color.white;
    }
}
