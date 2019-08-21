using static Instruction;

public static class Bench
{

	public static void Main()
	{
		var hmmm = new Hmmm();
		hmmm.AddN(0, 1);
		hmmm.AddN(1, 255);
		hmmm.Add(0, 0, 1);
		hmmm.Mul(0, 0, 1);
		hmmm.Mul(0, 0, 1);
		hmmm.Write(0);
	}
}


// Define other methods and classes here



/// <summary>
/// A Harvey Mudd Miniature Machine model.
/// Generally, the following convention is used: rX is a target register, rY is a source register, rZ is a second source register, found on the right of the operator represented by the instruction.
/// </summary>
/// <remarks>
/// Uses https://www.cs.hmc.edu/~cs5grad/cs5/hmmm/documentation/documentation.html as reference.
/// </remarks>
class Hmmm{
	
	public ushort[] Registers = new ushort[16];
	public ushort[] memory = new ushort[256];
//	// memory helper method
//	private ushort GetWord(byte address) { return new ushort ( memory[address], memory[address+1]);}
//	private void SetWord(byte address, byte[] value){memory[address]=value[0]; memory[address+1] = value[1];}
	public byte ProgramCounter = 0;

	public Instruction Decode(short instruction)
	{ 
		foreach (Instruction key in InstructionMaskIdentityPairs.Keys)
		{
			if (InstructionMaskIdentityPairs[key].Mask && instruction
				== InstructionMaskIdentityPairs[key].Value
				)
			{
				 return key;
			}
		}
		throw new InvalidOperationException("The instruction at PC=" + ProgramCounter + " : " + FormatBinaryString(instruction) + " is malformed.");
		return HALT;
	}

	public void OnTick()
	{
		Instruction instruction = Decode(memory[ProgramCounter]);
		switch(instruction){
			case Instruction.HALT:
			default:
				Halt();
		} 
	}

	#region System instructions
	public void Halt() { ProgramCounter = -1; }
	public void Nop() {}
	public void Read(byte rX) { do { Console.WriteLine("Enter number: "); } while (!ushort.TryParse(Console.ReadLine(), out Registers[rX]));}
	public void Write(byte rX) { Console.WriteLine(Registers[rX]); }
	#endregion
	#region Setting register data
	public void SetN(byte rX, byte n) { memory[rX] = n; }
	public void AddN(byte rX, byte n) { Registers[rX] = (ushort)(Registers[rX] + n); }
	public void Copy(byte rX, byte rY) { throw new NotImplementedException(); }
	#endregion
	#region Arithmetic
	public void Neg(byte rX, byte rY) { throw new NotImplementedException(); }
	public void Add(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] + Registers[rZ]); }
	public void Sub(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	public void Mul(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] * Registers[rZ]); }
	public void Div(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	public void Mod(byte rX, byte rY, byte rZ) { throw new NotImplementedException(); }
	#endregion
	#region Jumps
	public void LoadR(byte rX, byte rY) { Registers[rX] = memory[rY]; }
	public void StoreR(byte rX, byte rY) { memory[rY] = Registers[rX]; }
	public void PopR(byte rX, byte rY) { throw new NotImplementedException(); }
	public void PushR(byte rX, byte rY) { throw new NotImplementedException(); }
	public void LoadN(byte rX, byte n) { throw new NotImplementedException(); }
	public void StoreN(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jump(byte rX) { throw new NotImplementedException(); }
	public void JumpN(byte n) { throw new NotImplementedException(); }
	public void Jeqz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jneqz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jgtz(byte rX, byte n) { throw new NotImplementedException(); }
	public void Jltz(byte rX, byte n) { throw new NotImplementedException(); }
    public void CallN(byte rX, byte n) { throw new NotImplementedException(); }

	#region Aliases
    public void Call(byte rX, byte n) => CallN(rX, n);
	#endregion
}

public enum Instruction { HALT, NOP, READ, WRITE, SETN, LOADR, STORER, POPR, PUSHR, LOADN, STOREN, ADDN, COPY, NEG, ADD, SUB, MUL, DIV, MOD, JUMP, JUMPN, JEQZ, JNEZ, JGTZ, JLTZ, CALL }
private static Dictionary<Instruction, MaskIdentityPair> InstructionMaskIdentityPairs = new Dictionary<Instruction, MaskIdentityPair>(){
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{NOP, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0110_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
	{HALT, MaskIdentityPair(Mask=b1111_1111_1111_1111, Identity=b0000_0000_0000_0000)},
};
struct MaskIdentityPair{
	short Mask;
	short Identity;
}

private string FormatBinaryString(ushort number){
	string s = Convert.ToString(number, 2).PadLeft(16, '0');
	int[] seperatorPositions = new int[] {12, 8, 4};
	foreach (var index in seperatorPositions) // must happen in the order described above
	{
		s = s.Insert(index, "_");
	}
	return "0b"+s;
}