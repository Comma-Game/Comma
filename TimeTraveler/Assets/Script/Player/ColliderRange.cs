using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderRange : MonoBehaviour
{
    [SerializeField]
    Texture[] _textures;

    public void EnableRawImage()
    {
        gameObject.GetComponent<RawImage>().enabled = true;
    }

    public void DisableRawImage()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[0];
        gameObject.GetComponent<RawImage>().enabled = false;
    }

    public void SetInvincible()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[1];
    }

    public void SetSkill()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[2];
    }

    public void ReSetColor()
    {
        gameObject.GetComponent<RawImage>().texture = _textures[0];
    }
}
