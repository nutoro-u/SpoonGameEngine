using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpoonEditor.Utils
{
	public interface IUndoRedo
	{
		string Name { get; }
		void Undo();
		void Redo();
	}

	public class UndoRedoAction : IUndoRedo
	{
		private Action _undoAction;
		private Action _redoAction;

		public string Name { get; }

		public void Redo() => _redoAction();

		public void Undo() => _undoAction();

		public UndoRedoAction(string name)
		{
			Name = name;
		}

		public UndoRedoAction(Action undoAction, Action redoAction, string name) : this(name)
		{
			Debug.Assert(undoAction != null && redoAction != null);
			_undoAction = undoAction;
			_redoAction = redoAction;
		}

		public UndoRedoAction(string property, object instance, object undoValue, object redoValue, string name) :
			this(
				() => instance.GetType().GetProperty(property).SetValue(instance, undoValue),
				() => instance.GetType().GetProperty(property).SetValue(instance, redoValue), name)
		{

		}
	}

	public class UndoRedo
	{
		private bool enableAdd = true;

		private readonly ObservableCollection<IUndoRedo> _redoList = new ObservableCollection<IUndoRedo>();
		private readonly ObservableCollection<IUndoRedo> _undoList = new ObservableCollection<IUndoRedo>();

		public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; }
		public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; }

		public UndoRedo()
		{
			RedoList = new ReadOnlyObservableCollection<IUndoRedo>(_redoList);
			UndoList = new ReadOnlyObservableCollection<IUndoRedo>(_undoList);
		}

		public void Undo()
		{
			if(_undoList.Any())
			{
				IUndoRedo cmd = _undoList.Last();
				_undoList.RemoveAt(_undoList.Count - 1);
				enableAdd = false;
				cmd.Undo();
				enableAdd = true;
				_redoList.Insert(0, cmd);
			}
		}

		public void Redo()
		{
			if(_redoList.Any())
			{
				IUndoRedo cmd = _redoList.First();
				_redoList.RemoveAt(0);
				enableAdd = false;
				cmd.Redo();
				enableAdd = true;
				_undoList.Add(cmd);
			}
		}

		public void Add(IUndoRedo cmd)
		{
			if (enableAdd)
			{
				_undoList.Add(cmd);
				_redoList.Clear();
			}
		}

		public void Reset()
		{
			_redoList.Clear();
			_undoList.Clear();
		}
	}
}
