using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSchedulerAbility : Ability
{
	public IReadOnlyList<TurnAbility> Turns => turns;
	public TurnAbility ActivateTurn => activateTurn;

	List<TurnAbility> turns = new();

	TurnAbility activateTurn;
	
	public void AddTurn(TurnAbility turn)
	{ 
		turns.Add(turn);
	}

	public void RemoveTurn(TurnAbility turn)
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
