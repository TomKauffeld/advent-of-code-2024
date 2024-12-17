namespace AdventOfCode.Day17
{
    internal class ChronospatialVm
    {
        private readonly Dictionary<char, long> _registers = [];

        private readonly byte[] _memory;

        private readonly List<byte> _output = [];


        public ChronospatialVm(byte[] memory)
        {
            _memory = memory;
        }

        public long GetRegister(char register) => _registers.GetValueOrDefault(register, 0L);

        public void SetRegister(char register, long value) => _registers[register] = value;

        public ushort GetProgramCounter() => (ushort)GetRegister('P');
        public void SetProgramCounter(ushort value) => SetRegister('P', value);
        public void IncrementProgramCounter() => SetProgramCounter((ushort)(GetProgramCounter() + 2u));

        public ushort GetAndIncrementProgramCounter()
        {
            ushort pc = GetProgramCounter();
            SetProgramCounter((ushort)(pc + 1));
            return pc;
        }

        public IReadOnlyList<byte> GetMemory()
        {
            return _memory.AsReadOnly();
        }

        public byte GetMemoryAt(ushort address) => _memory[address];
        public byte SetMemoryAt(ushort address, byte value) => _memory[address] = value;

        public byte ReadOpcode() => GetMemoryAt(GetAndIncrementProgramCounter());
        public byte ReadOperand() => GetMemoryAt(GetAndIncrementProgramCounter());

        private long GetComboOperand(byte operand)
        {
            return operand switch
            {
                <= 3 => operand,
                <= 6 => GetRegister((char)('A' - 4 + operand)),
                _ => throw new Exception($"Operand {operand} not defined")
            };
        }

        private long GetLiteralOperand(byte operand) => operand;

        private byte ExecuteAdv(byte operand, char targetRegister)
        {
            long a = GetRegister('A');
            long b = GetComboOperand(operand);
            double d = Math.Pow(2, b);
            double v = a / d;
            long r = (long)Math.Floor(v);
            SetRegister(targetRegister, r);
            return 2;
        }

        private byte ExecuteBxl(byte operand)
        {
            long a = GetRegister('B');
            long b = GetLiteralOperand(operand);
            long r = a ^ b;
            SetRegister('B', r);
            return 2;
        }

        private byte ExecuteBst(byte operand)
        {
            long a = GetComboOperand(operand);
            long r = a % 8L;
            SetRegister('B', r);
            return 2;
        }

        private byte ExecuteJnz(byte operand)
        {
            long a = GetRegister('A');
            long b = GetLiteralOperand(operand);
            if (a != 0L)
                SetRegister('P', b);
            return 2;
        }

        private byte ExecuteBxc(byte _)
        {
            long a = GetRegister('B');
            long b = GetRegister('C');
            long r = a ^ b;
            SetRegister('B', r);
            return 2;
        }

        private byte ExecuteOut(byte operand)
        {
            long a = GetComboOperand(operand);
            long r = a % 8L;
            _output.Add((byte)r);
            return 2;
        }

        private byte ExecuteInstruction(byte opcode)
        {
            return opcode switch
            {
                0 => ExecuteAdv(ReadOpcode(), 'A'),
                1 => ExecuteBxl(ReadOpcode()),
                2 => ExecuteBst(ReadOpcode()),
                3 => ExecuteJnz(ReadOpcode()),
                4 => ExecuteBxc(ReadOpcode()),
                5 => ExecuteOut(ReadOpcode()),
                6 => ExecuteAdv(ReadOpcode(), 'B'),
                7 => ExecuteAdv(ReadOpcode(), 'C'),
                _ => throw new Exception($"Invalid opcode {opcode}")
            };
        }

        public void ResetState()
        {
            _registers.Clear();
            _output.Clear();
        }

        public IReadOnlyList<byte> GetOutput()
        {
            return _output.AsReadOnly();
        }

        public bool RunOnce()
        {
            if (GetProgramCounter() >= _memory.Length)
                return false;

            byte opcode = ReadOpcode();
            ExecuteInstruction(opcode);
            return true;
        }

        public IReadOnlyList<byte> ExecuteAll(int maxInstructions = int.MaxValue)
        {
            int i;
            for (i = 0; i < maxInstructions && RunOnce(); ++i)
            {
            }

            if (i >= maxInstructions)
                throw new Exception("");

            return GetOutput();
        }

        public ChronospatialVm Clone()
        {
            ChronospatialVm vm = new([.. _memory]);
            foreach (KeyValuePair<char, long> register in _registers)
                vm.SetRegister(register.Key, register.Value);
            return vm;
        }

    }
}
