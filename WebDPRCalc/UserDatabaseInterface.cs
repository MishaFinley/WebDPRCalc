using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using WebDPRCalc.Models;

namespace WebDPRCalc
{
    public class UserDatabaseInterface
    {
        private static string dbConnectionString = "mongodb://london:WQFtVZbbunpJXtzl@cluster0-shard-00-00-opx3w.mongodb.net:27017/final?authSource=admin&replicaSet=Cluster0-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
        private static MongoClient dbClient = new MongoClient(dbConnectionString);
        private static IMongoDatabase database = dbClient.GetDatabase("DPR");
        private IMongoCollection<BsonDocument> users = database.GetCollection<BsonDocument>("users");

        public void createUser(User user)
        {

        }
        public User readUser(string username)
        {
            return null;
        }
        public void updateUser(User user)
        {

        }
        public void deleteUser(string username)
        {

        }
        public void createAttack(string username, Attack attack)
        {
            var user = readUser(username);
            if (!(user is null))
            {
                if (user.attacks.Count > 0)
                {
                    attack.id = user.attacks[user.attacks.Count - 1].id + 1;
                }
                user.attacks.Add(attack);
                updateUser(user);
            }
            throw new ArgumentException("User does not exist");
        }
        public Attack readAttack(string username, int attackID)
        {
            var attacks = readAttacks(username);
            if (!(attacks is null))
            {
                foreach (var attack in attacks)
                {
                    if (attack.id.Equals(attackID))
                    {
                        return attack;
                    }
                }
            }
            return null;
        }
        public List<Attack> readAttacks(string username)
        {
            var user = readUser(username);
            if (!(user is null))
            {
                return user.attacks;
            }
            throw new ArgumentException("User does not exist");
        }
        public void updateAttack(string username, Attack attack)
        {
            var user = readUser(username);
            if (!(user is null))
            {
                var attacks = user.attacks;
                if (!(attacks is null))
                {
                    for (int i = 0; i < attacks.Count; i++)
                    {
                        if (attacks[i].id.Equals(attack.id))
                        {
                            attacks[i] = attack;
                            break;
                        }
                    }
                }
                updateUser(user);
            }
            throw new ArgumentException("User does not exist");
        }
        public void deleteAttack(string username, int attackId)
        {
            var user = readUser(username);
            if (!(user is null))
            {
                var attacks = user.attacks;
                if (!(attacks is null))
                {
                    for (int i = 0; i < attacks.Count; i++)
                    {
                        if (attacks[i].id.Equals(attackId))
                        {
                            attacks.RemoveAt(i);
                            break;
                        }
                    }
                }
                updateUser(user);
            }
            throw new ArgumentException("User does not exist");
        }
    }
}
