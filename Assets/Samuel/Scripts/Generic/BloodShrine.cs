using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Blood shrine logic
/// TODO modify to scale costs&chances
/// </summary>
public class BloodShrine : Interactable
{
   private PerkObject[] negatives, rares, epics, legendaries;

    [Header("Panels")]

    public GameObject shrineUI;
    public GameObject gameUI;

    [Space] [Header("UI")]

    public TMP_Text tokensDisplay;
    public TMP_Text negativeChanceText;
    public TMP_Text legendaryChanceText;
    public TMP_Text currentCostText;
    public Slider costSlider;

    private int currentWishCost;
    private int currentNegativeChance;
    private int currentLegendaryChance;

    protected override void Start()
    {
        base.Start();
        PerkLoader.LoadPerks(ref negatives, ref rares, ref epics, ref legendaries);
    }

    public override void Interact()
    {
        base.Interact();
        if (!interactable) return;

        if (!GameManager.instance.canPause) return;
        Time.timeScale = 0f;

        UpdateShrine();

        shrineUI.SetActive(true);
        gameUI.SetActive(false);
        GameManager.instance.canPause = false;
    }

    public void PurchaseWish()
    {
        var negativePerk = Random.Range(1, 100) < currentNegativeChance;

        InventoryManager.instance.SpendTokens(currentWishCost);
        PerkObject perkToGive;

        if (!negativePerk)
        {
            if(Utils.PercentageChance(currentLegendaryChance))
            {
                perkToGive = legendaries[Random.Range(0, legendaries.Length)];
            }
            else if(Utils.PercentageChance(currentLegendaryChance * 1.5f))
            {
                perkToGive = epics[Random.Range(0, epics.Length)];
            }
            else
            {
                perkToGive = rares[Random.Range(0, rares.Length)];
            }
        }
        else
        {
            perkToGive = negatives[Random.Range(0, negatives.Length)];
        }

        InventoryManager.instance.AddPerk(perkToGive);
        UpdateShrine();
    }

    public void UpdateShrine()
    {
        costSlider.maxValue = Mathf.Clamp(InventoryManager.instance.bloodTokens, 0, 200);
        currentWishCost = (int)costSlider.value;
        currentCostText.text = "Current Cost: " + (int)costSlider.value;
        tokensDisplay.text = InventoryManager.instance.bloodTokens + " Blood Tokens";

        currentNegativeChance = currentWishCost >= 100 ? 0 : 100 - currentWishCost;
        currentLegendaryChance = currentWishCost <= 100 ? 0 : (currentWishCost - 100) / 4;

        negativeChanceText.color = Color.Lerp(Color.green, Color.red, (float)currentNegativeChance / 100);
        negativeChanceText.text = "Chance of negative perk: " + currentNegativeChance + "%";
        legendaryChanceText.text = "Chance of legendary perk: " + currentLegendaryChance + "%"; 
    }

    public void CloseShrine()
    {
        Time.timeScale = 1f;

        gameUI.SetActive(true);
        shrineUI.SetActive(false);
        GameManager.instance.canPause = true;
    }
}
