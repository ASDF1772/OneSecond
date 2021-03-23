using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour 
{
    public float scrollSpeed = 0.1f;
    private Material thisMaterial;

    void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector2 newOffset = thisMaterial.mainTextureOffset;
        newOffset.Set(0, newOffset.y + (scrollSpeed * Time.deltaTime));
        thisMaterial.mainTextureOffset = newOffset;
    }
}
