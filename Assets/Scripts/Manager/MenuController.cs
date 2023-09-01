using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum SectionNavigation
{
    NEXT = 1,
    PREVIOUS = -1
}


/// <summary>
/// This class is responsible for controlling the game's main menu, including audio settings, loading scenes, and managing the pause menu.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> _volumeSources;
    [SerializeField]
    private List<Slider> _audioSliders;
    [SerializeField]
    private Sprite[] _sectionPreviewImages;
    [SerializeField]
    private Sprite[] _masterMindImages;
    [SerializeField]
    private List<GameObject> _sectionPreviewComponents;
    [SerializeField]
    private TextMeshProUGUI _sectionName;

    private DataManager _dataManager;

    private int _sectionIndex = 0;

    private string[] _sectionNames = { "Spencer Anwesen", "R.P.D.", "NEST 2", "Baker Anwesen", "Heisenbergs Fabrik" };

    void Awake()
    {
        _dataManager = Object.FindObjectOfType<DataManager>();
    }

    void Start()
    {
        LoadData();
    }

    /// <summary>
    /// Gets references to the inventory canvas, inventory panel, and pause screen gameobjects.
    /// </summary>
    /// <returns>A tuple containing references to the inventory canvas, inventory panel, and pause screen gameobjects.</returns>
    private (GameObject invCanvas, GameObject invPanel, GameObject pauseScreen) GetInventoryAndPauseMenuScreenData()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject invCanvas = canvas.transform.Find("InventoryCanvas").gameObject;
        GameObject invPanel = invCanvas.transform.Find("InventoryPanel").gameObject;
        GameObject pauseScreen = canvas.transform.Find("PauseScreen").gameObject;

        return (invCanvas, invPanel, pauseScreen);
    }


    public void OpenPauseMenu()
    {
        GameManager gameManagerInstance = FindObjectOfType<GameManager>();
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        var screenData = GetInventoryAndPauseMenuScreenData();

        if (screenData.invPanel.activeSelf)
            screenData.invPanel.SetActive(false);

        timer.PauseCountdown();
        screenData.pauseScreen.SetActive(true);
        gameManagerInstance.CanClick = false;
    }

    public void ClosePauseMenu()
    {
        GameManager gameManagerInstance = FindObjectOfType<GameManager>();
        var screenData = GetInventoryAndPauseMenuScreenData();

        if (screenData.invPanel.activeSelf == false)
            screenData.invPanel.SetActive(true);

        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        gameManagerInstance.CanClick = true;
        timer.StartCountdown();
    }


    //Audio methods

    /// <summary>
    /// Changes the volume of the music by setting the volume of the first audio source in the 
    /// _volumeSources array and updating the music volume in the data manager, if available.
    /// </summary>
    /// <param name="volume">The new volume value.</param>
    public void OnMusicVolumeChanged(float volume)
    {
        _volumeSources[0].volume = volume;

        if (_dataManager != null)
        {
            _dataManager.MusicVolume = _volumeSources[0].volume;
        }
    }

    /// <summary>
    /// Changes the volume of the sound effects by setting the volume of the second audio source in the 
    /// _volumeSources array and updating the SFX volume and danger SFX volume in the data manager, if available.
    /// </summary>
    /// <param name="volume">The new volume value.</param>
    public void OnSFXVolumeChanged(float volume)
    {
        _volumeSources[1].volume = volume;
        if (_dataManager != null)
        {
            _dataManager.SFXVolume = _volumeSources[1].volume;
            _dataManager.DangerSFXVolume = _volumeSources[1].volume;
        }
    }

    /// <summary>
    /// Changes the volume of the environment sound effects by setting the volume of the third audio source in the 
    /// _volumeSources array and updating the environment SFX volume in the data manager, if available.
    /// </summary>
    public void OnEnvSFXVolumeChanged(float volume)
    {
        _volumeSources[2].volume = volume;
        if (_dataManager != null)
        {
            _dataManager.EnvSFXVolume = _volumeSources[2].volume;
        }
    }

    //Data Management methods

    /// <summary>
    /// Saves the game data to the data manager when the back button is pressed, if available.
    /// </summary>
    public void OnBackPressed()
    {
        if (_dataManager != null)
        {
            _dataManager.Save();
        }
    }

    public void LoadData()
    {
        LoadAudioData();
    }


    //Scene Management methods
    public void LoadNextScene()
    {
        LevelManager._Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int index)
    {
        LevelManager._Instance.LoadScene(index);
    }

    public void LoadSection()
    {
        switch (_sectionIndex)
        {
            case 0:
                LevelManager._Instance.LoadScene(1);
                break;
            case 1:
                LevelManager._Instance.LoadScene(2);
                break;
        }
    }


    private void LoadAudioData()
    {
        if (_dataManager == null || _audioSliders[0] == null || _audioSliders[1] == null || _audioSliders[2] == null)
        {
            return;
        }
        _dataManager.Load();

        _volumeSources[0].volume = _dataManager.MusicVolume;
        _volumeSources[1].volume = _dataManager.SFXVolume;
        _volumeSources[2].volume = _dataManager.EnvSFXVolume;

        _audioSliders[0].value = _dataManager.MusicVolume;
        _audioSliders[1].value = _dataManager.SFXVolume;
        _audioSliders[2].value = _dataManager.EnvSFXVolume;
    }

    //Main Menu only section

    /// <summary>
    /// Retrieves the relevant game objects for the section panel UI screen.
    /// </summary>
    /// <returns>
    /// A tuple containing the section panel game object, 
    /// the left arrow button game object and the r
    /// ight arrow button game object.
    /// </returns>
    private (GameObject sectionPanel, GameObject leftArrowButton, GameObject rightArrowButton) GetSectionPanelScreenData()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject sectionPanel = canvas.transform.Find("Section_Panel").gameObject;
        GameObject leftArrowButton = sectionPanel.transform.Find("LeftArrowButton").gameObject;
        GameObject rightArrowButton = sectionPanel.transform.Find("RightArrowButton").gameObject;

        return (sectionPanel, leftArrowButton, rightArrowButton);
    }


    /// <summary>
    /// Updates the section preview components based on the given navigation direction.
    /// </summary>
    /// <param name="navigation">
    /// The direction of the navigation. 
    /// Use SectionNavigation.NEXT to navigate to the next section or SectionNavigation.
    /// PREVIOUS to navigate to the previous section.
    /// </param>
    private void UpdateSectionComponents(SectionNavigation navigation)
    {
        _sectionIndex += (int)navigation; 

        _sectionPreviewComponents[0].GetComponent<Image>().sprite = _masterMindImages[_sectionIndex];
        _sectionPreviewComponents[1].GetComponent<Image>().sprite = _sectionPreviewImages[_sectionIndex];
        _sectionName.text = _sectionNames[_sectionIndex];
    }

    /// <summary>
    /// Opens the next section in the section panel screen and updates the navigation buttons accordingly.
    /// </summary>
    public void OpenNextSection()
    {
        var screenData = GetSectionPanelScreenData();
        
        if(!screenData.leftArrowButton.activeSelf)
            screenData.leftArrowButton.SetActive(true);

        UpdateSectionComponents(SectionNavigation.NEXT);

        screenData.rightArrowButton.SetActive(_sectionIndex == 4 ? false : true);
    }

    /// <summary>
    /// Opens the previous section in the section panel screen and updates the navigation buttons accordingly.
    /// </summary>
    public void OpenPrevSection()
    {
        var screenData = GetSectionPanelScreenData();

        if(!screenData.rightArrowButton.activeSelf)
            screenData.rightArrowButton.SetActive(true);

        UpdateSectionComponents(SectionNavigation.PREVIOUS);

        screenData.leftArrowButton.SetActive(_sectionIndex == 0 ? false : true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

