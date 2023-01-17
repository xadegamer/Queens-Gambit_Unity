using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashMat : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] Color normalColour;
    [ColorUsage(true, true)]
    [SerializeField] Color flashColour;
    [SerializeField] float flashDuration;
    SpriteRenderer m_Renderer;

    void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        m_Renderer.color = flashColour;
        yield return new WaitForSeconds(flashDuration);
        m_Renderer.color = normalColour;
    }
}
