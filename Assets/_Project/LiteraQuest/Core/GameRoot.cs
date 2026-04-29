using UnityEngine;

[DisallowMultipleComponent]
public sealed class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    [Header("Core")]
    [SerializeField] private AppState appState;
    [SerializeField] private SceneLoader sceneLoader;

    [Header("API")]
    [SerializeField] private ApiConfig apiConfig;

    public AppState AppState => appState;
    public SceneLoader SceneLoader => sceneLoader;

    public AuthTokenStore TokenStore { get; private set; }
    public JsonSerializerService JsonSerializer { get; private set; }
    public ApiClient ApiClient { get; private set; }
    public AuthService AuthService { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ValidateReferences();
        InitializeServices();

        sceneLoader.Initialize(appState);
    }

    private void Start()
    {
        appState.SetCurrentScene(SceneNames.Bootstrap);
    }

    private void ValidateReferences()
    {
        if (appState == null)
        {
            appState = GetComponentInChildren<AppState>();
        }

        if (sceneLoader == null)
        {
            sceneLoader = GetComponentInChildren<SceneLoader>();
        }

        if (appState == null)
        {
            Debug.LogError("GameRoot is missing AppState.");
        }

        if (sceneLoader == null)
        {
            Debug.LogError("GameRoot is missing SceneLoader.");
        }

        if (apiConfig == null)
        {
            Debug.LogError("GameRoot is missing ApiConfig.");
        }
    }

    private void InitializeServices()
    {
        TokenStore = new AuthTokenStore();
        JsonSerializer = new JsonSerializerService();

        if (apiConfig != null)
        {
            ApiClient = new ApiClient(apiConfig, TokenStore, JsonSerializer);
            AuthService = new AuthService(ApiClient, TokenStore);
        }
    }
}