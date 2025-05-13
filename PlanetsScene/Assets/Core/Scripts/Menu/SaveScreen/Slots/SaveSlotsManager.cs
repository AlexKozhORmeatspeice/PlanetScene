using System;
using System.Collections;
using System.Collections.Generic;
using Reptutation_Screen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Save_screen
{
    public interface ISaveSlotsManager
    {
        event Action<SaveInfo> onDeleteSlot;

        void DeleteSlot(SaveInfo info);
        void AddSLot(SaveInfo slot);
    }

    public class SaveSlotsManager : MonoBehaviour, ISaveSlotsManager, IStartable, IDisposable
    {
        private struct SlotObservers
        {
            public ISaveSlotObserver saveSlotObserver;
            public IDeleteButtonObserver deleteButtonObserver;
            public IRewriteButtonObserver rewriteButtonsObserver;

            public void EnableObservers()
            {
                if(saveSlotObserver != null) 
                    saveSlotObserver.Enable();
                
                if(deleteButtonObserver != null)
                    deleteButtonObserver.Enable();
                
                if(rewriteButtonsObserver != null)
                    rewriteButtonsObserver.Enable();
            }
            public void DisableObservers()
            {
                if (saveSlotObserver != null)
                    saveSlotObserver.Disable();

                if (deleteButtonObserver != null)
                    deleteButtonObserver.Disable();

                if (rewriteButtonsObserver != null)
                    rewriteButtonsObserver.Disable();
            }
        }

        [Inject] private IObjectResolver resolver;
        [Inject] private ISaveManager saveManager;

        [Header("UI settings")]
        [SerializeField] private Transform startTransform;
        [SerializeField] private RectTransform spawnRect;
        [SerializeField] private float distBetweenY = 5.0f;

        [Header("Objs")]
        [SerializeField] private Transform parentObject;
        [SerializeField] private SaveSlot slotPrefab;
        [SerializeField] private NewSaveSlot newSaveSlotPrefab;

        private Dictionary<SaveInfo, SlotObservers> observersBySaveInfo;

        private ISaveSlotObserver newSaveSlotObserver;
        private ISaveButtonObserver saveButtonObserver;

        private List<Vector3> slotPositions;
        
        public event Action<SaveInfo> onDeleteSlot;

        public void Initialize()
        {
            observersBySaveInfo = new Dictionary<SaveInfo, SlotObservers>();
            
            foreach (SaveInfo info in saveManager.SaveInfos)
            { 
                AddSLot(info);
            }
            CreateNewSaveSlot();

            saveManager.onCreateNewSave += AddSLot;
            saveManager.onDeleteSave += DeleteSlot;
        }

        public void Start() { }

        public void Dispose()
        {
            foreach (SaveInfo info in observersBySaveInfo.Keys)
            {
                SlotObservers slotObservers = observersBySaveInfo[info];

                slotObservers.DisableObservers();
            }

            saveManager.onCreateNewSave -= AddSLot;
            saveManager.onDeleteSave -= DeleteSlot;
        }

        public void DeleteSlot(SaveInfo info)
        {
            if (!observersBySaveInfo.ContainsKey(info))
                return;
            
            SlotObservers slotObservers = observersBySaveInfo[info];

            observersBySaveInfo.Remove(info);
            
            onDeleteSlot?.Invoke(info);
            slotObservers.DisableObservers();
        }

        public void AddSLot(SaveInfo info)
        {
            ISaveSlot newSaveSlot = GameObject.Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, parentObject);
            
            ISaveSlotObserver observer = new SaveSlotObserver(newSaveSlot, info);
            IDeleteButtonObserver deleteObserver = new DeleteButtonObserver(newSaveSlot.DeleteButton, newSaveSlot);
            IRewriteButtonObserver rewriteObserver = new RewriteButtonObserver(newSaveSlot.RewriteButton, newSaveSlot);

            resolver.Inject(observer);
            resolver.Inject(deleteObserver);
            resolver.Inject(rewriteObserver);

            SlotObservers newObservers = new SlotObservers();

            newObservers.saveSlotObserver = observer;
            newObservers.deleteButtonObserver = deleteObserver;
            newObservers.rewriteButtonsObserver = rewriteObserver;

            newObservers.EnableObservers();

            observersBySaveInfo[info] = newObservers;
        }

        private void CreateNewSaveSlot()
        {
            newSaveSlotObserver = new NewGameSaveSlotObserver(newSaveSlotPrefab);
            saveButtonObserver = new SaveButtonObserver(newSaveSlotPrefab.SaveButton, newSaveSlotPrefab);

            resolver.Inject(newSaveSlotObserver);
            resolver.Inject(saveButtonObserver);

            newSaveSlotObserver.Enable();
            saveButtonObserver.Enable();
        }
    }

}

