using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootSteps : MonoBehaviour
{
    public float stepRate = 0.5f;
    public float sprintStepRate = 0.2f;
    public float stepCoolDown;
    public AudioClip footStep;
    AudioSource audio;
    CharacterController cc;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                audio.pitch = 1f + Random.Range(-0.2f, 0.2f);
                audio.PlayOneShot(footStep, 0.9f);
                stepCoolDown = sprintStepRate;
            }
            else
            {
                audio.pitch = 1f + Random.Range(-0.2f, 0.2f);
                audio.PlayOneShot(footStep, 0.9f);
                stepCoolDown = stepRate;
            }
        }
    }
}
