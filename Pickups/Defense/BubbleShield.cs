using System;
using UnityEngine;


public class BubbleShield : MonoBehaviour, IDefense
{

    Color startColor;
    Color endColor = Color.clear;
    Renderer rend;
    float shieldUseTime = 0f;
    float shieldUse = 2f;
    float force = 500f;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
    }
    void Update()
    {

        float lerpin = (shieldUseTime += Time.deltaTime) / shieldUse;
        rend.material.color = Color.Lerp(startColor, endColor, lerpin);

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
        return shieldUse;
    }
    public float GetCurrentShieldValue()
    {
        return shieldUse - shieldUseTime;
    }
    public void EndDefense()
    {
        gameObject.SetActive(false);

    }
}
