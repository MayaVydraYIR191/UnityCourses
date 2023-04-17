using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cannonBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        lifeTime = lifeTime - Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
        }
    }
}

