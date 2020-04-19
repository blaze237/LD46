using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarenAnimationController : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool chasing = false;
    [SerializeField] bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chasing)
        {
            anim.Play("Zombie_Walk");
        }
        if (attacking)
        {
            anim.Play("Melee_OneHanded");
        }
    }
}
