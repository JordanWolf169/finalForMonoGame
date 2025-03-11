const mongoose = require("mongoose");

const playersSchema = new mongoose.Schema({
    playerid:{ type: String, unique:true},
    screenName:String,
    firstName:String,
    lastName:String,
    numberOfGamesPlayed:String,
    score:String
});

const Players = mongoose.model("Players", playersSchema, "players");

module.exports = Players