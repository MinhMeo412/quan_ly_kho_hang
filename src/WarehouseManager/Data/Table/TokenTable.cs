using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class TokenTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<Token>? _tokens;
        public List<Token>? Tokens
        {
            get
            {
                this.Load();
                return this._tokens;
            }
            private set
            {
                this._tokens = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawTokens = Procedure.ExecuteReader(this.ConnectionString, "read_token", inParameters);

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

        public void Add(string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_token_uuid", tokenUUID},
                {"input_user_id", userID},
                {"input_token_last_activity_timestamp", tokenLastActivityTimeStamp}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_token", inParameters);
        }

        public void Update(string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_token_uuid", tokenUUID},
                {"input_user_id", userID},
                {"input_token_last_activity_timestamp", tokenLastActivityTimeStamp}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_token", inParameters);
        }

        public void Delete(string tokenUUID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_token_uuid", tokenUUID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_token", inParameters);
        }
    }
}
