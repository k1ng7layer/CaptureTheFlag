namespace Utils
{
    public class ConnectionGameState
    {
        public ConnectionGameState(int connectionId)
        {
            ConnectionId = connectionId;
        }
        
        public int ConnectionId { get; }
        public EConnectionState ConnectionState { get; set; }
    }
}