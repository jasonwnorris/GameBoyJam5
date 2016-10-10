using UnityEngine;

public class Teleport : MonoBehaviour
{
    #region Constants

    private const string c_PlayerTag = "Player";
    private const string c_RootGameObjectName = "root";

    #endregion

    #region Public Variables

    public string SceneName;

    public string SpawnPointName;

    #endregion

    #region Components

    LevelManager m_LevelManager;

    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        m_LevelManager = GameObject.Find(c_RootGameObjectName).GetComponent<LevelManager>();

        if (m_LevelManager == null)
        {
            Debug.LogWarning("Entity " + gameObject.name + " is using Teleport script and could not find LevelManager.");
        }
    }

    void OnTriggerEnter2D(Collider2D p_Collider)
    {
        if (p_Collider.tag == c_PlayerTag)
        {
            m_LevelManager.ChangeMap(SceneName, SpawnPointName);
        }
    }

    #endregion
}
