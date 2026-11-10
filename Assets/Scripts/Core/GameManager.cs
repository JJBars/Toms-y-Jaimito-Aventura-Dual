using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla los estados del flujo del juego: Menú, Jugando, Nivel Completado y Game Over.
/// Se encarga de instanciar a los jugadores y gestionar reinicios de nivel.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Menu,
        Playing,
        LevelCompleted,
        GameOver
    }

    [Header("Prefabs de jugadores")]
    [Tooltip("Arrastra aquí el prefab de Tom")]
    [SerializeField] private GameObject tomPrefab;

    [Tooltip("Arrastra aquí el prefab de Jaimito")]
    [SerializeField] private GameObject jerryPrefab;

    [Header("Puntos de inicio")]
    [Tooltip("Posición donde aparece Tom al iniciar el nivel")]
    [SerializeField] private Transform tomSpawnPoint;

    [Tooltip("Posición donde aparece Jaimito al iniciar el nivel")]
    [SerializeField] private Transform jerrySpawnPoint;

    public GameState CurrentState { get; private set; } = GameState.Menu;

    private GameObject tomInstance;
    private GameObject jerryInstance;

    private void Awake()
    {
        // Patrón singleton para persistir entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CurrentState = GameState.Menu;
    }

    /// <summary>
    /// Inicia una partida: cambia estado a Playing y crea a ambos jugadores.
    /// </summary>
    public void StartGame()
    {
        CurrentState = GameState.Playing;
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if (tomPrefab != null && tomSpawnPoint != null)
        {
            tomInstance = Instantiate(tomPrefab, tomSpawnPoint.position, Quaternion.identity);
            tomInstance.name = "Tom";
        }

        if (jerryPrefab != null && jerrySpawnPoint != null)
        {
            jerryInstance = Instantiate(jerryPrefab, jerrySpawnPoint.position, Quaternion.identity);
            jerryInstance.name = "Jerry";
        }
    }

    /// <summary>
    /// Llama al completar el nivel (ambos jugadores llegaron a la meta).
    /// </summary>
    public void CompleteLevel()
    {
        if (CurrentState == GameState.LevelCompleted) return;
        CurrentState = GameState.LevelCompleted;
        Debug.Log("¡Nivel completado!");
        // Aquí puedes activar una UI de victoria o cargar el siguiente nivel
    }

    /// <summary>
    /// Llama cuando algún jugador muere o falla el nivel.
    /// </summary>
    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) return;
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over");
    }

    /// <summary>
    /// Reinicia el nivel actual.
    /// </summary>
    public void RestartLevel()
    {
        CurrentState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    public void ReturnToMenu()
    {
        CurrentState = GameState.Menu;
        SceneManager.LoadScene("Menú");
    }

    public void DestroyPlayers()
    {
        if (tomInstance != null) Destroy(tomInstance);
        if (jerryInstance != null) Destroy(jerryInstance);
    }
}
