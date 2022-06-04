using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    public float maxFuel = 4f;
    public float thrustForce = 0.5f;
    public Rigidbody rigid;
    public Transform groundedTransform;
    public ParticleSystem effect01;
    public ParticleSystem effect02;

    private float curFuel;

    void Start()
    {
        curFuel = maxFuel;
    }

    void Update()
    {
        if(Input.GetAxis("Jump") > 0f && curFuel > 0f)
        {
            curFuel -= Time.deltaTime;
            rigid.AddForce(rigid.transform.up * thrustForce, ForceMode.Impulse);
            effect01.Play();
            effect02.Play();
        }
        else if(Physics.Raycast(groundedTransform.position, Vector3.down, 0.05f, LayerMask.GetMask("Blocks")) && curFuel < maxFuel)
        {
            curFuel += Time.deltaTime*100;
            effect01.Stop();
            effect02.Stop();
        }
        else
        {
            effect01.Stop();
            effect02.Stop(); 
        }
    }
}
