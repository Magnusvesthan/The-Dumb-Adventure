let points = 0;
document.getElementById("newEvent").addEventListener("click", loadEvent);

function loadEvent() {
    fetch("http://localhost:5500/api/event")
        .then(res => res.json())
        .then(data => {
            document.getElementById("scenario").innerText = data.Scenario;
            document.getElementById("result").innerText = "";
            const optionDiv = document.getElementById("options");
            optionDiv.innerText = "";

            data.Options.forEach(option => {
                const btn = document.createElement("button");
                btn.className = "option-btn";
                btn.innerText = option.Text;

                btn.onclick = () => {
                    points += option.Points
                    document.getElementById("result").innerText = option.Result;
                    document.getElementById("points").innerText = "Point " + points;
                };
                optionDiv.appendChild(btn);
            });
        });
}