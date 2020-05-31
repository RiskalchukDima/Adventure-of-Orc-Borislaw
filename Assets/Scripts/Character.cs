using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float jumpForce = 15.0f;

    private bool isGrounded = false;

    private Bullet bullet;

    private CharState State
	{
		get
		{
            return (CharState)animator.GetInteger("State"); 
		}
        set
		{
            animator.SetInteger("State", (int)value); 
		}
	}

    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");
    }



    private void FixedUpdate()
	{
        CheckGround();
	}

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }

    private void Shoot()
	{
        Vector3 position = transform.position;
        position.y += 0.7F;

        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F:1.0F );
	}

    public override void ReceiveDamage()
	{
        lives--;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 9.0F, ForceMode2D.Impulse); 

        UnityEngine.Debug.Log(lives);
	}

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (isGrounded)  State = CharState.Run;
    }

    private void Jump()
    {
        State = CharState.Jump; 
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
	{
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.7F);

        isGrounded = colliders.Length > 1;
        if (!isGrounded) State = CharState.Jump;
    }

     
}

public enum CharState
{
    Idle,
    Run,
    Jump
}
