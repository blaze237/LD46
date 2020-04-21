using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhoneController : MonoBehaviour
{
     AudioSource audioSource;
    public AudioClip lowBat, noBat;
    public float baseH = -0.5f;
    public float heightToSpin = -0.3f;
    bool phoneMovingUp = false;
    bool phoneUp = false;
    bool phoneRotatingTowardsKaren = false;
    public LightSource lightSource;
    [SerializeField] float phoneRotationSpeed;
    [SerializeField] GameObject flameThrower;
    [SerializeField] AudioSource flameThrowerInOutAudioSource;
    [SerializeField] AudioSource flameThrowerLoopAudioSource;
    [SerializeField] AudioClip[] flameThrowerSounds;
    bool flameThrowerFiring = false;
    bool flameThrowerInPlaying = false;
    bool flameThrowerLoopPlaying = false;
    bool flameThrowerOutPlaying = false;

    [SerializeField] TextMeshProUGUI batteryPercentageText;
    public float batteryPercentage = 100;
    [SerializeField] float batteryDrainTorch;
    [SerializeField] float batteryDrainFlameThrower;

    [SerializeField] AudioSource phoneAudioSource;
    [SerializeField] AudioClip[] phoneSounds;

    // Start is called before the first frame update
    void Start()
    {
        lightSource.SetEnabled(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(batteryPercentage == 0)
        {

            phoneMovingUp = false;
            phoneRotatingTowardsKaren = false;
            // phoneAudioSource.PlayOneShot(phoneSounds[1]);
            lightSource.SetEnabled(false);
        }

        if (Input.GetKeyDown(KeyCode.F) && !phoneMovingUp && (batteryPercentage != 0))
        {
            phoneMovingUp = true;
         //   phoneAudioSource.PlayOneShot(phoneSounds[0]);
            GetComponent<Renderer>().enabled = true;
            lightSource.SetEnabled(true);
        }
        else if ((( Input.GetKeyDown(KeyCode.F) && phoneMovingUp)) && !phoneRotatingTowardsKaren)
        {
            phoneMovingUp = false;
           // phoneAudioSource.PlayOneShot(phoneSounds[1]);
            lightSource.SetEnabled(false);
        }

        if (phoneMovingUp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, baseH, transform.localPosition.z), Time.deltaTime * 10);
            //Phone is up so torch is on
            if(phoneMovingUp && !flameThrowerFiring)
            {
                batteryPercentage -= Time.deltaTime * batteryDrainTorch;
                batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);
                int batteryPercentageInt = (int)batteryPercentage;
                batteryPercentageText.SetText(batteryPercentageInt.ToString() + "%");

                if(batteryPercentage == 0)
                {
                    audioSource.PlayOneShot(noBat);
                }
               
            }
        }
        else if (!phoneMovingUp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, -1.5f, transform.localPosition.z), Time.deltaTime * 10);
            if(transform.localPosition.y <= - 1.4f)
            {
                GetComponent<Renderer>().enabled = false;
            }
        }

        if (transform.localPosition.y >= heightToSpin)
        {
            phoneUp = true;

            if (Input.GetKeyDown(KeyCode.R) && phoneUp && !phoneRotatingTowardsKaren)
            {
                phoneRotatingTowardsKaren = true;
            }
            else if (Input.GetKeyDown(KeyCode.R) && phoneUp && phoneRotatingTowardsKaren)
            {
                phoneRotatingTowardsKaren = false;
            }
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
        if (transform.localRotation.eulerAngles.y <= 1 && phoneRotatingTowardsKaren && !flameThrowerFiring && phoneUp)
        {
            lightSource.m_lightSourceType = LightEffectType.Burn;
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

            batteryPercentage -= Time.deltaTime * batteryDrainFlameThrower;
            batteryPercentage = Mathf.Clamp(batteryPercentage, 0, 100);
            int batteryPercentageInt = (int)batteryPercentage;
            batteryPercentageText.SetText(batteryPercentageInt.ToString() + "%");
        }
        else if (!phoneMovingUp || !phoneRotatingTowardsKaren)
        {
            lightSource.m_lightSourceType = LightEffectType.SlowDown;
            flameThrowerLoopAudioSource.Stop();
            if (!flameThrowerOutPlaying)
            {
                flameThrowerLoopAudioSource.Stop();
            //    flameThrowerInOutAudioSource.PlayOneShot(flameThrowerSounds[2]);
                flameThrowerInPlaying = false;
                flameThrowerOutPlaying = true;
                flameThrowerLoopPlaying = false;
            }


            flameThrower.SetActive(false);
        }
    }

    public void AddBattery(float batteryAmount)
    {
        batteryPercentage += batteryAmount;
        Mathf.Clamp(batteryPercentage, 0, 100);
        int batteryPercentageInt = (int)batteryPercentage;
        batteryPercentageText.SetText(batteryPercentageInt.ToString() + "%");
    }
}
