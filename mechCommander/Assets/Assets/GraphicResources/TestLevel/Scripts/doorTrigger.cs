using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    public Animator doorAnim;
    [Space(10)]
    [SerializeField] private AudioSource doorOpenSFX;
    [SerializeField] private float openDelay = 0.0f;
    [Space(10)]
    [SerializeField] private AudioSource doorCloseSFX;
    [SerializeField] private float closeDelay = 0.0f;

    void Start()
    {
        doorAnim = GetComponentInParent<Animator>(); 
        doorAnim.SetBool("isOpened", false);
    }
    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.name == "mech")
        {
            Debug.Log("Enter!");
            doorAnim.SetBool("isOpened", true);
            doorOpenSFX.PlayDelayed(openDelay);
        }
    }
    void OnTriggerExit(Collider co)
    {
        if (co.gameObject.name == "mech")
        {
            Debug.Log("Exit!");
            doorAnim.SetBool("isOpened", false);
            doorCloseSFX.PlayDelayed(closeDelay);
        }
    }
}
