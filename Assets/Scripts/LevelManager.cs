using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Constants

    #endregion

    #region Public Variables

    public Transform PlayerTransform;
    public string SceneName;
    public string SpawnPointName;

    #endregion

    #region Members

    private string m_SceneName;
    private string m_SpawnPointName;

    #endregion

    #region MonoBehaviour Methods

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
            PlayerTransform.position = spawnPoint.transform.position;
            Camera.main.transform.position = PlayerTransform.position;
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
