const express = require("express");
const mongoose = require("mongoose");
const bodyParser = require("body-parser");
const fs = require("fs");
const cors = require("cors");
const { nanoid } = require("nanoid");
const Player = require("./models/Player");

const app = express();
app.use(express.json());
app.use(cors()); //Allows us to make requiests from our game.
app.use(bodyParser.json());

const FILE_PATH = "player.json";

//Connection for MongoDB
mongoose.connect("mongodb+srv://jcwolf:passpasspass@cluster0.kylzq.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");

const db = mongoose.connection;

db.on("error", console.error.bind(console, "MongoDB connection error"));
db.once("open", ()=>{
    console.log("Connected to MongoDB Database");
});


//API endpoint for player.json;

// app.get("/player", (req,res)=>{
//     fs.readFile(FILE_PATH, "utf-8",(err, data)=>{
//         if(err){
//             return res.status(500).json({error:"Unable to fetch data"});
//         }
//         res.json(JSON.parse(data));
//         console.log(`Responded with: ${data}`);
//     })
// });

app.get("/player", async (req,res)=>
{
    try
    {
        const players = await Player.find();
        if(!players)
        {
            return res.status(404).json({error:"Player not found"})
        }
        res.json(players);
        console.log(players);
    }
    catch(error)
    {
        res.status(500).json({error:"Failed to retrieve player"})
    }
});

app.get("/player/:playerid", async(req,res)=>{
    try{

        const player = await Player.findOne({playerid:req.params.playerid});

        if(!player){
            return res.status(404).json({error:"Player not found"})
        }
        res.json(player);

    }
    catch(error)
    {
        res.status(500).json({error:"Failed to retrieve player"})
    }
});

app.post("/sentdata", (req,res)=>{
    const newPlayerData = req.body;

    console.log(JSON.stringify(newPlayerData,null,5));

    res.json({message:"Player Data recieved"});
});



app.post("/sentdatatodb", async (req,res)=>{
    try{
        const newPlayerData = req.body;

        console.log(JSON.stringify(newPlayerData,null,5));

        const newPlayer = new Player({
            playerid:nanoid(8),
            screenName:newPlayerData.screenName,
            firstName:newPlayerData.firstName,
            lastName:newPlayerData.lastName,
            numberOfGamesPlayed:newPlayerData.numberOfGamesPlayed,
            score:newPlayerData.score

        });
        //save to database
        await newPlayer.save();
        res.json({message:"Player Added Successfully",playerid:newPlayer.playerid, firstName:newPlayer.firstName});
    }
    catch(error){
        res.status(500).json({error:"Failed to add player"})
    }
    
    
});

//update player
// Example using Express.js (Node.js)
app.put('/updatePlayer', (req, res) => {
    const { screenName, firstName, lastName, numberOfGamesPlayed, score } = req.body;

    // Check if the player exists
    Player.findOne({ screenName }).then((player) => {
        if (!player) {
            return res.status(404).json({ message: "Player not found" });
        }

        // Update the player's data
        player.firstName = firstName;
        player.lastName = lastName;
        player.numberOfGamesPlayed = numberOfGamesPlayed;
        player.score = score;

        player.save().then(() => {
            res.status(200).json({ message: "Player updated successfully!" });
        }).catch((err) => {
            res.status(500).json({ message: "Error updating player", error: err });
        });
    });
});

app.put("/updatePlayer/:id", async (req, res) => {
    try {
        const { screenName, firstName, lastName, numberOfGamesPlayed, score } = req.body;
        const updatedPlayer = await Player.findByIdAndUpdate(
            req.params.id,
            { screenName, firstName, lastName, numberOfGamesPlayed, score },
            { new: true } // Return updated player
        );

        if (!updatedPlayer) {
            return res.status(404).json({ message: "Player not found" });
        }

        res.status(200).json({ message: "Player updated successfully", updatedPlayer });
    } catch (error) {
        res.status(500).json({ message: "Error updating player", error: error.message });
    }
});

app.get("/topTen", async (req, res) => {
    try {
        let topPlayers = await Player.find().lean(); // Fetch all players as plain objects

        // Convert `score` to a number for sorting
        topPlayers.sort((a, b) => Number(b.score) - Number(a.score));

        // Get top 10 players
        topPlayers = topPlayers.slice(0, 10);

        console.log("Top 10 Players:", topPlayers.map(p => ({ name: p.screenName, score: p.score })));

        res.status(200).json(topPlayers);
    } catch (error) {
        console.error("Error fetching top players:", error);
        res.status(500).json({ message: "Error fetching top players", error: error.message });
    }
});

app.get('/findPlayer', (req, res) => {
    const { screenName } = req.query;  // Use query parameters for finding

    // Use Mongoose's findOne method to find a player by screenName
    Player.findOne({ screenName })
        .then((player) => {
            if (!player) {
                return res.status(404).json({ message: "Player not found" });
            }
            res.status(200).json(player);  // Send the found player as response
        })
        .catch((err) => {
            console.error("Error finding player:", err);
            res.status(500).json({ message: "Error finding player", error: err });
        });
});

app.delete('/deletePlayer', async (req, res) => {
    const { screenName } = req.query; // Get screenName from query parameter

    if (!screenName) {
        return res.status(400).json({ message: "screenName is required" });
    }

    try {
        // Find and delete the player by screenName
        const player = await Player.findOneAndDelete({ screenName });

        if (!player) {
            return res.status(404).json({ message: `Player with screenName ${screenName} not found` });
        }

        return res.status(200).json({ message: `Player with screenName ${screenName} deleted successfully` });
    } catch (error) {
        console.error(error);
        return res.status(500).json({ message: "Error deleting player", error: error.message });
    }
});

app.delete('/deletePlayer/:id', async (req, res) => {
    try {
        const player = await Player.findByIdAndDelete(req.params.id);
        if (!player) {
            return res.status(404).json({ message: "Player not found" });
        }
        res.status(200).json({ message: "Player deleted successfully" });
    } catch (error) {
        res.status(500).json({ message: "Error deleting player", error: error.message });
    }
});


app.delete('/deletePlayer/:screenName', async (req, res) => {
    const { screenName } = req.params; // Get screenName from URL path

    try {
        const player = await Player.findOneAndDelete({ screenName });

        if (!player) {
            return res.status(404).json({ message: `Player with screenName ${screenName} not found` });
        }

        return res.status(200).json({ message: `Player with screenName ${screenName} deleted successfully` });
    } catch (error) {
        console.error(error);
        return res.status(500).json({ message: "Error deleting player", error: error.message });
    }
});



app.listen(3000, ()=>{
    console.log("Running on port 3000");
})