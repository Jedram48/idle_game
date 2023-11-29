using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Statistics : MonoBehaviour {
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float damage = 1;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        anim.speed = animationSpeed;
    }

    public float attackDamage() {
        return animationSpeed * damage;
    }
}
