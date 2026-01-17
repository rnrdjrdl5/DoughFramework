using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSchedulerTrait : Trait
{
	public IReadOnlyList<TurnTrait> Turns => turns;
	public TurnTrait ActivateTurn => activateTurn;

	List<TurnTrait> turns = new();

	TurnTrait activateTurn;
	
	public void AddTurn(TurnTrait turn)
	{ 
		turns.Add(turn);
	}

	public void RemoveTurn(TurnTrait turn)
	{
		if (!turns.Contains(turn))
		{
			return;
		}
		
		turns.Remove(turn);
	}

	public void ProcessTurnPoint()
	{
		var targetTurn = turns.OrderBy(turn => turn.LeftPoint).FirstOrDefault();
		var leftPoint = targetTurn.LeftPoint;
		
		foreach (var turn in turns)
		{
			if (turn.IsStop)
				continue;

			turn.LeftPoint -= leftPoint;
		}
	}

	public void ProcessTurn()
	{
		activateTurn = turns.OrderBy(turn => turn.LeftPoint).FirstOrDefault();
		if (activateTurn != null)
		{
			activateTurn.PlayTurn();
		}
	}

	public void ProcessFillPoint()
	{
		if (activateTurn != null)
		{
			activateTurn.FillPoint();
		}
	}
}
