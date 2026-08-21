let points = 0;
document.getElementById("newEvent").addEventListener("click", loadEvent);

function loadEvent() {
    fetch("http://localhost:5250/api/event")
        .then(res => {
            if (!res.ok) throw new Error(res.statusText || 'Network response was not ok');
            return res.json();
        })
        .then(data => {
            document.getElementById("scenario").innerText = data.scenario;
            document.getElementById("result").innerText = "";
            const optionButtons = document.getElementById("option-buttons");
            optionButtons.innerHTML = "";

            (data.options || []).forEach(option => {
                const btn = document.createElement("button");
                btn.type = "button";
                btn.className = "option-btn";
                btn.innerText = option.text;

                btn.addEventListener('click', () => {
                    points += Number(option.points) || 0;
                    document.getElementById("result").innerText = option.result;
                    document.getElementById("points").innerText = "Points: " + points;
                });
                optionButtons.appendChild(btn);
            });
        })
        .catch(err => {
            console.error(err);
            document.getElementById("result").innerText = "Fejl ved indlæsning af event.";
        });
}