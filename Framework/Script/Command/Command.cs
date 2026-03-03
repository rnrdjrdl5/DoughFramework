using System;

public abstract class Command
{
    public bool IsExecuted { get; private set; }
    
    public string Name { get; protected set; }
    
    public virtual bool CanExecute()
    {
        return true;
    }
    
    public bool Execute()
    {
        if (!CanExecute())
        {
            return false;
        }

        OnExecute();
        IsExecuted = true;
        return true;
    }
    
    public bool Undo()
    {
        if (!IsExecuted)
        {
            return false;
        }

        OnUndo();
        IsExecuted = false;
        return true;
    }
    
    public bool Redo()
    {
        if (IsExecuted)
        {
            return false;
        }

        return Execute();
    }
    
    protected abstract void OnExecute();
    
    protected virtual void OnUndo()
    {
        
    }
    
    public static Command Create(Action execute, Action undo = null, Func<bool> canExecute = null, string name = null)
    {
        return new DelegateCommand(execute, undo, canExecute, name);
    }

    private sealed class DelegateCommand : Command
    {
        private readonly Action _execute;
        private readonly Action _undo;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Action undo, Func<bool> canExecute, string name)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _undo = undo;
            _canExecute = canExecute;
            Name = name ?? "Command";
        }

        public override bool CanExecute()
        {
            return _canExecute == null || _canExecute();
        }

        protected override void OnExecute()
        {
            _execute();
        }

        protected override void OnUndo()
        {
            _undo?.Invoke();
        }
    }
}
