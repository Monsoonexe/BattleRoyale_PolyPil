using UnityEngine;

public class Colors_Rarity : ScriptableObject
{
    [Header("Colors for Rarity")]
    public Color common;
    public Color uncommon;
    public Color rare;
    public Color epic;
    public Color legendary;
    public Color mythic;
    
    public Color GetColor(ItemRarityENUM rarity)
    {
        Color rarityColor;
        switch (rarity)
        {
            case ItemRarityENUM.Common:
                rarityColor = common;
                break;
            case ItemRarityENUM.Uncommon:
                rarityColor = uncommon;
                break;
            case ItemRarityENUM.Rare:
                rarityColor = rare;
                break;
            case ItemRarityENUM.Epic:
                rarityColor = epic;
                break;
            case ItemRarityENUM.Legendary:
                rarityColor = legendary;
                break;
            case ItemRarityENUM.Mythic:
                rarityColor = mythic;
                break;
            default:
                Debug.Log("What am I doing with my life?");
                rarityColor = Color.white;
                break;
        }
        return rarityColor;
    }
    

}
