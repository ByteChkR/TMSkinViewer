using System;
using System.Collections;
using System.Collections.Generic;

public class TaskCollection
{

    private class Task
    {

        public readonly string Name;
        public readonly Action Action;

        public Task( string name, Action action )
        {
            Name = name;
            Action = action;
        }

    }

    private readonly List < Task > m_Tasks = new List < Task >();
    
    public void AddTask( string name, Action action )
    {
        m_Tasks.Add( new Task( name, action ) );
    }
    
    public IEnumerator ProcessTasks(Action onComplete, Action<int, int, string> onProgress)
    {
        for ( int i = 0; i < m_Tasks.Count; i++ )
        {
            Task task = m_Tasks[i];
            onProgress?.Invoke(i, m_Tasks.Count, $"Task: {task.Name}" );
            task.Action();

            yield return null;
        }
        
        onComplete?.Invoke();
    }

}