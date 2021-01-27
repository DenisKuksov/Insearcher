using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeScript : MonoBehaviour
{

   [System.Serializable] struct GameMode
    {
        public string name;
        public int sceneIndex;
    }

    [SerializeField] private int _tutorialSceneIndex;
    [SerializeField] private Text _gameModeText;
    [SerializeField] private GameMode[] _gameModes;

    private int _currentGameMode = 0;

    private void Start()
    {
        _currentGameMode = PlayerPrefs.GetInt("LastGameMode", 0);
        _gameModeText.text = _gameModes[_currentGameMode].name;
    }

    public void ChangeGameMode()
    {
        _currentGameMode++;
        if (_currentGameMode == _gameModes.Length)
            _currentGameMode = 0;

        _gameModeText.text = _gameModes[_currentGameMode].name;

    }

    public void Play()
    {
        if (PlayerPrefs.GetInt("TutorialsFinished", 0) < 1)
        {
            SceneManager.LoadScene(_tutorialSceneIndex);
        }
        else
        {
            PlayerPrefs.SetInt("LastGameMode", _currentGameMode);
            SceneManager.LoadScene(_gameModes[_currentGameMode].sceneIndex);
        }
    }
}
