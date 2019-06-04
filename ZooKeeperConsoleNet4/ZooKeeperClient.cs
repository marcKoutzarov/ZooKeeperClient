using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using ZooKeeperNet;

namespace ZooKeeperConsoleNet4
{
    public class ZooKeeperClient
    {
        private readonly ZookeeperEventHandler E = new ZookeeperEventHandler();
        private ZooKeeper zk;

        public ZooKeeperClient()
        {
            Connect();
        }

        public void Connect()
        {
            var s = GetState();
            if  (s != ZooKeeper.States.CONNECTED || s != ZooKeeper.States.CONNECTING)
            {
                zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(1, 1, 1), E);
            }
        }

        public void Close()
        {
           zk.Dispose();
        }

        private ZooKeeper.States GetState()
        {
            if (zk == null)
            {
                return ZooKeeper.States.NOT_CONNECTED;
            }

            return zk.State;
        }

        public List<string> GetChildren(string Path)
        {
            List<string> result = new List<string>();
            IEnumerable<string> children;

            children = zk.GetChildren(Path, false);
            if (Path == "/")
            {
                foreach (string item in children)
                {
                    if (!isZookeeperDefaultNode(item.Trim()))
                    {
                        result.Add(item);
                    };
                }
            }
            else
            {
                foreach (string item in children)
                {                  
                  result.Add(Path + "/" + item);
                }
            }

            return result;
        }

        public int ChildrenCount(string Path)
        {
           return GetChildren(Path).Count;
        }
         
        public string DataGet(string Path)
        {
            var stat = new Stat();

            var byteArray = zk.GetData(Path, false, stat);

            string result = System.Text.Encoding.UTF8.GetString(byteArray);

            return result;
        }

        public bool DataCreate(string Path, string data)
        {

            CreateMode cm = CreateMode.Persistent;

            ACL auth = new ACL();

            List<ACL> authList= new List<ACL>();

            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(data);

            zk.Create(Path, utf8Bytes, authList, cm);

            return true;
        }

        public bool DataEdit(string Path, string data)
        {
            int version = 1;

            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(data);

            zk.SetData(Path, utf8Bytes, version);

            return true;
        }

        public bool DataDelete(string Path)
        {
            int version = 1;

            zk.Delete(Path, version);

            return true;
        }

        private bool isZookeeperDefaultNode(string n)
        {
            List<string> DefaultNodes =  new List<string>() { "cluster", "controller_epoch", "brokers", "zookeeper", "admin", "isr_change_notification", "consumers", "log_dir_event_notification", "latest_producer_id_block", "config" };

            foreach(string item in DefaultNodes)
            {
                if (item == n) { return true; }
            }

            return false;
        }

    }

}
