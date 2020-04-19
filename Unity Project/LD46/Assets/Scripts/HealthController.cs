using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject karenExplosionPrefab;
    [SerializeField] AudioSource karenScreamAudioSource;
    [SerializeField] AudioClip[] karenScreamAudioClips;
    bool alive = true;
    int screamClipIndex = 0;
    float screamClipLength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0 && alive)
        {
            alive = false;
            Death();
        }
    }

    void Death()
    {
        if (gameObject.tag == "Karen")
        {
            screamClipIndex = Random.Range(0, karenScreamAudioClips.Length);
            screamClipLength = karenScreamAudioClips[screamClipIndex].length;
            karenScreamAudioSource.PlayOneShot(karenScreamAudioClips[screamClipIndex]);

            if (screamClipLength > 1.5f)
            {
                screamClipLength = 1.5f;
            }

            Destroy(gameObject, screamClipLength);
        }
    }

    private void OnDestroy()
    {
        GameObject explosion = Instantiate(karenExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 10);
    }
}
