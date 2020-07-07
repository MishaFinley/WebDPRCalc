using MongoDB.Bson;
using MongoDB.Driver;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDPRCalc.Models;

namespace WebDPRCalc
{
    public class UserDatabaseInterface
    {
        private static string dbConnectionString = "mongodb://london:WQFtVZbbunpJXtzl@cluster0-shard-00-00-opx3w.mongodb.net:27017/final?authSource=admin&replicaSet=Cluster0-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
        private static MongoClient dbClient = new MongoClient(dbConnectionString);
        private static IMongoDatabase database = dbClient.GetDatabase("DPR");
        private IMongoCollection<BsonDocument> users = database.GetCollection<BsonDocument>("users");
        private BsonDocument ToBson(object o)
        {
            return BsonDocument.Parse(new JavaScriptSerializer().Serialize(o));
        }
        public async void createUser(User user)
        {
            await users.InsertOneAsync(ToBson(user));
        }
        public async Task<User> readUser(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var document = await users.Find(filter).FirstOrDefaultAsync();
            if (document is null)
                return null;
            document.Remove("_id");
            var json = document.ToJson();
            return new JavaScriptSerializer().Deserialize<User>(json);
        }
        public void updateUser(User user)
        {
            deleteUser(user.username);
            createUser(user);
        }
        public async void deleteUser(string username)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            await users.DeleteManyAsync(filter);
        }
        public async void createAttack(string username, Attack attack)
        {
            var user = await readUser(username);
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
        public async Task<Attack> readAttack(string username, int attackID)
        {
            var attacks = await readAttacks(username);
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
        public async Task<List<Attack>> readAttacks(string username)
        {
            var user = await readUser(username);
            if (!(user is null))
            {
                return user.attacks;
            }
            throw new ArgumentException("User does not exist");
        }
        public async void updateAttack(string username, Attack attack)
        {
            var user = await readUser(username);
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
        public async void deleteAttack(string username, int attackId)
        {
            var user = await readUser(username);
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
