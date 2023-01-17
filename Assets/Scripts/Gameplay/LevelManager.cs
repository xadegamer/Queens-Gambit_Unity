using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] GameObject gameoverUI;
    [SerializeField] GameObject levelComplectedUI;
    [SerializeField] Transform collectableHolder;

    [Header("Debug")]
    [SerializeField] int totalPumpkin;
    [SerializeField] List<Collectable> allPumpkin;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;

        audioSource = GetComponent<AudioSource>();

        UIManager.Instance.SetScroreText(GameManager.Instance.GetScore());
    }

    private void OnEnable()
    {
        Collectable.OnPumpkinCollected += CheckPumpkinCount;
    }

    private void Start()
    {
        GetAllPumpkinInScene();
    }

    public void GetAllPumpkinInScene()
    {
        foreach (Transform item in collectableHolder)
        {
            Collectable collectable = item.GetComponent<Collectable>();
            if (collectable.type == Collectable.Type.Pumpkin) allPumpkin.Add(collectable);
        }

        totalPumpkin = allPumpkin.Count;

        UpdatePumkinText();
    }

    public void CheckPumpkinCount(Collectable collectable)
    {
        allPumpkin.Remove(collectable);
        UpdatePumkinText();

        if (allPumpkin.Count == 0) LevelComplected();
    }
    public void UpdatePumkinText()
    {
        UIManager.Instance.SetPumpkinText(totalPumpkin - allPumpkin.Count, totalPumpkin);
    }

    public void LevelComplected()
    {
        Time.timeScale = 0;
        audioSource.Play();

        FadeScreen.Instance.ScreenFade(Color.green, () => levelComplectedUI.SetActive(true));

        if (SceneManager.GetActiveScene().buildIndex < GameManager.Instance.GetMaxLevel())
            GameManager.Instance.IncrementLevel();
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= GameManager.Instance.GetMaxLevel())
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } 
    }

    public void GameOver()
    {
        FadeScreen.Instance.ScreenFade(Color.red, ()=> gameoverUI.SetActive(true));
    }

    private void OnDisable()
    {
        Collectable.OnPumpkinCollected -= CheckPumpkinCount;
    }
}
