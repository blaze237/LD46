using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    bool phoneMovingUp = false;
    bool phoneUp = false;
    bool phoneRotatingTowardsKaren = false;
    [SerializeField] float phoneRotationSpeed;
    [SerializeField] GameObject flameThrower;
    [SerializeField] AudioSource flameThrowerInOutAudioSource;
    [SerializeField] AudioSource flameThrowerLoopAudioSource;
    [SerializeField] AudioClip[] flameThrowerSounds;
    bool flameThrowerFiring = false;
    bool flameThrowerInPlaying = false;
    bool flameThrowerLoopPlaying = false;
    bool flameThrowerOutPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !phoneMovingUp)
        {
            phoneMovingUp = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && phoneMovingUp && !phoneRotatingTowardsKaren)
        {
            phoneMovingUp = false;
            
        }

        if (phoneMovingUp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Time.deltaTime * 10);
        }
        else if (!phoneMovingUp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, -1.5f, transform.localPosition.z), Time.deltaTime * 10);
            
        }

        if (transform.localPosition.y >= -0.1f)
        {
            phoneUp = true;            
        }

        if (Input.GetKeyDown(KeyCode.R) && phoneUp && !phoneRotatingTowardsKaren)
        {
            phoneRotatingTowardsKaren = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && phoneUp && phoneRotatingTowardsKaren)
        {
            phoneRotatingTowardsKaren = false;
        }

        if (phoneRotatingTowardsKaren)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 360, 0), Time.deltaTime * phoneRotationSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * phoneRotationSpeed);
        }

        Flamethrower();
    }

    void Flamethrower()
    {
        // If the phone is facing Karen
        if (transform.localRotation.eulerAngles.y <= 1 && phoneRotatingTowardsKaren && !flameThrowerFiring)
        {
            flameThrower.SetActive(true);

            flameThrowerOutPlaying = false;

            // If the in sound has not been played
            if (!flameThrowerInPlaying)
            {
                flameThrowerInOutAudioSource.PlayOneShot(flameThrowerSounds[0]);
                flameThrowerInPlaying = true;
            }
            // If the in sound has been played and the loop is not playing
            if (flameThrowerInPlaying && !flameThrowerLoopPlaying)
            {
                flameThrowerLoopAudioSource.Play();                
                flameThrowerLoopPlaying = true;                
            }
        }
        else if (!phoneMovingUp || !phoneRotatingTowardsKaren)
        {        

            if (!flameThrowerOutPlaying)
            {
                flameThrowerLoopAudioSource.Stop();
                flameThrowerInOutAudioSource.PlayOneShot(flameThrowerSounds[2]);
                flameThrowerInPlaying = false;
                flameThrowerOutPlaying = true;
                flameThrowerLoopPlaying = false;
            }


            flameThrower.SetActive(false);
        }
    }
}
