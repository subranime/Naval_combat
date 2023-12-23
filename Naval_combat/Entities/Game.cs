using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naval_combat.Entities
{
    public class Game
    {
        public Player Player { get; set; }
        public Field Field { get; set; }
        public TorpedoLauncher TorpedoLauncher { get; set; }
        public string GameState { get; set; }

        // Пустой конструктор для десериализации JSON
        public Game()
        {
        }

        public Game(string nickname)
        {
            // Инициализация по умолчанию, можете дополнить по необходимости
            Player = new Player(nickname);
            Field = new Field();
            TorpedoLauncher = new TorpedoLauncher(0, 0); // Инициализация торпедного установщика
            GameState = "NotStarted"; // Начальное состояние
        }

        // Конструктор с параметрами
        public Game(string playerNickname, Field gameField, TorpedoLauncher launcher, string gameState)
        {
            Player = new Player(playerNickname);
            Field = gameField;
            TorpedoLauncher = launcher;
            GameState = gameState;
        }

        // Дополнительные методы для обновления данных и выполнения действий в игре
        public void UpdateGameState(string newState)
        {
            GameState = newState;
        }

        public string ToJson()
        {
            // Преобразование объекта Game в JSON
            return JsonConvert.SerializeObject(this);
        }

        public void FromJson(string json)
        {
            // Преобразование JSON в объект Game
            Game game = JsonConvert.DeserializeObject<Game>(json);

            // Копирование данных из преобразованного объекта в текущий объект
            Player = game.Player;
            Field = game.Field;
            TorpedoLauncher = game.TorpedoLauncher;
            GameState = game.GameState;
        }
    }
}
