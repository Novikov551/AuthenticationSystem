namespace AuthenticationSystem.Client.Services.Notification.Services
{
    public class FrontendCommunicationService
    {
        private static readonly Dictionary<string, List<string>> _connections = new();
        private static readonly object _connectionLock = new object();

        public void AddUserConnectionId(string userId, string connectionId)
        {
            lock (_connectionLock)
            {
                if (!_connections.ContainsKey(userId))
                {
                    _connections.Add(userId,
                        new List<string>()
                        {
                        connectionId
                        });
                }
                else
                {
                    _connections[userId].Add(connectionId);
                }
            }
        }

        public void DeleteUserConnectionId(string userId, string connectionId)
        {
            lock (_connectionLock)
            {
                if (!_connections.ContainsKey(userId))
                {
                    throw new Exception("Cannot delete user connection,userId not found");
                }

                _connections[userId].Remove(connectionId);

                if (_connections[userId].Count == 0)
                {
                    _connections.Remove(userId);
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            lock (_connectionLock)
            {
                if (!_connections.ContainsKey(userId))
                {
                    throw new Exception("UserId not found");
                }

                return _connections[userId];
            }
        }
    }
}
