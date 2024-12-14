using System.Collections.Generic;
using System.IO;
using LuddoGameCLI;
using Newtonsoft.Json;

    public class RunRecord
    {
        private List<Player> players;
        private List<(int rolledValue, int index)> gameHistory;
        private Dictionary<string, (int Common, int Private)> dicePositions;

        public RunRecord(Stream fileStream)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                var data = JsonConvert.DeserializeObject<(List<Player>, List<(int, int)>, Dictionary<string, (int, int)>)>(reader.ReadToEnd());
                players = data.Item1 ?? new List<Player>();
                gameHistory = data.Item2 ?? new List<(int, int)>();
                dicePositions = data.Item3 ?? new Dictionary<string, (int, int)>();
            }
        }

        public IEnumerable<Player> GetPlayers()
        {
            return players;
        }

        public IEnumerable<(int rolledValue, int index)> GetGameHistory()
        {
            return gameHistory;
        }

        public Dictionary<string, (int Common, int Private)> GetDicePositions()
        {
            return dicePositions;
        }
    }

    public class MakeRecord
    {
        private List<Player> players;
        private List<(int rolledValue, int index)> gameHistory;
        private Dictionary<string, (int Common, int Private)> dicePositions; // Use Dice ID as key for serialization.

        public MakeRecord()
        {
            players = new List<Player>();
            gameHistory = new List<(int, int)>();
            dicePositions = new Dictionary<string, (int, int)>();
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public void AddGameTurn(int rolledValue, int index)
        {
            gameHistory.Add((rolledValue, index));
        }

        // Save dice positions as a string-to-position mapping
        public void SaveDicePositions(Dictionary<Dice, (int Common, int Private)> currentPositions)
        {
            dicePositions.Clear();
            foreach (var entry in currentPositions)
            {
                dicePositions[entry.Key.ToString()] = entry.Value; // Save Dice ID as the key and position as the value
            }
        }

        public void Save(Stream fileStream)
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                var data = JsonConvert.SerializeObject((players, gameHistory, dicePositions));
                writer.Write(data);
            }
        }

        public List<Player> Players => players;
        public List<(int rolledValue, int index)> GameHistory => gameHistory;
        public Dictionary<string, (int Common, int Private)> DicePositions => dicePositions;
    }

