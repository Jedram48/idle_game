using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_Follower : MonoBehaviour {
    [SerializeField] private GameObject waypoint;
    [SerializeField] private float speed = 2f;


    private Animator anim;
    private enum State { Idle, Sword, Attack, Run, Death };
    private State currentState = State.Run;


    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(waypoint.transform.position, transform.position) > .1f)
        {
            currentState = State.Run;
            transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, Time.deltaTime * speed);
            anim.SetInteger("state", (int)currentState);
        }
        else
        {
            currentState = State.Attack;
            anim.SetInteger("state", (int)currentState);
            enabled = false;
        }
    }
}
