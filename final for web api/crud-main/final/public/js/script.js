const playersContainer = document.getElementById("players-container");


document.addEventListener("click", async (event) => {
    if (event.target.classList.contains("delete-btn") ) 
    {
        const playerId = event.target.getAttribute("data-id");
        try 
        {
            const response = await fetch(`http://localhost:3000/deletePlayer/${playerId}`, {
                method: "DELETE"
            });
            
            if (!response.ok) throw new Error("Failed to delete player");

            fetchPlayers(); // Refresh the player list
        } catch (err) 
        {
            console.error("Error deleting player:", err);
        }
    }
});

document.addEventListener("click", async (event) => {
    if (event.target.classList.contains("update-btn")) {
        const playerId = event.target.getAttribute("data-id");

        // Get updated values from input fields
        const screenName = prompt("Enter new screen name:");
        const firstName = prompt("Enter new first name:");
        const lastName = prompt("Enter new last name:");
        const numberOfGamesPlayed = prompt("Enter updated games played:");
        const score = prompt("Enter new score:");

        if (!screenName || !firstName || !lastName || !numberOfGamesPlayed || !score) {
            alert("All fields are required!");
            return;
        }

        try {
            const response = await fetch(`http://localhost:3000/updatePlayer/${playerId}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ screenName, firstName, lastName, numberOfGamesPlayed, score })
            });

            if (!response.ok) throw new Error("Failed to update player");

            alert("Player updated successfully!");
            fetchPlayers(); // Refresh player list
        } catch (err) {
            console.error("Error updating player:", err);
        }
    }
});

const fetchTopPlayers = async () => {
    try {
        const response = await fetch("http://localhost:3000/topTen");
        if (!response.ok) {
            throw new Error("Failed to fetch top players");
        }

        const players = await response.json();
        console.log("Fetched Players:", players); // Debugging log

        const topPlayersContainer = document.getElementById("top-players-container");
        topPlayersContainer.innerHTML = "<h2>Top 10 Players</h2>";

        players.forEach((player, index) => {
            const playerDiv = document.createElement("div");
            playerDiv.className = "player";
            playerDiv.innerHTML = `
                <p><strong>#${index + 1}</strong></p>
                <p>Screen Name: ${player.screenName}</p>
                <p>Score: ${player.score}</p>
            `;
            topPlayersContainer.appendChild(playerDiv);
        });

    } catch (error) {
        console.error("Error fetching top players:", error);
        document.getElementById("top-players-container").innerHTML = "<p style='color:red'>Failed to fetch top players</p>";
    }
};

// Call the function when the page loads
document.addEventListener("DOMContentLoaded", fetchTopPlayers);


const fetchPlayers = async () => {
    try {
        const response = await fetch("http://localhost:3000/player");
        if (!response.ok) {
            throw new Error(`Failed to get players. Status: ${response.status}`);
        }

        const players = await response.json();
        console.log(players);

        playersContainer.innerHTML = "";

        players.forEach((player) => {
            const playerDiv = document.createElement("div");
            playerDiv.className = "player";
            playerDiv.innerHTML = `
                <p>Screen Name: ${player.screenName}</p>
                <p>First Name: ${player.firstName}</p>
                <p>Last Name: ${player.lastName}</p>
                <p>Number of Games Played: ${player.numberOfGamesPlayed}</p>
                <p>Score: ${player.score}</p>
                <button class="delete-btn" data-id="${player._id}">Delete</button>
                <button class="update-btn" data-id="${player._id}">Update</button>
            `;
            playersContainer.appendChild(playerDiv);
        });
        

    } catch (error) {
        console.error("Error:", error);
        playersContainer.innerHTML = "<p style='color:red'>Failed to get players</p>";
    }
};

fetchPlayers();
