using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterFXs { Cure }

public class MonsterFX : MonoBehaviour
{
    [SerializeField] Transform[] monsterTransforms;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMonsterFX(MonsterFXs fxToPlay, Transform monsterTransform)
    {

        switch (fxToPlay)
        {
            case MonsterFXs.Cure:
                transform.position = monsterTransform.position;
                animator.SetTrigger("cure");
                break;
        }
    }

}
