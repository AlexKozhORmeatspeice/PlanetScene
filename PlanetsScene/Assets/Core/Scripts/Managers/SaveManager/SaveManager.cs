
using System;
using System.Collections.Generic;
using Save_screen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[Serializable]
public struct SaveInfo
{
    public string name;
    public string chapter;
    public string date;

    public Texture screenshotTexture;
}

public interface ISaveManager
{
    event Action<SaveInfo> onCreateNewSave;
    event Action<SaveInfo> onDeleteSave;
    event Action onSave;
    event Action onDelete;
    List<SaveInfo> SaveInfos { get; }

    bool IsGotSave { get;  }

    void DeleteSave(SaveInfo saveInfo);
    void Load(SaveInfo info);
    void LoadLastSave();
    void Save(string name);
}

public class SaveManager : ISaveManager, IStartable, IDisposable
{
    [Inject] private IPopUpsManager popUpsManager;

    private List<SaveInfo> saveInfos;

    public List<SaveInfo> SaveInfos => saveInfos;

    public bool IsGotSave => saveInfos.Count > 0;

    public event Action<SaveInfo> onCreateNewSave;
    public event Action<SaveInfo> onDeleteSave;
    public event Action onSave;
    public event Action onDelete;

    public void Initialize()
    {
        LoadSaveData();

        EnablePopUpsEvents();
    }

    public void Dispose()
    {
        DisablePopUpsEvents();
    }

    public void Load(SaveInfo info)
    {
        Debug.Log("Loaded " + info.name);
    }

    public void DeleteSave(SaveInfo saveInfo)
    {
        saveInfos.Remove(saveInfo);

        onDeleteSave?.Invoke(saveInfo);
        onDelete?.Invoke();
    }

    public void Save(string name)
    {
        SaveInfo info = new SaveInfo();

        info.name = name;
        info.chapter = "тест";
        info.date = "тест";

        saveInfos.Add(info);
        
        onCreateNewSave?.Invoke(info);
        onSave?.Invoke();
    }

    public void Start() { }

    private void LoadSaveData()
    {
        //TODO: подгрузка из json данных
        saveInfos = new List<SaveInfo>();
    }

    private void EnablePopUpsEvents()
    {
        if (popUpsManager == null)
            return;

        popUpsManager.onClickSaveButton += Save;
        popUpsManager.onClickDeleteButton += DeleteSave;
    }

    private void DisablePopUpsEvents()
    {
        if (popUpsManager == null)
            return;

        popUpsManager.onClickSaveButton -= Save;
        popUpsManager.onClickDeleteButton -= DeleteSave;
    }

    public void LoadLastSave()
    {
        if (saveInfos.Count == 0)
            return;

        Load(saveInfos[saveInfos.Count - 1]);
    }
}