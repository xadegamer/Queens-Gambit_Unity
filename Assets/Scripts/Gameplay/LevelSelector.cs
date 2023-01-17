using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] int levelSceneIndex;
    [SerializeField] Color unlockedColour;
    [SerializeField] Color lockedColour;

    private Image buttonImage;
    private Button button;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        if (levelSceneIndex > GameManager.Instance.GetLastUnlockedLevel())
        {
            buttonImage.color = lockedColour;
            button.interactable = false;
        }
        else buttonImage.color = unlockedColour;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelSceneIndex);
    }
}
