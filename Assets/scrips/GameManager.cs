using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // Array de prefabs de monstruos
    public Transform spawnPoint;
    public Text coinsText; // Texto para mostrar las monedas en la UI
    public Text healthText;
    public Image healthBar;// Texto para mostrar la vida del monstruo
    public Button buyGoldGenButton, buyDamageButton; // Botones de mejoras
    public Text goldGenPriceText, damagePriceText;
    public GameObject shop;// Textos de precios de mejoras

    private int currentCoins = 0; // Monedas actuales
    private GameObject currentMonster; // Referencia al monstruo actual
    private MonsterHealth monsterHealthScript; // Referencia al script de vida del monstruo
    private float monsterHealth = 100f; // Salud inicial del monstruo
    private float monsterHealthIncreaseRate = 0.05f; // Incremento de vida en cada nuevo monstruo (5%)
    private float monsterCoinIncreaseRate = 0.1f; // Incremento de monedas al morir

    private float goldPerSecond = 1f; // Oro generado por segundo
    private float damagePerTap = 10f; // Daño por tap

    private float goldGenPrice = 1000f; // Precio inicial de generación de oro
    private float damagePrice = 500f; // Precio inicial de daño
    private float priceIncreaseRate = 1.5f; // Aumento del precio en 50%
    private float goldGenIncrease = 1.10f; // Aumento del 2% en generación de oro
    private float damageIncrease = 1.02f; // Aumento del 2% en daño por tap

    bool canTap;

    void Start()
    {
        UpdateUI();
        SpawnMonster();
        buyGoldGenButton.onClick.AddListener(BuyGoldGenerationUpgrade);
        buyDamageButton.onClick.AddListener(BuyDamageUpgrade);
        StartCoroutine(GenerateGold());
        canTap = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canTap)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Monster"))
                    {
                        MonsterHealth monster = hit.collider.GetComponent<MonsterHealth>();
                        if (monster != null && monster.CurrentHealth > 0)
                        {
                            monster.TakeDamage(damagePerTap);
                            UpdateHealthText(monster);

                            Animator animator = hit.collider.GetComponent<Animator>();
                            if (animator != null)
                            {
                                animator.SetBool("Click", true);
                                StartCoroutine(ResetClickBool(animator));
                            }

                            if (monster.CurrentHealth <= 0)
                            {
                                monster.GetComponent<Collider>().enabled = false; // Desactivar el collider
                                OnMonsterDeath();
                            }
                        }
                    }
                }
            }
           
        }
    }

    private IEnumerator ResetClickBool(Animator animator)
    {
        yield return new WaitForSeconds(0.4f);
        if (animator != null)
        {
            animator.SetBool("Click", false);
        }
    }

    void OnMonsterDeath()
    {
        int coinsEarned = Mathf.CeilToInt(100 * (1 + monsterCoinIncreaseRate));
        currentCoins += coinsEarned;
        Animator animator = currentMonster.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Dead", true);
            StartCoroutine(DestroyAfterAnimation(animator));
        }
        monsterHealth *= (1 + monsterHealthIncreaseRate);
        monsterCoinIncreaseRate += 0.1f;
        UpdateUI();
    }

    private IEnumerator DestroyAfterAnimation(Animator animator)
    {
        yield return new WaitForSeconds(25f / 60f); // Espera 25 frames (asumiendo 60 FPS)
        Destroy(currentMonster);
        yield return new WaitForSeconds(0.3f); // Espera 0.3 segundos antes de spawnear el siguiente monstruo
        SpawnMonster();
    }

    void SpawnMonster()
    {
        int randomIndex = Random.Range(0, monsterPrefabs.Length);
        currentMonster = Instantiate(monsterPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        monsterHealthScript = currentMonster.GetComponent<MonsterHealth>();
        monsterHealthScript.SetHealth(monsterHealth);
        UpdateHealthText(monsterHealthScript);
    }

    void UpdateHealthText(MonsterHealth monster)
    {
        healthText.text =  monster.CurrentHealth.ToString("F0");
        healthBar.fillAmount = monster.CurrentHealth /monsterHealth;
    }

    private IEnumerator GenerateGold()
    {
        while (true)
        {
            currentCoins += Mathf.FloorToInt(goldPerSecond);
            UpdateUI();
            yield return new WaitForSeconds(1f);
        }
    }

    void BuyGoldGenerationUpgrade()
    {
        if (currentCoins >= goldGenPrice)
        {
            currentCoins -= Mathf.FloorToInt(goldGenPrice);
            goldPerSecond *= goldGenIncrease;
            goldGenPrice *= priceIncreaseRate;
            UpdateUI();
        }
    }

    void BuyDamageUpgrade()
    {
        if (currentCoins >= damagePrice)
        {
            currentCoins -= Mathf.FloorToInt(damagePrice);
            damagePerTap *= damageIncrease;
            damagePrice *= priceIncreaseRate;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        coinsText.text =  currentCoins.ToString();
        goldGenPriceText.text = "Upgrade Gold: " + Mathf.FloorToInt(goldGenPrice);
        damagePriceText.text = "Upgrade Damage: " + Mathf.FloorToInt(damagePrice);
    }

    public void OpenShop(bool state)
    {
        if(state)
        {
            shop.SetActive(state);
            canTap = false;
        }
        else
        {
            canTap = true;
        }
       
    }
}
