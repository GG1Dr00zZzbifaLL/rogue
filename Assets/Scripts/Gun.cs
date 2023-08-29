using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType { Default, Enemy }
    public GunType gunType;
    public float offset;
    public float StartTimeBtwShots;
    public GameObject bullet;
    public GameObject bulletEffect;  
    public Transform shotPoint;
    public Joystick joystick;


    private float timeBtwShots;
    private Player player;
    private float rotZ;
    private Vector3 difference;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player.controlType == Player.ControlType.PC && gunType == GunType.Default)
        {
            joystick.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        //управление оружием
        if (gunType == GunType.Default)
        {
            if (player.controlType == Player.ControlType.PC)
            {
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            else if (player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
            {
                rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
        }
        else if (gunType == GunType.Enemy)
        {
            difference = player.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        }
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        //время между выстрелами и непосредственно вызов метода отвечающего за стрельбу 
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && player.controlType == Player.ControlType.PC || gunType == GunType.Enemy)
            {
                Shoot();
            }
            else if (Input.GetMouseButton(0) && player.controlType == Player.ControlType.Android || gunType == GunType.Enemy)
            {
                if (joystick.Horizontal != 0 || joystick.Vertical != 0) 
                {
                    Shoot();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    //метод стрельбы
    public void Shoot()
    {
        Instantiate(bulletEffect, transform.position, Quaternion.identity);
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = StartTimeBtwShots;
    }
}