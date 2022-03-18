using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    Rigidbody2D enemyRigidbody;
    public float speed;
    public float maxHealth;
    [SerializeField]
    float health;

    Transform target;
    [SerializeField]
    int currentWayPoint;
    GameControler cont;
    public float rotationSpeed;

    float distance;

    bool canMove = true;
    public float damage;

    public float dropMoney;

    public GameObject explosion;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        cont = FindObjectOfType<GameControler>();
        canMove = true;

    }

    private void OnEnable()
    {
        health = maxHealth;
        currentWayPoint = 0;
        target = cont.waypoints[currentWayPoint];
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            cont.GiveMoney(dropMoney);
            Instantiate(explosion, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);

        if (canMove)
            enemyRigidbody.AddForce(transform.up * speed * Time.deltaTime);

        distance = Vector2.Distance(transform.position, target.position);
        if (distance <= 0.5f)
        {

            if (currentWayPoint < cont.waypoints.Length - 1)
            {
                canMove = false;
                Invoke("CanMove", 1f);

                currentWayPoint++;
                target = cont.waypoints[currentWayPoint];
                Debug.Log(currentWayPoint);
            }
            else
            {
                cont.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }

    void CanMove()
    {
        canMove = true;
    }
}
