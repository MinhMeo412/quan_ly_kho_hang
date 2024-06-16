using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class TokenTable
    {
        public List<Token>? Tokens { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawTokens = Procedure.ExecuteReader(connectionString, "read_token", inParameters);

            List<Token> tokens = new List<Token>();
            foreach (List<object?> rawToken in rawTokens)
            {
                Token tokenObj = new Token(
                    (string)(rawToken[0] ?? ""),
                    (int)(rawToken[1] ?? 0),
                    (DateTime)(rawToken[2] ?? DateTime.Now)
                );
                tokens.Add(tokenObj);
            }

            this.Tokens = tokens;
        }

        public void Add(string connectionString, string token, string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_token_uuid", tokenUUID},
                {"input_user_id", userID},
                {"input_token_last_activity_timestamp", tokenLastActivityTimeStamp}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_token", inParameters);

            Token tokenObj = new Token(tokenUUID, userID, tokenLastActivityTimeStamp);

            this.Tokens ??= new List<Token>();
            this.Tokens.Add(tokenObj);
        }

        public void Update(string connectionString, string token, string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_token_uuid", tokenUUID},
                {"input_user_id", userID},
                {"input_token_last_activity_timestamp", tokenLastActivityTimeStamp}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_token", inParameters);

            var tokenObj = this.Tokens?.FirstOrDefault(t => t.TokenUUID == tokenUUID);
            if (tokenObj != null)
            {
                tokenObj.UserID = userID;
                tokenObj.TokenLastActivityTimeStamp = tokenLastActivityTimeStamp;
            }
        }

        public void Delete(string connectionString, string token, string tokenUUID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_token_uuid", tokenUUID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_token", inParameters);

            var tokenObj = this.Tokens?.FirstOrDefault(t => t.TokenUUID == tokenUUID);
            if (tokenObj != null)
            {
                this.Tokens ??= new List<Token>();
                this.Tokens.Remove(tokenObj);
            }
        }
    }
}
