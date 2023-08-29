using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public GameObject destroyEffect;
    public GameObject bullSound;

    [SerializeField] bool bulletEnemy;

    private void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    private void Update()
    {
        //столкновение пули с врагами и игроком и получение у них урона 
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            else if (hitInfo.collider.CompareTag("Boss"))
            {
                hitInfo.collider.GetComponent<Boss1>().TakeDamage(damage);
            }
            else if (hitInfo.collider.CompareTag("DestractBlock"))
            {
                hitInfo.collider.GetComponent<DestractionBlock>().TakeDamage(-damage);
            }
            else if (hitInfo.collider.CompareTag("Player") && bulletEnemy) 
            {
                hitInfo.collider.GetComponent<Player>().ChangeHealth(-damage); 
            }
            DestroyBullet();
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public void DestroyBullet()
    {
        //уничтожение пули и появление звука с эффектом
        Instantiate(bullSound, transform.position, Quaternion.identity);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}