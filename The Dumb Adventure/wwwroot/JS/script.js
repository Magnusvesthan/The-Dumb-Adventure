let points = 0;

const apiBaseUrl = "http://localhost:5250";

document.getElementById("newEvent").addEventListener("click", loadEvent);

function loadEvent() {
    fetch(`${apiBaseUrl}/api/event`)
        .then(res => res.json())
        .then(data => {
            const scenarioText = data.scenario ?? data.Scenario;
            const options = data.options ?? data.Options ?? [];

            document.getElementById("scenario").innerText = scenarioText;
            document.getElementById("result").innerText = "";
            const optionDiv = document.getElementById("choiceButtons");
            optionDiv.innerHTML = "";

            options.forEach(option => {
                const btn = document.createElement("button");
                btn.className = "option-btn";
                btn.innerText = option.text ?? option.Text;

                btn.onclick = () => {
                    points += option.points ?? option.Points ?? 0;
                    document.getElementById("result").innerText = option.result ?? option.Result;
                    document.getElementById("points").innerText = "Points: " + points;
                };
                optionDiv.appendChild(btn);
            });
        })
        .catch(error => {
            console.error("Kunne ikke hente event:", error);
            document.getElementById("result").innerText = "Kunne ikke hente nyt event. Start ASP.NET-serveren på port 5250.";
        });
}