using Microsoft.AspNetCore.Mvc;
using SurvivalGame.Models;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
namespace SurvivalGame.Controllers
{
	[ApiController]
	[Route("api/event")]
	public class EventController : ControllerBase
	{
		private static Random rnd = new Random();
		private static List<GameEvent> events = new List<GameEvent>
		{
			new GameEvent 
			{
				Scenario = " En gås stirrer vredt på dig ",
				Options = new List<EventOption> {
					new EventOption { Text = "Klap gåsen", Points = -10, Result = "Gåsen angriber dig!"},
					new EventOption { Text = "Løb væk", Points = 5, Result = "Du undslap Gåsen!"},
					new EventOption { Text = "Honk Tilbage", Points = 10, Result = "Gåsen respekterer dig!"}
				}
			},
			new GameEvent
			{
				Scenario = "Du Taber Din is på gulvet",
				Options = new List<EventOption> {
					new EventOption { Text = "Spis den alligvel", Points = -20, Result = "Du bliver syg."},
					new EventOption { Text = "Køb en ny", Points = 5, Result = "Du får en ny is!"},
					new EventOption { Text = "Græd Dramatisk", Points = 10, Result = "Nogen giver dig en gratis is."}
				}
			},

			new GameEvent
			{
				Scenario = "En kat udfordrer dig til sten-saks-papir.",
				Options = new List<EventOption> {
					new EventOption { Text = "Sten", Points = rnd.Next(-10, 10), Result = "Katten valgte tilfældigt"},
					new EventOption { Text = "Saks", Points = rnd.Next(-10, 10), Result = "Katten valgte tilfældigt"},
					new EventOption { Text = "Papir", Points = rnd.Next(-10, 10), Result = "Katten valgte tilfældigt"}
				}
			}

		};
		[HttpGet]
		public IActionResult GetRandomEvent()
		{
			var e = events[rnd.Next(events.Count)];
			return Ok(e);
		}
	}
}

