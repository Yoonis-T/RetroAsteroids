using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Prefabs")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject playerPrefab;

    [Header("Game Settings")]
    [SerializeField] private int startingAsteroids = 5;
    [SerializeField] private int asteroidsPerLevelIncrease = 2;
    [SerializeField] private float spawnForce = 5f;

    private int score = 0;
    private int currentLevel = 1;
    private int lives = 3;
    private bool isGameOver = false;
    private bool levelStarting = false;

    private Camera cam;
    private Vector2 screenBounds;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        // Set starting UI text
        if (scoreText != null) scoreText.text = "Score: 0";
        if (livesText != null) livesText.text = "Lives: 3";
        if (levelText != null) levelText.text = "Level: 1";

        SpawnPlayer();
        StartLevel(currentLevel);
    }

    private void Update()
    {
        if (isGameOver) return;

        // When all asteroids destroyed → start next level
        if (!levelStarting &&
            GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            levelStarting = true;
            Invoke(nameof(NextLevel), 2f); // wait 2 sec before next level
        }
    }

    private void NextLevel()
    {
        currentLevel++;
        StartLevel(currentLevel);
        levelStarting = false;
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }

    public void PlayerDied()
    {
        if (isGameOver) return;

        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(SpawnPlayer), 3f); // respawn after 3 sec
        }
    }

    private void StartLevel(int level)
    {
        UpdateLevelUI();

        // Increase asteroid speed each level
        spawnForce = 5f + level * 0.3f;

        int asteroidCount = startingAsteroids + (level - 1) * asteroidsPerLevelIncrease;
        SpawnAsteroids(asteroidCount);
    }

    private void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = GetSpawnPositionOutsideScreen();
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
            asteroid.tag = "Asteroid";

            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                rb.AddForce(direction * spawnForce, ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
            }
        }
    }

    private Vector2 GetSpawnPositionOutsideScreen()
    {
        float x, y;

        if (Random.value > 0.5f)
        {
            x = Random.value > 0.5f ? screenBounds.x + 1f : -screenBounds.x - 1f;
            y = Random.Range(-screenBounds.y, screenBounds.y);
        }
        else
        {
            x = Random.Range(-screenBounds.x, screenBounds.x);
            y = Random.value > 0.5f ? screenBounds.y + 1f : -screenBounds.y - 1f;
        }

        return new Vector2(x, y);
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Level: " + currentLevel;
    }

    public void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}