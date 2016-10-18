using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Constants

    #endregion

    #region Public Variables

    public GameObject Player;
    public string SceneName;
    public string SpawnPointName;

    #endregion

    #region Components

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;

    #endregion

    #region Members

    private string m_SceneName;
    private string m_SpawnPointName;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_Transform = Player.GetComponent<Transform>();
        m_Rigidbody = Player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        ChangeMap(SceneName, SpawnPointName);
    }

    #endregion

    #region Public Methods

    public void ChangeMap(string p_SceneName, string p_SpawnPointName)
    {
        m_SceneName = p_SceneName;
        m_SpawnPointName = p_SpawnPointName;

        SceneManager.LoadScene(m_SceneName);
    }

    #endregion

    #region Private Methods

    private void MoveToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.Find(m_SpawnPointName);
        if (spawnPoint != null)
        {
            m_Transform.position = spawnPoint.transform.position;
            m_Rigidbody.velocity = Vector3.zero;

            Camera.main.transform.position = m_Transform.position;
        }
    }

    #endregion

    #region Events

    private void OnSceneLoaded(Scene p_Scene, LoadSceneMode p_Mode)
    {
        MoveToSpawnPoint();
    }

    #endregion
}
