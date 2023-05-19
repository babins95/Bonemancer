using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingAlpha : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    public Color spriteColor;
    public Color startingColor;
    public float alphaColor;
    public bool alphaChange = false;
    public float alphaMaxSpeed = 1f;
    public float alphaMinSpedd = 0.5f;
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        spriteColor = sRenderer.color;
        startingColor = sRenderer.color;
        alphaColor = spriteColor.a;
    }
    void Update()
    {
        if(alphaChange == true)
        {
            if(GetComponent<SpriteRenderer>().color.a < 1f)
            {
                alphaColor += alphaMaxSpeed * Time.deltaTime;
            }
            else if (GetComponent<SpriteRenderer>().color.a > 1f)
            {
                alphaColor = 1f;
            }
        }
        if(alphaChange == false)
        {
            if (GetComponent<SpriteRenderer>().color.a > 0f)
            {
                alphaColor -= alphaMinSpedd * Time.deltaTime;
            }
            else if (GetComponent<SpriteRenderer>().color.a < 0f)
            {
                alphaColor = 0;
            }
        }
        GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alphaColor);
    }
}
