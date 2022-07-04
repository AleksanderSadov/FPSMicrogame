using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.FPS.Game;
using UnityEngine;

public class HighscoreCounter : MonoBehaviour
{
    [Tooltip("The text field displaying the score")]
    public TextMeshProUGUI UIText;

    private string defaultText;
    private HighscoreManager highscoreManager;

    // Start is called before the first frame update
    void Start()
    {
        highscoreManager = GameObject.FindObjectOfType<HighscoreManager>();
        DebugUtility.HandleErrorIfNullFindObject<HighscoreManager, HighscoreCounter>(
                highscoreManager, this);

        defaultText = UIText.text;
    }

    // Update is called once per frame
    void Update()
    {
        UIText.text = defaultText + highscoreManager.GetCurrentScore();
    }
}
