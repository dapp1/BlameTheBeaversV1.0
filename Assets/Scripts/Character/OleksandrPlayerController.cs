using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    private bool isRightSide = true;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);

        if (horizontalInput != 0)
            _anim.SetFloat("Speed", Math.Abs(horizontalInput));

        if ((horizontalInput < 0 && isRightSide) || (horizontalInput > 0 && !isRightSide))
            Flip();
    }

    private void Flip()
    {
        isRightSide = !isRightSide;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
