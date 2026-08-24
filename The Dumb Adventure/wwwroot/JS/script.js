let points = 0;

const apiBaseUrl = (() => {
    if (window.location.origin && window.location.origin !== "null") {
        return window.location.origin;
    }

    return "http://localhost:5250";
})();

const choiceMap = {
    Sten: "Saks",
    Saks: "Papir",
    Papir: "Sten"
};

function getCatChoice() {
    const choices = ["Sten", "Saks", "Papir"];
    return choices[Math.floor(Math.random() * choices.length)];
}

function resolveRockPaperScissors(playerChoice, catChoice) {
    if (playerChoice === catChoice) {
        return { pointsDelta: 0, message: `Uafgjort! Katten valgte ${catChoice}.` };
    }

    if (choiceMap[playerChoice] === catChoice) {
        return { pointsDelta: 10, message: `Du vandt! Katten valgte ${catChoice}.` };
    }

    return { pointsDelta: -10, message: `Du tabte! Katten valgte ${catChoice}.` };
}

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
                const optionText = option.text ?? option.Text;
                btn.innerText = optionText;

                btn.onclick = () => {
                    optionDiv.querySelectorAll("button").forEach(button => {
                        button.disabled = true;
                    });

                    if ((optionText === "Sten" || optionText === "Saks" || optionText === "Papir") && scenarioText.includes("sten-saks-papir")) {
                        const catChoice = getCatChoice();
                        const outcome = resolveRockPaperScissors(optionText, catChoice);
                        points += outcome.pointsDelta;
                        document.getElementById("result").innerText = outcome.message;
                        document.getElementById("points").innerText = "Points: " + points;
                        setTimeout(loadEvent, 1500);
                        return;
                    }

                    points += option.points ?? option.Points ?? 0;
                    document.getElementById("result").innerText = option.result ?? option.Result;
                    document.getElementById("points").innerText = "Points: " + points;
                    setTimeout(loadEvent, 1500);
                };
                optionDiv.appendChild(btn);
            });
        })
        .catch(error => {
            console.error("Kunne ikke hente event:", error);
            document.getElementById("result").innerText = "Kunne ikke hente nyt event. Start ASP.NET-serveren på port 5250.";
        });
}