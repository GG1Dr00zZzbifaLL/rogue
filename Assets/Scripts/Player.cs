using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum ControlType { PC, Android }

    [Header("Controls")]
    public ControlType controlType;
    public Joystick joystick;
    public float speed;

    [Header("Health")]
    public GameObject PotionEffect;
    public GameObject effect;
    public int numOfHearts;
    public int health;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Shield")]
    public GameObject shield;
    public GameObject ShieldEffect;
    public Shield shieldTimer;

    [Header("Weapon")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    [Header("Key")]
    public GameObject keyIcon;
    public GameObject wallEffect;

    [Header("Sounds")]
    public GameObject keySound;
    public GameObject shieldSound;
    public GameObject healSound; 
    public GameObject keyDoorSound;

    [Header("Movement")]
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator anim;

    private bool facingRight = false;
    private bool keyButtonPushed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }

    void Update() 
    {
        //передвижение игрока и тип управления
        if (controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        moveVelocity = moveInput.normalized * speed;

        //переключение стандартной анимации и анимации бега
        if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        //поворот модели игрока
        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    public void Flip()
    {
        //разворот игрока 
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    //уменьшение здоровья щита и игрока
    public void ChangeHealth(int healthValue)
    {
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
        }
        else if (shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceTime(healthValue);
        }
        Instantiate(effect, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //подбор разных вещей
        if (other.CompareTag("Potion"))
        {
            //хил
            Instantiate(healSound, transform.position, Quaternion.identity);
            ChangeHealth(3);
            Instantiate(PotionEffect, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            //щит
            Instantiate(shieldSound, transform.position, Quaternion.identity);

            if (!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Instantiate(ShieldEffect, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetTimer();
                Instantiate(ShieldEffect, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Weapon"))
        {
            //оружие
            for (int i = 0; i < allWeapons.Length; i++)
            {
                if (other.name == allWeapons[i].name)
                {
                    unlockedWeapons.Add(allWeapons[i]);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Key"))
        {
            //ключ
            Instantiate(keySound, transform.position, Quaternion.identity);
            keyIcon.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    public void SwitchWeapon()
    {
        //смена оружия
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeInHierarchy)
            {
                unlockedWeapons[i].SetActive(false);
                if (i != 0)
                {
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            }
        }
    }

    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //открытие двери ключом
        if (other.CompareTag("Door") && keyButtonPushed && keyIcon.activeInHierarchy)
        {
            Instantiate(keyDoorSound, transform.position, Quaternion.identity);
            Instantiate(wallEffect, other.transform.position, Quaternion.identity);
            keyIcon.SetActive(false);
            other.gameObject.SetActive(false);
            keyButtonPushed = false;
        }
    }

    private void FixedUpdate() 
    {
        //сердечки равны количеству здоровья
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Mathf.RoundToInt(health))
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }
}