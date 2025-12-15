using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxLife = 10;
    public int currentLife;

    [Header("UI")]
    public Image healthFill; // assign in Inspector (the fill image of your health bar)

    void Start()
    {
        currentLife = maxLife;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentLife -= damage;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife); // clamp so it never goes negative
        UpdateHealthUI();

        if (currentLife <= 0)
        {
            GameOverManager.Instance?.GameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = (float)currentLife / maxLife;
        }
    }
}
