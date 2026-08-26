using Microsoft.AspNetCore.Mvc;
using SurvivalGame.Models;
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
                Scenario = "Det begynder at regne på vej hjem.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Løb hjem", Points = 5, Result = "Du kommer hurtigt hjem, men bliver våd."},
                    new EventOption { Text = "Vent i en butik", Points = 10, Result = "Regnen stopper, før du går videre."},
                    new EventOption { Text = "Gå videre", Points = -5, Result = "Du bliver gennemblødt."}
                }
            },
            new GameEvent
            {
                Scenario = "Du kan ikke finde dine nøgler.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Tjek dine lommer", Points = 5, Result = "Nøglerne ligger i din jakkelomme."},
                    new EventOption { Text = "Kig hele hjemmet igennem", Points = 10, Result = "Du finder nøglerne på køkkenbordet."},
                    new EventOption { Text = "Ring efter hjælp", Points = 0, Result = "Du kommer ind, men skal stadig finde nøglerne."}
                }
            },
            new GameEvent
            {
                Scenario = "Du står i en lang kø i supermarkedet.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Bliv i køen", Points = 5, Result = "Du får handlet færdig."},
                    new EventOption { Text = "Find en anden kasse", Points = 10, Result = "Den anden kasse går lidt hurtigere."},
                    new EventOption { Text = "Gå hjem uden at handle", Points = -5, Result = "Du sparer tid, men mangler stadig varerne."}
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
                Scenario = "En ven spørger, om du kan hjælpe med at flytte.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Hjælp med det samme", Points = 15, Result = "I bliver hurtigt færdige."},
                    new EventOption { Text = "Aftal et tidspunkt senere", Points = 10, Result = "I finder et tidspunkt, der passer."},
                    new EventOption { Text = "Sig nej tak", Points = 0, Result = "Din ven finder en anden løsning."}
                }
            },
            new GameEvent
            {
                Scenario = "Du opdager, at en opgave skal afleveres i morgen.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Gå i gang med det samme", Points = 15, Result = "Du bliver færdig i god tid."},
                    new EventOption { Text = "Lave en plan", Points = 10, Result = "Du får bedre overblik over opgaven."},
                    new EventOption { Text = "Udskyd den", Points = -10, Result = "Du får travlt senere."}
                }
            },
            new GameEvent
            {
                Scenario = "Din nabo spiller høj musik.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Spørg pænt, om der kan skrues ned", Points = 15, Result = "Naboen skruer ned for musikken."},
                    new EventOption { Text = "Bruge høretelefoner", Points = 5, Result = "Du kan koncentrere dig trods musikken."},
                    new EventOption { Text = "Ignorere det", Points = 0, Result = "Musikken stopper senere."}
                }
            },
            new GameEvent
            {
                Scenario = "Du finder en pung på fortovet.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Aflevere den til politiet", Points = 15, Result = "Ejeren kan få pungen tilbage."},
                    new EventOption { Text = "Lægge den synligt", Points = 5, Result = "Ejeren har en chance for at finde den."},
                }
            },
            new GameEvent
            {
                Scenario = "Du har en ledig eftermiddag.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Rydde op derhjemme", Points = 10, Result = "Dit værelse bliver mere ryddeligt."},
                    new EventOption { Text = "Møde en ven", Points = 15, Result = "I hygger jer sammen."},
                    new EventOption { Text = "Slappe af", Points = 5, Result = "Du får ladet op."}
                }
            },
            new GameEvent
            {
                Scenario = " Du er til en fest, og nogen tilbyder dig en drink.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Tag imod drinken", Points = 5, Result = "Du nyder festen."},
                    new EventOption { Text = "Afvis høfligt", Points = 10, Result = "Du holder dig ædru og har det sjovt."},
                    new EventOption { Text = "Lav din egen drink", Points = 15, Result = "Du imponerer dine venner med dine mixevner."}
                }
            },
            new GameEvent
            {
                Scenario = "Du sidder på dit arbejde og opdager, at du har lavet en fejl i et vigtigt dokument.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Ret fejlen med det samme", Points = 15, Result = "Du undgår problemer senere."},
                    new EventOption { Text = "Informer din chef om fejlen", Points = 10, Result = "Din chef hjælper dig med at rette fejlen."},
                    new EventOption { Text = "Ignorer fejlen og håb på det bedste", Points = -10, Result = "Fejlen bliver opdaget senere, og du får problemer."}
                }
            },
            new GameEvent
            {
                Scenario = "Du er på en vandretur og støder på en flod, der blokerer din vej.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Forsøg at krydse floden", Points = 10, Result = "Du kommer sikkert over floden."},
                    new EventOption { Text = "Find en bro eller en anden vej", Points = 15, Result = "Du finder en sikker rute rundt om floden."},
                    new EventOption { Text = "Vend om og gå tilbage", Points = 5, Result = "Du undgår risikoen, men mister tid."}
                }
            },
            new GameEvent
            {
                Scenario = "Du er på en campingtur og opdager, at du har glemt at medbringe mad.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Gå tilbage til bilen og hent mad", Points = 10, Result = "Du får mad og kan fortsætte turen."},
                    new EventOption { Text = "Forsøg at finde spiselige planter i naturen", Points = 5, Result = "Du finder nogle bær og nødder, men det er ikke nok."},
                    new EventOption { Text = "Lav en nødplan og vent på hjælp", Points = 15, Result = "Du får hjælp fra andre campister, der deler deres mad med dig."}
                }
            },
            new GameEvent
            {
                Scenario = "Du er på en cykeltur og punkterer din cykel.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Forsøg at reparere cyklen selv", Points = 10, Result = "Du får cyklen til at køre igen."},
                    new EventOption { Text = "Ring efter hjælp", Points = 15, Result = "En ven kommer og hjælper dig med at reparere cyklen."},
                    new EventOption { Text = "Gå hjem med cyklen", Points = 5, Result = "Du mister tid, men kommer sikkert hjem."}
                }
            },
            new GameEvent
            {
                Scenario = "Du møder en pige i skoven, der græder.",
                Options = new List<EventOption> {
                    new EventOption { Text = "Spørg hende, hvad der er galt", Points = 10, Result = "Hun fortæller dig, at hun er faret vild."},
                    new EventOption { Text = "Tilbyd at hjælpe hende med at finde vej", Points = 15, Result = "I finder sammen tilbage til stien."},
                    new EventOption { Text = "Ignorer hende og gå videre", Points = -5, Result = "Hun bliver mere fortvivlet og du føler dig skyldig."},
                }
            }


        };
        [HttpGet]
        public IActionResult GetRandomEvent([FromQuery] int level = 1)
        {
            level = Math.Max(level, 1);
            var maximumPointsDifference = level * 10;
            var availableEvents = events
                .Where(gameEvent => gameEvent.Options.Max(option => Math.Abs(option.Points)) <= maximumPointsDifference)
                .ToList();

            if (availableEvents.Count == 0)
            {
                availableEvents = events;
            }

            var e = availableEvents[rnd.Next(availableEvents.Count)];
            return Ok(e);
        }
    }
}