using System;
using System.Collections.Generic;
using System.Text;
using ZooKeeperNet;

namespace ZooKeeperConsoleNet4
{
    class ZookeeperEventHandler : ZooKeeperNet.IWatcher
    {

        public void Process(WatchedEvent @event)
        {
             Program.publishEvent(@event.State.ToString());
        }

    }
}

