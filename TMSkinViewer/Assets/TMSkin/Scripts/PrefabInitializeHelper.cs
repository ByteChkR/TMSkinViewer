using System;

using UI.LoadingWindow;

using UnityEngine;

public class PrefabInitializeHelper : MonoBehaviour
{

    public event Action OnInitialized;

    public event Action OnFinalize;
    private readonly TaskCollection m_Tasks = new TaskCollection();
    public TaskCollection Tasks => m_Tasks;

    public void Start()
    {
        OnFinalize?.Invoke();
        
        LoadingWindow window =LoadingWindowBuilder.CreateWindow();
        
        window.OnComplete+=()=>
                           {
                               if (OnInitialized != null)
                               {
                                   OnInitialized();
                               }
                           };
        
        
        window.Process( m_Tasks );
        
    }

}