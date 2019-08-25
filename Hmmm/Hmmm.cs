using System;
using System.Collections.Generic;

using static DTV.Instruction;

namespace DTV
{
	/// <summary>
	/// A Harvey Mudd Miniature Machine model.
	/// Generally, the following convention is used: rX is a target register, rY is a source register, rZ is a second source register, found on the right of the operator represented by the instruction.
	/// </summary>
	/// <remarks>
	/// Uses https://www.cs.hmc.edu/~cs5grad/cs5/hmmm/documentation/documentation.html as reference.
	/// </remarks>
	public class Hmmm{
		public event EventHandler SystemHaltEvent;
		protected virtual void OnSystemHaltEvent(EventArgs e)
		{
			EventHandler handler = SystemHaltEvent;
			handler?.Invoke(this, e);
		}
		public RegisterSet Registers = new RegisterSet();
		public ushort[] memory = new ushort[256];
	//	// memory helper method
	//	private ushort GetWord(byte address) { return new ushort ( memory[address], memory[address+1]);}
	//	private void SetWord(byte address, byte[] value){memory[address]=value[0]; memory[address+1] = value[1];}
		public byte ProgramCounter = 0;

		public Instruction Decode(ushort instruction)
		{ 
			foreach (Instruction key in InstructionMaskIdentityPairs.Keys)
			{
				if ((InstructionMaskIdentityPairs[key].Mask & instruction)
					== InstructionMaskIdentityPairs[key].Identity
					)
				{
					return key;
				}
			}
			throw new InvalidOperationException("The instruction at PC=" + ProgramCounter + " : " + FormatBinaryString(instruction) + " is malformed.");
		}

		public void OnTick()
		{
			Instruction instruction = Decode(memory[ProgramCounter]);
			switch(instruction){
				case Instruction.HALT:
				default:
					Halt();
					break;
			} 
		}

		#region System instructions
		public void Halt() => OnSystemHaltEvent(null);
		public void Nop() {/* Instruction CAN be decoded as a copy from r0 to r0, and in effect this method MIGHT only be called artificially, and not by a "real" program. */}
		public void Read(byte rX) 
		{
			ushort value;
			 do { Console.WriteLine("Enter number: "); }
			 while (!ushort.TryParse(Console.ReadLine(), out value));
			 Registers[rX] = value;
		}
		public void Write(byte rX) { Console.WriteLine(Registers[rX]); }
		#endregion
		#region Setting register data
		public void SetN(byte rX, byte n) { memory[rX] = n; }
		public void AddN(byte rX, byte n) { Registers[rX] = (ushort)(Registers[rX] + n); }
		public void Copy(byte rX, byte rY) { Registers[rX] = Registers[rY]; }
		#endregion
		#region Arithmetic
		public void Neg(byte rX, byte rY) { Sub(rX, Registers[0], rY); } // reuses subtract
		public void Add(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] + Registers[rZ]); }
		public void Sub(byte rX, byte rY, byte rZ) { Registers[rX] = Registers[rY] - Registers[rZ]; }
		public void Mul(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] * Registers[rZ]); }
		public void Div(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] / Registers[rZ]); }
		public void Mod(byte rX, byte rY, byte rZ) { Registers[rX] = (ushort)(Registers[rY] % Registers[rZ]); }
		#endregion
		#region Jumps
		public void LoadR(byte rX, byte rY) { Registers[rX] = memory[rY]; }
		public void StoreR(byte rX, byte rY) { memory[rY] = Registers[rX]; }
		public void PopR(byte rX, byte rY) { Registers[rY] -= 1; Registers[rX] = memory[Registers[rY]]; }
		public void PushR(byte rX, byte rY) { memory[Registers[rY]] = Registers[rX]; Registers[rY] += 1; }
		public void LoadN(byte rX, byte n) { Register[rX] = n; }
		public void StoreN(byte rX, byte n) { memory[n] = Registers[rX]; }
		public void Jump(byte rX) { ProgramCounter = Registers[rX]; }
		/// <remarks>
		/// The official Hmmm spec indicates that a Jump to -1 is possible however seeing as the memory addresses range from 0 to 255 for 256 words and that is exactly the number of addresses that can be represented by a byte, which is what the instruction uses for n,
		/// I believe this to be an oversight enabled by the use of Python in the original Hmmm simulator.
		/// </remarks>
		public void JumpN(byte n) { ProgramCounter = n; }
		public void Jeqz(byte rX, byte n) { (Registers[rX] == 0) ? (JumpN(n)) : Nop(); }
		public void Jneqz(byte rX, byte n) { (Registers[rX] != 0) ? (JumpN(n)) : Nop(); }
		public void Jgtz(byte rX, byte n) { (Registers[rX] > 0) ? (JumpN(n)) : Nop(); }
		public void Jltz(byte rX, byte n) { (Registers[rX] < 0) ? (JumpN(n)) : Nop(); }
		public void CallN(byte rX, byte n) { Registers[rX] = ProgramCounter + 1; ProgramCounter = n; }
		#endregion
		#region Aliases
		public void Call(byte rX, byte n) => CallN(rX, n);
		#endregion

		
		private static Dictionary<Instruction, MaskIdentityPair> InstructionMaskIdentityPairs = new Dictionary<Instruction, MaskIdentityPair>(){
			{HALT, new MaskIdentityPair(){Mask=(ushort)0b1111_1111_1111_1111, Identity=(ushort)0b0000_0000_0000_0000}},
			{NOP, new MaskIdentityPair(){Mask=(ushort)0b1111_1111_1111_1111, Identity=(ushort)0b0110_0000_0000_0000}}, // NOP is equivalent to `COPY r0 r0`
			{READ, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_1111_1111, Identity=(ushort)0b0000_0000_0000_0001}},
			{WRITE, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_1111_1111, Identity=(ushort)0b0000_0000_0000_0010}},
			{SETN, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0001_0000_0000_0000}},
			{LOADR, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_1111, Identity=(ushort)0b0100_0000_0000_0000}},
			{STORER, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_1111, Identity=(ushort)0b0100_0000_0000_0001}},
			{POPR, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_1111, Identity=(ushort)0b0100_0000_0000_0010}},
			{PUSHR, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_1111, Identity=(ushort)0b0100_0000_0000_0011}},
			{LOADN, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0010_0000_0000_0000}},
			{STOREN, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0011_0000_0000_0000}},
			{ADDN, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0101_0000_0000_0000}},
			{COPY, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_1111, Identity=(ushort)0b0110_0000_0000_0000}},
			{NEG, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_1111_0000, Identity=(ushort)0b0111_0000_0000_0000}}, // NEG is equivalent to `SUB rX r0 rY`
			{ADD, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0110_0000_0000_0000}},
			{SUB, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b0111_0000_0000_0000}},
			{MUL, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1000_0000_0000_0000}},
			{DIV, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1001_0000_0000_0000}},
			{MOD, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1010_0000_0000_0000}},
			{JUMP, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_1111_1111, Identity=(ushort)0b0000_0000_0000_0011}},
			{JUMPN, new MaskIdentityPair(){Mask=(ushort)0b1111_1111_0000_0000, Identity=(ushort)0b1011_0000_0000_0000}},
			{JEQZ, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1100_0000_0000_0000}},
			{JNEZ, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1101_0000_0000_0000}},
			{JGTZ, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1110_0000_0000_0000}},
			{JLTZ, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_0000_0000, Identity=(ushort)0b1111_0000_0000_0000}},
			{CALL, new MaskIdentityPair(){Mask=(ushort)0b1111_0000_1111_1111, Identity=(ushort)0b1011_0000_0000_0000}} // CALL r0 N is equivalent to JUMPN N
			
		};

		private string FormatBinaryString(ushort number){
			string s = Convert.ToString(number, 2).PadLeft(16, '0');
			int[] seperatorPositions = new int[] {12, 8, 4};
			foreach (var index in seperatorPositions) // must happen in the order described above
			{
				s = s.Insert(index, "_");
			}
			return "0b"+s;
		}
		}

	public enum Instruction { HALT, NOP, READ, WRITE, SETN, LOADR, STORER, POPR, PUSHR, LOADN, STOREN, ADDN, COPY, NEG, ADD, SUB, MUL, DIV, MOD, JUMP, JUMPN, JEQZ, JNEZ, JGTZ, JLTZ, CALL }
	struct MaskIdentityPair{
		public ushort Mask;
		public ushort Identity;
	}

	public class RegisterSet
	{
		private ushort[] _registers = new ushort[16];
		public ushort this[byte index]
		{
			get {if (index == 0) { return 0; } else { return _registers[index]; }}
			set { if (index == 0) {} else { _registers[index] = value; }}
		}
	}
}