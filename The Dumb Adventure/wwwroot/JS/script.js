let points = 0;
let highestScore = 0;
let lives = 3;
let gameOver = false;
let eventsSurvived = 0;
let xp = 0;
let combo = 0;
let currentRoundLog = [];

const apiBaseUrl = "http://localhost:5250";
const leaderboardKey = "theDumbAdventureLeaderboard";

const newEventButton = document.getElementById("newEvent");
const restartButton = document.getElementById("restart");
const optionDiv = document.getElementById("choiceButtons");
const scenarioElement = document.getElementById("scenario");
const resultElement = document.getElementById("result");
const pointsElement = document.getElementById("points");
const livesElement = document.getElementById("lives");
const titleElement = document.getElementById("title");
const xpElement = document.getElementById("xp");
const levelElement = document.getElementById("level");
const comboElement = document.getElementById("combo");
const leaderboardLists = {
    events: document.getElementById("eventsScoreList"),
    points: document.getElementById("pointsScoreList"),
    xp: document.getElementById("xpScoreList")
};
const logList = document.getElementById("logList");
const playerNameInput = document.getElementById("playerName");

function getScores() {
    try {
        return JSON.parse(localStorage.getItem(leaderboardKey)) ?? [];
    } catch {
        return [];
    }
}

function renderLeaderboard() {
    const scores = getScores();
    const rankings = [
        [leaderboardLists.events, (score) => score.events ?? 0, "events"],
        [leaderboardLists.points, (score) => score.points ?? 0, "points"],
        [leaderboardLists.xp, (score) => score.xp ?? 0, "XP"]
    ];

    rankings.forEach(([list, value, label]) => {
        list.innerHTML = "";
        [...scores]
            .sort((first, second) => value(second) - value(first))
            .slice(0, 10)
            .forEach((score) => {
                const item = document.createElement("li");
                const nameButton = document.createElement("button");
                const titleText = document.createElement("span");
                const scoreText = document.createElement("span");

                nameButton.className = "score-name";
                nameButton.type = "button";
                nameButton.innerText = score.name;
                nameButton.addEventListener("click", () => renderSurvivalLog(score.log ?? []));
                titleText.className = "score-title";
                titleText.innerText = ` (${getTitleForXp(score.xp ?? 0)})`;
                scoreText.innerText = `: ${value(score)} ${label}`;

                item.append(nameButton, titleText, scoreText);
                list.appendChild(item);
            });
    });
}

function getPlayerName() {
    const name = playerNameInput.value.trim();

    if (!name) {
        playerNameInput.setCustomValidity("Indtast et spillernavn før du starter.");
        playerNameInput.reportValidity();
        playerNameInput.focus();
        return null;
    }

    playerNameInput.setCustomValidity("");
    return name;
}

function findScore(scores, name) {
    const normalizedName = name.trim().toLowerCase();
    return scores.find((score) =>
        score.name?.trim().toLowerCase() === normalizedName
    );
}

function getSavedScore(name) {
    return findScore(getScores(), name);
}

function loadSavedPlayerData() {
    if (points !== 0 || xp !== 0 || eventsSurvived !== 0 || gameOver) {
        return;
    }

    const name = playerNameInput.value.trim();
    const savedScore = getSavedScore(name);
    if (!savedScore) {
        return;
    }

    eventsSurvived = savedScore.events ?? 0;
    xp = savedScore.xp ?? 0;
    updateGameStats();
    resultElement.innerText = `Velkommen tilbage, ${savedScore.name}! Din gemte titel, level og XP er indlæst.`;
}

function saveScore() {
    const name = getPlayerName();
    if (!name) {
        return;
    }
    const scores = getScores();
    const newScore = { name, points, events: eventsSurvived, xp, log: [...currentRoundLog] };
    const existingScore = findScore(scores, name);

    if (!existingScore) {
        scores.push(newScore);
    } else {
        existingScore.points = newScore.points;
        existingScore.events = newScore.events;
        existingScore.xp = newScore.xp;
        existingScore.log = [...(existingScore.log ?? []), ...newScore.log];
    }

    localStorage.setItem(leaderboardKey, JSON.stringify(scores));
    renderLeaderboard();
}

function renderSurvivalLog(entries) {
    logList.innerHTML = "";
    entries.forEach((entry) => {
        const logEntry = document.createElement("li");
        const challenge = document.createElement("span");
        const details = document.createElement("span");

        challenge.className = "log-challenge";
        challenge.innerText = entry.scenario;
        details.className = "log-details";
        details.innerText = `Valg: ${entry.choice} | ${entry.result} (${entry.pointsDelta >= 0 ? "+" : ""}${entry.pointsDelta} points)`;

        logEntry.append(challenge, details);
        logList.appendChild(logEntry);
    });
}

function addSurvivalLogEntry(scenario, choice, result, pointsDelta) {
    currentRoundLog.push({ scenario, choice, result, pointsDelta });
    renderSurvivalLog(currentRoundLog);
}

function endGame() {
    gameOver = true;
    optionDiv.innerHTML = "";
    newEventButton.disabled = true;
    restartButton.hidden = false;
    resultElement.innerText = `${getEndingMessage()} Du fik ${highestScore} points og overlevede ${eventsSurvived} events. Scoren er gemt i highscores.`;
    saveScore();
}

function getEndingMessage() {
    if (highestScore >= 100) {
        return "Du blev en legende og overlevede næsten alt!";
    }
    if (eventsSurvived >= 10) {
        return "Du overlevede længe nok til at blive en ægte survivor!";
    }
    return "Eventyret sluttede, men du kæmpede tappert.";
}

function updateLives() {
    livesElement.innerText = "Liv: " + lives;
}

function getTitleForXp(scoreXp) {
    if (scoreXp >= 5000) {
        return "Overlevelseslegende";
    }
    if (scoreXp >= 3500) {
        return "Udødelig overlever";
    }
    if (scoreXp >= 2000) {
        return "Survival-mester";
    }
    if (scoreXp >= 1000) {
        return "Eventyrer";
    }
    if (scoreXp >= 500) {
        return "Erfaren overlever";
    }
    if (scoreXp >= 250) {
        return "Overlever";
    }
    return "Nybegynder";
}

function getTitle() {
    return getTitleForXp(xp);
}

function updateTitle() {
    titleElement.innerText = "Titel: " + getTitle();
}

function updateXp() {
    xpElement.innerText = "XP: " + xp;
}

function getLevel() {
    return Math.floor(xp / 100) + 1;
}

function updateLevel() {
    const nextLevelXp = getLevel() * 100;
    levelElement.innerText = `Level: ${getLevel()} (${xp}/${nextLevelXp} XP)`;
}

function updateCombo() {
    comboElement.innerText = "Combo: " + combo;
}

function updateGameStats() {
    pointsElement.innerText = "Points: " + points;
    updateLives();
    updateXp();
    updateLevel();
    updateTitle();
    updateCombo();
}

function addPoints(pointsDelta) {
    points += pointsDelta;
    if (pointsDelta > 0) {
        xp += 10;
    }
    highestScore = Math.max(highestScore, points);
    pointsElement.innerText = "Points: " + points;
    updateXp();
    updateLevel();
    updateTitle();

    if (pointsDelta > 0) {
        combo += 1;
        updateCombo();
        if (combo % 3 === 0) {
            points += 10;
            highestScore = Math.max(highestScore, points);
            pointsElement.innerText = "Points: " + points;
            updateTitle();
            resultElement.innerText += ` Combo x${combo}! +10 bonus points.`;
            addSurvivalLogEntry("Combo-bonus", `Lav ${combo} gode valg i træk`, "Du fik 10 ekstra points", 10);
        }
    } else if (pointsDelta < 0) {
        combo = 0;
        updateCombo();
    }

    if (pointsDelta < 0 && Math.random() < 0.5) {
        lives -= 1;
        updateLives();
        resultElement.innerText += " Du mistede et liv!";
    }

    if (lives <= 0) {
        endGame();
        return true;
    }
    return false;
}

function restartGame() {
    points = 0;
    highestScore = 0;
    lives = 3;
    eventsSurvived = 0;
    xp = 0;
    combo = 0;
    currentRoundLog = [];
    gameOver = false;
    updateGameStats();
    restartButton.hidden = true;
    newEventButton.disabled = false;
    loadSavedPlayerData();
    loadEvent();
}

function giveRandomBonus() {
    if (Math.random() < 0.2) {
        points += 20;
        highestScore = Math.max(highestScore, points);
        pointsElement.innerText = "Points: " + points;
        resultElement.innerText += " Du fandt en bonus på 20 points!";
        addSurvivalLogEntry("Tilfældig bonus", "Samlede bonus op", "Du fik 20 ekstra points", 20);
    }

    if (Math.random() < 0.2) {
        xp += 20;
        updateXp();
        updateTitle();
        resultElement.innerText += " Du fandt 20 ekstra XP!";
        addSurvivalLogEntry("Tilfældig bonus", "Samlede bonus op", "Du fik 20 ekstra XP", 0);
    }

    if (lives < 3 && Math.random() < 0.05) {
        lives += 1;
        updateLives();
        resultElement.innerText += " Du fandt et ekstra liv!";
        addSurvivalLogEntry("Tilfældig bonus", "Samlede bonus op", "Du fik et ekstra liv", 0);
    }
}

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

newEventButton.addEventListener("click", loadEvent);
restartButton.addEventListener("click", restartGame);
playerNameInput.addEventListener("change", loadSavedPlayerData);
renderLeaderboard();

function loadEvent() {
    if (gameOver) {
        return;
    }

    if (!getPlayerName()) {
        return;
    }

    fetch(`${apiBaseUrl}/api/event?level=${getLevel()}`)
        .then(res => res.json())
        .then(data => {
            const scenarioText = data.scenario ?? data.Scenario;
            const options = data.options ?? data.Options ?? [];

            scenarioElement.innerText = scenarioText;
            resultElement.innerText = "";
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
                        resultElement.innerText = outcome.message;
                        addSurvivalLogEntry(scenarioText, optionText, outcome.message, outcome.pointsDelta);
                        eventsSurvived += 1;
                        if (addPoints(outcome.pointsDelta)) {
                            return;
                        }
                        giveRandomBonus();
                        setTimeout(loadEvent, 1500);
                        return;
                    }

                    const optionResult = option.result ?? option.Result;
                    const pointsDelta = option.points ?? option.Points ?? 0;
                    resultElement.innerText = optionResult;
                    addSurvivalLogEntry(scenarioText, optionText, optionResult, pointsDelta);
                    eventsSurvived += 1;
                    const hasDied = addPoints(pointsDelta);
                    if (hasDied) {
                        return;
                    }
                    giveRandomBonus();
                    setTimeout(loadEvent, 1500);
                };
                optionDiv.appendChild(btn);
            });
        })
        .catch(error => {
            console.error("Kunne ikke hente event:", error);
            resultElement.innerText = "Kunne ikke hente nyt event. Start ASP.NET-serveren på port 5250.";
        });
}