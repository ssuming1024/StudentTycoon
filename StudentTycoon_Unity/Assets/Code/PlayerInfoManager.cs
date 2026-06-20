using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInfoManager : MonoBehaviour
{
    public List<Image> HairList;
    public List<Color> ColorList;
    public List<Image> ClothList;
    public int hairIndex = 0;
    public int colorIndex = 0;
    public int clothIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hairIndex = PlayerPrefs.GetInt("HAIR_INDEX");
        colorIndex = PlayerPrefs.GetInt("COLOR_INDEX");
        clothIndex = PlayerPrefs.GetInt("CLOTH_INDEX");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
