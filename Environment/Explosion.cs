using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float waitTime = 0.3f;
    ParticleSystem explosion;
    private void Awake()
    {
        explosion = GetComponent<ParticleSystem>();
        explosion.Play();
        GameObject.FindGameObjectWithTag("Core").GetComponent<AudioController>().PlayAudio(AudioType.Explosion_00);
        StartCoroutine(WaitToDestroy());
    }
    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
