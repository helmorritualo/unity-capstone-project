using UnityEngine;

[DisallowMultipleComponent]
public sealed class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    [SerializeField] private AppState appState;
    [SerializeField] private SceneLoader sceneLoader;

    public AppState AppState => appState;
    public SceneLoader SceneLoader => sceneLoader;

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
    }
}