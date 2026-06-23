using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    private const string PlayerNameKey = "PlayerName";
    private const string HairIndexKey = "HairIndex";
    private const string HairColorIndexKey = "HairColorIndex";
    private const string ClothesIndexKey = "ClothesIndex";

    [Header("Scene")]
    public string nextSceneName = "StatUp_UI";

    [Header("Input")]
    public TMP_InputField nameInput;

    [Header("Existing Project Bridge")]
    public CustomizeController legacyCustomizeController;

    [Header("Preview Images")]
    public Image hairImage;
    public Image clothesImage;

    [Header("Customization Data")]
    public Sprite[] hairSprites;
    public Sprite[] clothesSprites;
    public Color[] hairColors;

    [Header("Current Index")]
    public int hairIndex;
    public int hairColorIndex;
    public int clothesIndex;

    private void Start()
    {
        if (legacyCustomizeController == null)
        {
            legacyCustomizeController = GetComponent<CustomizeController>();
        }

        LoadSavedCustomization();
        SyncToLegacyController();
        ApplyPreview();
    }

    public void NextHair()
    {
        hairIndex = GetNextIndex(hairIndex, GetLength(hairSprites));
        ApplyPreview();
    }

    public void PrevHair()
    {
        hairIndex = GetPrevIndex(hairIndex, GetLength(hairSprites));
        ApplyPreview();
    }

    public void NextHairColor()
    {
        hairColorIndex = GetNextIndex(hairColorIndex, GetLength(hairColors));
        ApplyPreview();
    }

    public void PrevHairColor()
    {
        hairColorIndex = GetPrevIndex(hairColorIndex, GetLength(hairColors));
        ApplyPreview();
    }

    public void NextClothes()
    {
        clothesIndex = GetNextIndex(clothesIndex, GetLength(clothesSprites));
        ApplyPreview();
    }

    public void PrevClothes()
    {
        clothesIndex = GetPrevIndex(clothesIndex, GetLength(clothesSprites));
        ApplyPreview();
    }

    public void StartGame()
    {
        SyncFromLegacyController();

        string playerName = nameInput != null ? nameInput.text.Trim() : string.Empty;

        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.SetInt(HairIndexKey, hairIndex);
        PlayerPrefs.SetInt(HairColorIndexKey, hairColorIndex);
        PlayerPrefs.SetInt(ClothesIndexKey, clothesIndex);

        // Backward-compatible keys for older project scripts.
        PlayerPrefs.SetInt("HAIR_INDEX", hairIndex);
        PlayerPrefs.SetInt("COLOR_INDEX", hairColorIndex);
        PlayerPrefs.SetInt("CLOTH_INDEX", clothesIndex);

        PlayerPrefs.Save();

        Debug.Log($"[CustomizationManager] Saved PlayerName='{playerName}', HairIndex={hairIndex}, HairColorIndex={hairColorIndex}, ClothesIndex={clothesIndex}. Move scene: {nextSceneName}");

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogWarning("[CustomizationManager] nextSceneName is empty. Scene load skipped.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private void LoadSavedCustomization()
    {
        if (nameInput != null)
        {
            nameInput.text = PlayerPrefs.GetString(PlayerNameKey, nameInput.text);
        }

        hairIndex = ClampIndex(GetSavedInt(HairIndexKey, "HAIR_INDEX", hairIndex), GetLength(hairSprites));
        hairColorIndex = ClampIndex(GetSavedInt(HairColorIndexKey, "COLOR_INDEX", hairColorIndex), GetLength(hairColors));
        clothesIndex = ClampIndex(GetSavedInt(ClothesIndexKey, "CLOTH_INDEX", clothesIndex), GetLength(clothesSprites));
    }

    private void ApplyPreview()
    {
        if (hairImage != null)
        {
            if (HasItems(hairSprites))
            {
                hairIndex = ClampIndex(hairIndex, hairSprites.Length);
                hairImage.sprite = hairSprites[hairIndex];
                hairImage.enabled = true;
            }

            if (HasItems(hairColors))
            {
                hairImage.color = hairColors[ClampIndex(hairColorIndex, hairColors.Length)];
            }
        }

        if (clothesImage != null && HasItems(clothesSprites))
        {
            clothesIndex = ClampIndex(clothesIndex, clothesSprites.Length);
            clothesImage.sprite = clothesSprites[clothesIndex];
            clothesImage.enabled = true;
        }
    }

    private void SyncFromLegacyController()
    {
        if (legacyCustomizeController == null)
        {
            return;
        }

        hairIndex = legacyCustomizeController.hairIndex;
        hairColorIndex = legacyCustomizeController.colorIndex;
        clothesIndex = legacyCustomizeController.clothIndex;
    }

    private void SyncToLegacyController()
    {
        if (legacyCustomizeController == null)
        {
            return;
        }

        legacyCustomizeController.hairIndex = ClampIndex(hairIndex, GetLegacyListCount(legacyCustomizeController.HairList));
        legacyCustomizeController.colorIndex = ClampIndex(hairColorIndex, GetLegacyListCount(legacyCustomizeController.ColorList));
        legacyCustomizeController.clothIndex = ClampIndex(clothesIndex, GetLegacyListCount(legacyCustomizeController.ClothList));

        legacyCustomizeController.UpdateHair();
        legacyCustomizeController.UpdateColor();
        legacyCustomizeController.UpdateCloth();
    }

    private int GetNextIndex(int currentIndex, int length)
    {
        if (length <= 0)
        {
            return 0;
        }

        return (ClampIndex(currentIndex, length) + 1) % length;
    }

    private int GetSavedInt(string key, string fallbackKey, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        return PlayerPrefs.GetInt(fallbackKey, defaultValue);
    }

    private int GetPrevIndex(int currentIndex, int length)
    {
        if (length <= 0)
        {
            return 0;
        }

        return (ClampIndex(currentIndex, length) - 1 + length) % length;
    }

    private int ClampIndex(int index, int length)
    {
        if (length <= 0)
        {
            return 0;
        }

        return Mathf.Clamp(index, 0, length - 1);
    }

    private int GetLength<T>(T[] array)
    {
        return array != null ? array.Length : 0;
    }

    private int GetLegacyListCount<T>(System.Collections.Generic.List<T> list)
    {
        return list != null ? list.Count : 0;
    }

    private bool HasItems<T>(T[] array)
    {
        return array != null && array.Length > 0;
    }
}
