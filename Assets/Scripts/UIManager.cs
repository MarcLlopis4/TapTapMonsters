using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoins()
    {
        coins.text = GameManager.instance.GameData.coins.ToString();
    }

    public void AddCoins(int coins)
    {
        GameManager.instance.GameData.coins += coins;
        UpdateCoins();
    }
}
