using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float cooldown;
    
    private Player player;
    private Image ShieldImage;

    [HideInInspector] public bool isCooldown;

    void Start()
    {
        ShieldImage = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isCooldown = true;
    }
    
    void Update()
    {
        //��� ��������� �� ��������
        if (isCooldown)
        {
            ShieldImage.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (ShieldImage.fillAmount <= 0)
            {
                ShieldImage.fillAmount = 1;
                isCooldown = false;
                player.shield.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetTimer()
    {
        //���������� ������ ���� ��� ������� ������, ���� ������ �� �������� ����������� 
        ShieldImage.fillAmount = 1;
    }

    public void ReduceTime(int damage)
    {
        //���������� ���� ��� ��������� �����
        ShieldImage.fillAmount += damage / 5f;
    }
}