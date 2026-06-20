using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomizeController : MonoBehaviour
{
    public List<Image> HairList;
    public List<Color> ColorList;
    public List<Image> ClothList;

    public Image ColorPanel;

    public int hairIndex = 0;
    public int colorIndex = 0;
    public int clothIndex = 0;

    public Button HairIndexUpButton;
    public Button HairIndexDownButton;
    public Button ColorIndexUpButton;
    public Button ColorIndexDownButton;
    public Button ClothIndexUpButton;
    public Button ClothIndexDownButton;

    public List<Image> FinalHairImageList;
    public List<Image> FinalClothList;

    void Start()
    {
        HairIndexUpButton.onClick.AddListener(OnHairIndexUp);
        HairIndexDownButton.onClick.AddListener(OnHairIndexDown);

        ColorIndexUpButton.onClick.AddListener(OnColorIndexUp);
        ColorIndexDownButton.onClick.AddListener(OnColorIndexDown);

        ClothIndexUpButton.onClick.AddListener(OnClothIndexUp);
        ClothIndexDownButton.onClick.AddListener(OnClothIndexDown);

        UpdateHair();
        UpdateColor();
        UpdateCloth();
    }

    public void OnHairIndexUp()
    {
        if (hairIndex < HairList.Count - 1)
            hairIndex++;
        else
            hairIndex = 0;

        UpdateHair();
    }

    public void OnHairIndexDown()
    {
        if (hairIndex > 0)
            hairIndex--;
        else
            hairIndex = HairList.Count - 1;

        UpdateHair();
    }

    public void OnColorIndexUp()
    {
        if (colorIndex < ColorList.Count - 1)
            colorIndex++;
        else
            colorIndex = 0;

        UpdateColor();
    }

    public void OnColorIndexDown()
    {
        if (colorIndex > 0)
            colorIndex--;
        else
            colorIndex = ColorList.Count - 1;

        UpdateColor();
    }

    public void OnClothIndexUp()
    {
        if (clothIndex < ClothList.Count - 1)
            clothIndex++;
        else
            clothIndex = 0;

        UpdateCloth();
    }

    public void OnClothIndexDown()
    {
        if (clothIndex > 0)
            clothIndex--;
        else
            clothIndex = ClothList.Count - 1;

        UpdateCloth();
    }

    public void UpdateColor()
    {
        if (ColorList.Count == 0) return;

        ColorPanel.color = ColorList[colorIndex];
        UpdateFinalHairColor();
    }

    public void UpdateHair()
    {
        for (int i = 0; i < HairList.Count; i++)
        {
            HairList[i].gameObject.SetActive(false);
        }

        if (HairList.Count > 0)
        {
            HairList[hairIndex].gameObject.SetActive(true);
        }

        UpdateFinalHairColor();
    }

    public void UpdateFinalHairColor()
    {
        for (int i = 0; i < FinalHairImageList.Count; i++)
        {
            FinalHairImageList[i].gameObject.SetActive(false);
        }

        if (FinalHairImageList.Count > 0)
        {
            FinalHairImageList[hairIndex].gameObject.SetActive(true);
            FinalHairImageList[hairIndex].color = ColorPanel.color;
        }
    }

    public void UpdateCloth()
    {
        for (int i = 0; i < ClothList.Count; i++)
        {
            ClothList[i].gameObject.SetActive(false);
        }

        if (ClothList.Count > 0)
        {
            ClothList[clothIndex].gameObject.SetActive(true);
        }

        
        for (int i = 0; i < FinalClothList.Count; i++)
        {
            FinalClothList[i].gameObject.SetActive(false);
        }

        if (FinalClothList.Count > 0)
        {
            FinalClothList[clothIndex].gameObject.SetActive(true);
        }
    }
}