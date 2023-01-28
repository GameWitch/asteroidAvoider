using System;
using UnityEngine;

public class BalloonShield : MonoBehaviour, IDefense
{

    float growBubbleValue = 9f;
    float force = 1000;
    float poppingPoint = 8f;

    Renderer rend;

    Color startColor;
    Color endColor = Color.clear;
    Vector3 startingScale;

    float shieldUseTime = 0f;
    float shieldUse = 1f;
    bool growState = true;
    bool popState = false;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;


        startingScale = transform.localScale;
    }

    private void Update()
    {
        if (growState)
        {
            if (!rend.enabled)
            {
                rend.enabled = true;
            }

            float lerpin = (shieldUseTime += Time.deltaTime) / shieldUse;
            rend.material.color = Color.Lerp(startColor, endColor, lerpin);

            Vector3 scale = transform.localScale * growBubbleValue * Time.deltaTime;
            transform.localScale += scale;
            
            if (transform.localScale.x > poppingPoint)
            {
                rend.material.color = startColor;
                rend.enabled = false;
                shieldUseTime = 0f;
                growState = false;
                popState = true;
            }
            
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Untagged":

                Vector3 dir = (collision.transform.position - transform.position).normalized;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);

                break;
        }
    }

    public float GetShieldDestroyValue()
    {
        return poppingPoint;
    }

    public float GetCurrentShieldValue()
    {
        return poppingPoint - transform.localScale.x;
    }
    public void EndDefense()
    {
        
        rend.material.color = startColor;
        rend.enabled = false;        
        shieldUseTime = 0f;
        gameObject.SetActive(false);

        if (popState) return;

        transform.localScale = startingScale;

    }
}
