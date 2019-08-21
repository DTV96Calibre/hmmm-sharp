void Main()
{
	var hmmm = new Hmmm();
	hmmm.AddN(0, 1);
	hmmm.AddN(1, 255);
	hmmm.Add(0, 0, 1);
	hmmm.Mul(0, 0, 1);
	hmmm.Mul(0, 0, 1);
	hmmm.Write(0);
}

// Define other methods and classes here



// Below, rX is a target register, rY is a source register, rZ is a second source register, found on the right of the operator represented by the instruction.
class Hmmm{
	
	public ushort[] Registers = new ushort[16];
	public ushort[] memory = new ushort[256];
//	// memory helper method
//	private ushort GetWord(byte address) { return new ushort ( memory[address], memory[address+1]);}
//	private void SetWord(byte address, byte[] value){memory[address]=value[0]; memory[address+1] = value[1];}
	public byte ProgramCounter = 0;

	public Instruction Decode(short instruction)
	{ switch (instruction) 
	{
		case 0b0000_0000_0000_0000:
		default:
		return Instruction.HALT;
	}}

	#region System instructions
	public void Halt() { System.Environment.Exit(0); }
	public void Nop() { }
	public void Read(byte rX) { do { Console.WriteLine("Enter number: "); } while (!ushort.TryParse(Console.ReadLine(), out Registers[rX]));}
	public void Write(byte rX) { Console.WriteLine(Registers[rX]); }
	public void SetN(byte rX, byte n) { memory[rX] = n; }
	public void LoadR(byte rX, byte rY) { Registers[rX] = memory[rY]; }
	public void StoreR(byte rX, byte rY) { memory[rY] = Registers[rX]; }
	public void PopR(byte rX, byte rY) { throw new NotImplementedException(); }
	public void PushR(byte rX, byte rY) { throw new NotImplementedException(); }
	public void LoadN(byte rX, byte n) { throw new NotImplementedException(); }
	public void StoreN(byte rX, byte n) { throw new NotImplementedException(); }
	public void AddN(byte rX, byte n) { Registers[rX] = (ushort)(Registers[rX] + n); }
	public void Copy(byte rX, byte rY) { throw new NotImplementedException(); }
	public void Neg(byte rX, byte rY) { throw new NotImplementedException(); }
	public void Add(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] + Registers[rZ]); }
	public void Sub(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	public void Mul(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] * Registers[rZ]); }
	public void Div(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	public void Mod(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	public void Jump(byte rX) { throw new NotImplementedException(); }
	public void JumpN(byte n) { throw new NotImplementedException(); }
	public void Jeqz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jneqz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jgtz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jltz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Call(byte rX, byte n) { throw new NotImplementedException(); }
	#endregion
}

enum Instruction { HALT, NOP, READ, WRITE, SETN, LOADR, STORER, POPR, PUSHR, LOADN, STOREN, ADDN, COPY, NEG, ADD, SUB, MUL, DIV, MOD, JUMP, JUMPN, JEQZ, JNEZ, JGTZ, JLTZ, CALL }

struct Word
{
}