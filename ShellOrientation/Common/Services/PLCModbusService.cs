

using NModbus;
using System;
using System.Net.Sockets;

namespace ShellOrientation.Common.Services
{
    public class PLCModbusService : IPLCModbusService
    {
        IModbusMaster master;
        TcpClient client;
        public bool Connect(string ip)
        {
            try
            {
                client = new TcpClient(ip, 502);
                var factory = new ModbusFactory();
                master = factory.CreateMaster(client);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void Close()
        {
            try
            {
                master.Dispose();
                client.Close();
                client.Dispose();
            }
            catch { }

        }
        public bool[] ReadXCoils(int addr, int count)
        {
            return master.ReadCoils(1, (ushort)(0xF800 + addr), (ushort)count);
        }
        public bool[] ReadYCoils(int addr, int count)
        {
            return master.ReadCoils(1, (ushort)(0xFC00 + addr), (ushort)count);
        }
        public void WriteYCoil(int addr, bool val)
        {
            master.WriteSingleCoil(1, (ushort)(0xFC00 + addr), val);
        }
        public ushort[] ReadDRegisters(int addr, int count)
        {
            return master.ReadHoldingRegisters(1, (ushort)addr, (ushort)count);
        }
        public void WriteSingleRegister(int addr, int value)
        {
            var a1 = Convert.ToUInt16(((short)value).ToString("X4"), 16);
            master.WriteSingleRegister(1, (ushort)addr, a1);
        }
        public bool[] ReadMCoils(int addr, int count)
        {
            return master.ReadCoils(1, (ushort)addr, (ushort)count);
        }
        public void WriteMCoil(int addr, bool val)
        {
            master.WriteSingleCoil(1, (ushort)addr, val);
        }
    }
}
