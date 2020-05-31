using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableMonster : Monster
{
    [SerializeField]
    private float rate = 2.0F;

    [SerializeField]
    private Color bulletColor = Color.white;

    private Bullet bullet;

    protected override void Awake()
	{
        bullet = Resources.Load<Bullet>("Bullet");
	}

    protected override void Start()
	{
        InvokeRepeating("Shoot", rate, rate);
	}

    private void Shoot()
	{
        Vector3 position = transform.position;
        position.y += 0.1F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;  
	}

    protected override void  OnTriggerEnter2D(Collider2D collider)
	{
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.7F) ReceiveDamage();
            else unit.ReceiveDamage();
        }
    }
}
