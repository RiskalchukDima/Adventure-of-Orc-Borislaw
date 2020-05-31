using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private GameObject winConditionPannel;
    [SerializeField]
    private GameObject loseConditionPannel;

    [SerializeField]
    private float timeBtwAttack;
    [SerializeField]
    public float startTimeBtwAttack;
    [SerializeField]
    public Transform attackPos;
    [SerializeField]
    public LayerMask enemy;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    public int damage;
    [SerializeField]
    public Animator anim;

    [SerializeField]
    private int lives = 5;


    public int Lives
	{
		get { return lives; }
		set
		{
            if (value < 5) lives = value;
            livesBar.Refresh();
		}
	}
    private LivesBar livesBar; 

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
        livesBar = FindObjectOfType<LivesBar>();
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

        if (Input.GetButtonDown("Fire1")) Attak();
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
        Lives--;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 9.0F, ForceMode2D.Impulse); 

        UnityEngine.Debug.Log(lives);

        if (Lives == 0)
        {
            Die();
            
        }
	}

    public override void Die()
	{
        Destroy(gameObject);
        loseConditionPannel.SetActive(true);
    }

    public void Win()
	{
        UnityEngine.Debug.Log("Yarik pidar");
	}

    private void Attak()
	{
        if (timeBtwAttack <= 0)
        {
            State = CharState.Attak;

            
        }
		else
		{
            timeBtwAttack -= Time.deltaTime;
		}
    }

    private void OnAttak()
	{
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right, 2.0F);
        for (int i = 0; i < enemies.Length; i++)
        {

            if (enemies[i].GetComponent<Monster>()) enemies[i].GetComponent<Monster>().ReceiveDamage();
            if (enemies[i].GetComponent<MovableMonster>()) enemies[i].GetComponent<MovableMonster>().ReceiveDamage();
            if (enemies[i].GetComponent<ShootableMonster>())
            {

                enemies[i].GetComponent<ShootableMonster>().ReceiveDamage();
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();

        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }

}

public enum CharState
{
    Idle,
    Run,
    Jump,
    Attak
}
