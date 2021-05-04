using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communication : IObserver, ISubject
{
	private List<IObserver> _observers = new List<IObserver>();

	public void Attach(IObserver observer)
	{
		_observers.Add(observer);
	}

	public void Detach(IObserver observer)
	{
		_observers.Remove(observer);
	}

	public void Notify()
	{
		foreach (var observer in _observers)
		{
			observer.Update(this);
		}
	}

	public void Update(ISubject subject)
	{
		throw new System.NotImplementedException();
	}
}
