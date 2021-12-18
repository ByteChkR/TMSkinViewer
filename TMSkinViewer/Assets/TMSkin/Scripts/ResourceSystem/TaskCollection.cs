using System;
using System.Collections;
using System.Collections.Generic;

public class TaskCollection
{
    public delegate void OnProgress(int progress, int maxProgress, string status);

    private class Task
    {

        public readonly string Name;
        public readonly Action<OnProgress> Action;

        #region Public

        public Task( string name, Action<OnProgress> action )
        {
            Name = name;
            Action = action;
        }

        #endregion

    }

    private readonly List < Task > m_Tasks = new List < Task >();

    #region Public

    public void AddTask( string name, Action action )
    {
        m_Tasks.Add( new Task( name, p => action?.Invoke() ) );
    }
    public void AddTask( string name, Action<OnProgress> action )
    {
        m_Tasks.Add( new Task( name, action));
    }

    public IEnumerator ProcessTasks( Action onComplete, OnProgress onProgress )
    {
        for ( int i = 0; i < m_Tasks.Count; i++ )
        {
            Task task = m_Tasks[i];
            onProgress?.Invoke( i, m_Tasks.Count, $"Task: {task.Name}" );
            task.Action?.Invoke(onProgress);

            yield return null;
        }

        onComplete?.Invoke();
    }

    #endregion

}
