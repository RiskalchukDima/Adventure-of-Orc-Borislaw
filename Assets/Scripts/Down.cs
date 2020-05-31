using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{

		Character character = collider.GetComponent<Character>();

		if (character)
		{
			character.Die();

		}
	}
}
