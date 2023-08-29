using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
{
    public GameObject FloatingDamage;
    public GameObject effect;
    public GameObject deathSound;
    public GameObject trash;
    public GameObject bossGun; 
    public float health;
    public float speed;
    public float startStopTime;
    public float attackRange;
    public float heal;
    public int damage;
    public Transform attackPos;
    public LayerMask playerMask;

    private float stopTime;
    private Player player;
    private Animator anim;
    private AddRoom room; 
    private bool stopped;

    [HideInInspector] public bool playerNotInRoom;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        room = GetComponentInParent<AddRoom>();
    }

    private void Update()
    {
        //остановочка при получении урона и проверка игрока в комнате
        if (!playerNotInRoom)
        {
            if (stopTime <= 0)
            {
                stopped = false;
            }
            else
            {
                stopped = true;
                stopTime -= Time.deltaTime;
            }
        }
        else
        {
            stopped = true;
        }

        if (!stopped)
        {
            //следует за игроком 
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        //небольшое восстановление здоровья 
        if (health < 140)
        {         
            health += Time.deltaTime * heal;
        }

        //смена атак в зависимости от колличества здоровья
        if (health < 101 && health > 60)
        {
            anim.SetBool("Stage2HP", true);
            trash.SetActive(false);
            damage = 0;
            bossGun.SetActive(true);
        }
        else if (health <= 60 && health > 0)
        {
            anim.SetBool("Stage3HP", true);
            trash.SetActive(true);
            damage = 1;
            speed = 1.6f;
        }
        //победа над боссом переносит на новый уровень
        else if (health <= 0)
        {
            Instantiate(deathSound, transform.position, Quaternion.identity);
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //поворот босса
        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    //получение урона
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime; 
        Instantiate(effect, transform.position, Quaternion.identity);     
        Vector2 damagePos = new Vector2(transform.position.x, transform.position.y + 2f);
        Instantiate(FloatingDamage, damagePos, Quaternion.identity);
        FloatingDamage.GetComponentInChildren<FloatingDamage>().damage = damage;
        health -= damage;
    }

    //нанесение урона
    public void OnAttack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
        for (int i = 0; i < player.Length; i++)
        {   
            player[i].GetComponent<Player>().ChangeHealth(-damage);
        }
    }
}