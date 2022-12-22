namespace ShellOrientation.Common.Services
{
    public interface IPLCModbusService
    {
        void Close();
        bool Connect(string ip);
        ushort[] ReadDRegisters(int addr, int count);
        bool[] ReadMCoils(int addr, int count);
        bool[] ReadXCoils(int addr, int count);
        bool[] ReadYCoils(int addr, int count);
        void WriteMCoil(int addr, bool val);
        void WriteSingleRegister(int addr, int value);
        void WriteYCoil(int addr, bool val);
    }
}