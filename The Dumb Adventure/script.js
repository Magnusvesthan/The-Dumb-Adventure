let points = 0;
document.getElementById("newEvent").addEventListener("click", loadEvent);

function loadEvent() {
    fetch("http://localhost:5250/api/event")
        .then(res => res.json())
        .then(data => {
            document.getElementById("scenario").innerText = data.scenario;
            document.getElementById("result").innerText = "";
            const optionDiv = document.getElementById("option");
            optionDiv.innerText = "";

            data.options.forEach(option => {
                const btn = document.createElement("button");
                btn.className = "option-btn";
                btn.innerText = option.text;

                btn.onclick = () => {
                    points += option.points
                    document.getElementById("result").innerText = option.result;
                    document.getElementById("points").innerText = "Point " + points;
                };
                optionDiv.appendChild(btn);
            });
        });
}