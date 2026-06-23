using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedCharacterLoader : MonoBehaviour
{
    private const string PlayerNameKey = "PlayerName";
    private const string HairIndexKey = "HairIndex";
    private const string HairColorIndexKey = "HairColorIndex";
    private const string ClothesIndexKey = "ClothesIndex";

    [Header("Preview Images")]
    public Image hairImage;
    public Image clothesImage;

    [Header("Name Text")]
    public TMP_Text playerNameText;

    [Header("Customization Data")]
    public Sprite[] hairSprites;
    public Sprite[] clothesSprites;
    public Color[] hairColors;

    private void Start()
    {
        LoadCharacter();
    }

    public void LoadCharacter()
    {
        string playerName = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
        int hairIndex = ClampIndex(GetSavedInt(HairIndexKey, "HAIR_INDEX", 0), GetLength(hairSprites));
        int hairColorIndex = ClampIndex(GetSavedInt(HairColorIndexKey, "COLOR_INDEX", 0), GetLength(hairColors));
        int clothesIndex = ClampIndex(GetSavedInt(ClothesIndexKey, "CLOTH_INDEX", 0), GetLength(clothesSprites));

        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }

        if (hairImage != null)
        {
            if (HasItems(hairSprites))
            {
                hairImage.sprite = hairSprites[hairIndex];
                hairImage.enabled = true;
            }

            if (HasItems(hairColors))
            {
                hairImage.color = hairColors[hairColorIndex];
            }
        }

        if (clothesImage != null && HasItems(clothesSprites))
        {
            clothesImage.sprite = clothesSprites[clothesIndex];
            clothesImage.enabled = true;
        }

        Debug.Log($"[SavedCharacterLoader] Loaded PlayerName='{playerName}', HairIndex={hairIndex}, HairColorIndex={hairColorIndex}, ClothesIndex={clothesIndex}");
    }

    private int ClampIndex(int index, int length)
    {
        if (length <= 0)
        {
            return 0;
        }

        return Mathf.Clamp(index, 0, length - 1);
    }

    private int GetSavedInt(string key, string fallbackKey, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        return PlayerPrefs.GetInt(fallbackKey, defaultValue);
    }

    private int GetLength<T>(T[] array)
    {
        return array != null ? array.Length : 0;
    }

    private bool HasItems<T>(T[] array)
    {
        return array != null && array.Length > 0;
    }
}
