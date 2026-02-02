using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;
    public TextMeshProUGUI countText;

    public int score = 0;
    void Start()
    {
        SetCountText();
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoint()
    {
        score++;
        SetCountText();
    }
    void SetCountText()
    {
        countText.text = "Count: " + score.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
