using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    Vector3 dir;
    RaycastHit hit;
    List<Material> mats;
    Coroutine _coroutine;

    private void Start()
    {
        mats = new List<Material>();
    }

    private void LateUpdate()
    {
        dir = (_player.transform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, _player.transform.lossyScale / 2, dir, transform.rotation, 8.6f,
            1 << LayerMask.NameToLayer("Object"));

        for (int i = 0; i < hits.Length; i++)
        {
            MeshRenderer[] obj = hits[i].transform.GetComponentsInChildren<MeshRenderer>();

            for (int j = 0; j < obj.Length; j++)
            {
                Material mat = obj[j].material;
                Color color = mat.color;

                SetMaterialRenderingMode(mat, 3f, 3000);
                StartCoroutine(ChangeAlpha(mat, color));

                mats.Add(mat);
            }

            SkinnedMeshRenderer[] s_obj = hits[i].transform.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int j = 0; j < s_obj.Length; j++)
            {
                Material mat = s_obj[j].material;
                Color color = mat.color;

                SetMaterialRenderingMode(mat, 3f, 3000);
                StartCoroutine(ChangeAlpha(mat, color));

                mats.Add(mat);
            }
        }
    }

    // 0 = Opaque, 1 = Cutout, 2 = Fade, 3 = Transparent
    private void SetMaterialRenderingMode(Material material, float mode, int renderQueue)
    {
        material.SetFloat("_Mode", mode);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = renderQueue;
    }

    public void SetOpaque()
    {
        StopAllCoroutines();

        foreach (Material mat in mats)
        {
            if (mat == null) continue;

            Color color = mat.color;

            SetMaterialRenderingMode(mat, 0f, -1);
            color.a = 1f;
            mat.color = color;
        }

        mats.Clear();
    }

    IEnumerator ChangeAlpha(Material mat, Color color)
    {
        color.a -= 0.1f;
        mat.color = color;
        yield return new WaitForSeconds(0.1f);
    }
}