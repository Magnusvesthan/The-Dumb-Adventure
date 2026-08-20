using System;
using System.Collections.Generic;

namespace SurvivalGame.Models
{
	public class GameEvent
	{
		public string Scenario { get; set; }
		public List<EventOption> Options { get; set; }
	}
}
