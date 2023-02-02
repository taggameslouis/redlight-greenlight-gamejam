namespace Quantum
{
    public unsafe class ConnectionsSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
    {
        public override void OnInit(Frame f)
        {
            f.Global->ConnectionCount = 0;
        }

        public void OnPlayerConnected(Frame f, PlayerRef player)
        {
            f.Global->ConnectionCount++;
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            f.Global->ConnectionCount--;
        }
    }
}