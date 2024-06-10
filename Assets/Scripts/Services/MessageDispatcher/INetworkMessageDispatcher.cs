using System;
using Mirror;

namespace Services.MessageDispatcher
{
    public interface INetworkMessageDispatcher
    {
        
    }

    public static class NetworkMessageDispatcherExtensions
    {
        public static void Register<T>(this INetworkMessageDispatcher messageDispatcher, Action<NetworkConnectionToClient, T> handler)
            where T : struct, NetworkMessage
        {
            NetworkServer.RegisterHandler(handler);
        }

        public static void Send<T>(this INetworkMessageDispatcher messageDispatcher, T message) where T : struct, NetworkMessage
        {
            NetworkClient.Send(message);
        }

        public static void Unregister<T>(this INetworkMessageDispatcher messageDispatcher) where T : struct, NetworkMessage
        {
            NetworkServer.UnregisterHandler<T>();
        }
    }
}