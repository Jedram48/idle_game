using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Statistics : MonoBehaviour {
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float damage = 1;
    [SerializeField] private float clickDamage = 5;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        anim.speed = animationSpeed;
    }
    public void addClickDamage() {
        clickDamage += 2;
    }

    public void addDamage() {
        damage += 1;
    }

    public void addSpeed() {
        animationSpeed += (float).1;
    }
    public float clickDamages() {
        return clickDamage;
    }

    public float attackDamage() {
        return animationSpeed * damage;
    }
}
