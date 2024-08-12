using System;
using System.Collections.Generic;
using _root.Script.Network;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace _root.Script.Client
{
    [Serializable]
    public class GameCards
    {
        public List<GameCard> cards;
    }

    [Serializable]
    public class GameCard
    {
        public string id;
        public string defaultCardType;
        public int limit;
        public bool dayTime;
    }

    [Serializable]
    public class GameStudentCards
    {
        public List<GameStudentCard> cards;
    }

    [Serializable]
    public class GameStudentCard
    {
        public string id;
        public string cardType;
        public List<MajorType> groups;
        public List<Tiers> tiers;
        public List<Passives> passives;
        public double fatigue;
    }

    public enum StudentState
    {
        BadHealth,
        Panic,
        Despair,
        Cooperation,
        WakeUp,
        Concentrate,
        SuperConcentrate,

        Passion // 열기
    }

    [Serializable]
    public class StudentData
    {
        public string id;
        public double fatigue;
        public Dictionary<StudentState, int> data;
    }

    [Serializable]
    public class RawData
    {
        public List<string> self;
        public List<string> other;
        public bool turn;
    }

    [Serializable]
    public class StudentDatas
    {
        public List<StudentData> students;
    }

    public enum Modifier
    {
        Client,
        Student,
        Deck,
        HeldCard,
        Time,
        FieldCard,
        Project,
        Sleep,
        DateData,
        ProjectAdditionData,
        Students,
        Turn
    }

    [CanBeNull]
    [Serializable]
    public class UpdatedData
    {
        [CanBeNull] public string client;
        [CanBeNull] public GameStudentCards student;
        [CanBeNull] public GameCards heldCards;
        [CanBeNull] public GameCards fieldCards;
        [CanBeNull] public StudentDatas students;
        [CanBeNull] public Dictionary<string, int> dateData;
        public int? deckSize;
        [CanBeNull] public Dictionary<MajorType, List<int>> issues;
        public bool? isTurn;
        [CanBeNull] public Dictionary<MajorType, Dictionary<string, List<int>>> projectAdditionData;
        [CanBeNull] public Dictionary<MajorType, int> projects;
        public bool? sleep;
        public int? time;

        public static UpdatedData ConvertUpdatedData(List<string> data)
        {
            if (data == null) return null;
            var updatedData = new UpdatedData();

            var set = data[0].Split(",");

            for (var i = 0; i < set.Length; i++)
            {
                if (!Enum.TryParse<Modifier>(set[i], out var result)) continue;
                var s = data[i + 1].Replace(@"\", "");
                try
                {
                    switch (result)
                    {
                        case Modifier.Client:
                            updatedData.client = s;
                            break;
                        case Modifier.Student:
                            updatedData.student = JsonConvert.DeserializeObject<GameStudentCards>(s);
                            break;
                        case Modifier.Deck:
                            updatedData.deckSize = int.Parse(s);
                            break;
                        case Modifier.HeldCard:
                            updatedData.heldCards = JsonConvert.DeserializeObject<GameCards>(s);
                            break;
                        case Modifier.Time:
                            updatedData.time = int.Parse(s);
                            break;
                        case Modifier.FieldCard:
                            updatedData.fieldCards = JsonConvert.DeserializeObject<GameCards>(s);
                            break;
                        case Modifier.Project:
                            updatedData.projects = JsonConvert.DeserializeObject<Dictionary<MajorType, int>>(s);
                            break;
                        case Modifier.Sleep:
                            updatedData.sleep = bool.Parse(s);
                            break;
                        case Modifier.DateData:
                            updatedData.dateData = JsonConvert.DeserializeObject<Dictionary<string, int>>(s);
                            break;
                        case Modifier.ProjectAdditionData:
                            updatedData.projectAdditionData = JsonConvert
                                .DeserializeObject<Dictionary<MajorType, Dictionary<string, List<int>>>>(s);
                            break;
                        case Modifier.Students:
                            updatedData.students = JsonConvert.DeserializeObject<StudentDatas>(s);
                            break;
                        case Modifier.Turn:
                            updatedData.isTurn = bool.Parse(s);
                            break;
                        default:
                            Debug.LogError(result);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    // ignored
                }
            }

            return updatedData;
        }
    }
}