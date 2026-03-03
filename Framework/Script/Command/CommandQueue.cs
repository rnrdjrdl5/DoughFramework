using System;
using System.Collections.Generic;

public class CommandQueue
{
    public event Action<Command> OnEnqueued;
    public int Count => queue.Count;
    public bool IsEmpty => queue.Count == 0;
    
    readonly Queue<Command> queue = new Queue<Command>();
    
    public void Enqueue(Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        queue.Enqueue(command);
        OnEnqueued?.Invoke(command);
    }
    
    public void EnqueueRange(IEnumerable<Command> commands)
    {
        if (commands == null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        foreach (var command in commands)
        {
            Enqueue(command);
        }
    }
    
    public Command Peek()
    {
        return queue.Count > 0 ? queue.Peek() : null;
    }
    
    public Command Dequeue()
    {
        return queue.Count > 0 ? queue.Dequeue() : null;
    }
    
    public bool TryDequeue(out Command command)
    {
        if (queue.Count > 0)
        {
            command = queue.Dequeue();
            return true;
        }

        command = null;
        return false;
    }
    
    public bool ExecuteNext()
    {
        if (!TryDequeue(out var command))
        {
            return false;
        }

        command.Execute();
        return true;
    }
    
    public int ExecuteAll()
    {
        var executed = 0;
        while (ExecuteNext())
        {
            executed++;
        }

        return executed;
    }
    
    public void Clear()
    {
        queue.Clear();
    }
}
