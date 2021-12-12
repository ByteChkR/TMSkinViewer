using System;

using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingWindow
{

    public class LoadingWindow : MonoBehaviour
    {

        public event Action OnComplete;
        [SerializeField]
        private Window m_Window;
        [SerializeField]
        private Text m_Text;
        [SerializeField]
        private Slider m_Slider;


        public void Process(TaskCollection collection)
        {
            StartCoroutine( collection.ProcessTasks( TaskListCompleted, OnProgress ) );
        }

        private void TaskListCompleted()
        {
            OnComplete?.Invoke();
            m_Window.Close();
        }

        private void OnProgress( int currentTask, int totalTasks, string status )
        {
            m_Slider.value = currentTask / (float)totalTasks;
            m_Text.text = status;
        }

    }

}
