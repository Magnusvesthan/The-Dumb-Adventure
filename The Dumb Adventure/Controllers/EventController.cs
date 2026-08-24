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
					new EventOption { Text = "Sten", Points = 0, Result = "Katten valgte tilfældigt"},
					new EventOption { Text = "Saks", Points = 0, Result = "Katten valgte tilfældigt"},
					new EventOption { Text = "Papir", Points = 0, Result = "Katten valgte tilfældigt"}
				}
			},
			new GameEvent
			{
				Scenario = "Du finder en skattekiste i skoven.",
				Options = new List<EventOption> {
					new EventOption { Text = "Åbn den", Points = 20, Result = "Du fandt guld!"},
					new EventOption { Text = "Ignorer den", Points = 0, Result = "Du går videre uden at åbne kisten."},
					new EventOption { Text = "Tag den med hjem", Points = 10, Result = "Du tager kisten med hjem og finder nogle skatte senere."}
				}
			},
			new GameEvent
			{
				Scenario = "Du møder en venlig troldmand.",
				Options = new List<EventOption> {
					new EventOption { Text = "Bed om en trylleformular", Points = 15, Result = "Troldmanden giver dig en kraftfuld trylleformular!"},
					new EventOption { Text = "Spørg om råd", Points = 5, Result = "Troldmanden giver dig nogle nyttige tips."},
					new EventOption { Text = "Ignorer ham", Points = 0, Result = "Du går videre uden at interagere med troldmanden."}
				}
			},
			new GameEvent
			{
				Scenario = "Du finder en mystisk potion.",
				Options = new List<EventOption> {
					new EventOption { Text = "Drik den", Points = 20, Result = "Potionen giver dig superkræfter!"},
					new EventOption { Text = "Gem den til senere", Points = 5, Result = "Du gemmer potionen og kan bruge den senere."},
					new EventOption { Text = "Kast den væk", Points = -10, Result = "Du mister muligheden for at få superkræfter."}
				}
			},
			new GameEvent
			{
				Scenario = "Du støder på en flok vilde dyr.",
				Options = new List<EventOption> {
					new EventOption { Text = "Løb væk", Points = 5, Result = "Du undslap dyrene!"},
					new EventOption { Text = "Forsøg at tæmme dem", Points = -10, Result = "Dyrene angriber dig!"},
					new EventOption { Text = "Kast mad til dem", Points = 10, Result = "Dyrene bliver glade og lader dig passere."}
				}
			},
			new GameEvent
			{
				Scenario = "Du finder en skjult hule.",
				Options = new List<EventOption> {
					new EventOption { Text = "Udforsk hulen", Points = 15, Result = "Du finder skjulte skatte!"},
					new EventOption { Text = "Ignorer hulen", Points = 0, Result = "Du går videre uden at udforske hulen."},
					new EventOption { Text = "Tag en ven med ind", Points = 5, Result = "Din ven hjælper dig med at finde skatte i hulen."}
				}
			},
			new GameEvent
			{
				Scenario = "Du møder en gammel vis mand.",
				Options = new List<EventOption> {
					new EventOption { Text = "Spørg om visdom", Points = 10, Result = "Den gamle mand deler sin visdom med dig."},
					new EventOption { Text = "Ignorer ham", Points = 0, Result = "Du går videre uden at interagere med den gamle mand."},
					new EventOption { Text = "Bed om en gave", Points = 5, Result = "Den gamle mand giver dig en magisk genstand."}
				}
			},
			new GameEvent
			{
				Scenario = "Du finder en skjult skattekort.",
				Options = new List<EventOption> {
					new EventOption { Text = "Følg kortet", Points = 20, Result = "Du finder en skjult skat!"},
					new EventOption { Text = "Ignorer kortet", Points = 0, Result = "Du går videre uden at følge kortet."},
					new EventOption { Text = "Del kortet med en ven", Points = 5, Result = "Din ven hjælper dig med at finde skatten."}
				}
			},
			new GameEvent
			{
				Scenario = "Du støder på en mystisk portal.",
				Options = new List<EventOption> {
					new EventOption { Text = "Gå igennem portalen", Points = 15, Result = "Du bliver transporteret til en anden dimension!"},
					new EventOption { Text = "Ignorer portalen", Points = 0, Result = "Du går videre uden at interagere med portalen."},
					new EventOption { Text = "Undersøg portalen nærmere", Points = 5, Result = "Du finder ud af, at portalen fører til en skjult skattekiste."}
				}
			},
			new GameEvent
			{
				Scenario = "Du møder en venlig fe.",
				Options = new List<EventOption> {
					new EventOption { Text = "Bed om et ønske", Points = 20, Result = "Feen opfylder dit ønske!"},
					new EventOption { Text = "Ignorer feen", Points = 0, Result = "Du går videre uden at interagere med feen."},
					new EventOption { Text = "Spørg om råd", Points = 5, Result = "Feen giver dig nogle nyttige tips."}
				}
			},
			new GameEvent
			{
				Scenario = "Du møder en lille pige i skoven, der græder.",
				Options = new List<EventOption> {
					new EventOption { Text = "Trøst hende", Points = 10, Result = "Pigen bliver glad og takker dig."},
					new EventOption { Text = "Ignorer hende", Points = 0, Result = "Du går videre uden at hjælpe pigen."},
					new EventOption { Text = "Spørg hvad der er galt", Points = 5, Result = "Pigen fortæller dig, at hun har mistet sin bamse."},
					new EventOption { Text = "Voldtag hende", Points = -100, Result = "Du bliver arresteret og mister alle dine point."}
				}
			},
			new GameEvent
			{
				Scenario = " Du møder en pedofil i skoven.",
				Options = new List<EventOption> {
					new EventOption { Text = "Ignorer ham", Points = 0, Result = "Du går videre uden at interagere med den pedofil."},
					new EventOption { Text = "Rapporter ham", Points = 10, Result = "Du rapporterer den pedofil til politiet."},
					new EventOption { Text = "Forsøg at overbevise ham", Points = 5, Result = "Du forsøger at overbevise den pedofil, men det lykkes ikke."}
				}
			},
			new GameEvent
			{
				Scenario = "DU bliver jagtet af et bande",
				Options = new List<EventOption> {
					new EventOption { Text = "Løb væk", Points = 5, Result = "Du undslap banden!"},
					new EventOption { Text = "Forsøg at forhandle", Points = -10, Result = "Banden angriber dig!"},
					new EventOption { Text = "Kast penge til dem", Points = 10, Result = "Banden bliver glade og lader dig passere."}
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