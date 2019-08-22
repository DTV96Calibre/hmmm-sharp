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
		public void Nop() {}
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
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>The official Hmmm spec indicates that a Jump to -1 is possible however seeing as the memory addresses range from 0 to 255 for 256 words and that is exactly the number of addresses that can be represented by a byte, which is what the instruction uses for n,
		/// I believe this to be an oversight enabled by the use of Python in the original Hmmm simulator.</remarks>
		public void JumpN(byte n) { throw new NotImplementedException(); }
		public void Jeqz(byte rX, byte n) { throw new NotImplementedException(); }
		public void Jneqz(byte rX, byte n) { throw new NotImplementedException(); }
		public void Jgtz(byte rX, byte n) { throw new NotImplementedException(); }
		public void Jltz(byte rX, byte n) { throw new NotImplementedException(); }
		public void CallN(byte rX, byte n) { throw new NotImplementedException(); }
		#endregion
		#region Aliases
		public void Call(byte rX, byte n) => CallN(rX, n);
		#endregion

		
		private static Dictionary<Instruction, MaskIdentityPair> InstructionMaskIdentityPairs = new Dictionary<Instruction, MaskIdentityPair>(){
			{HALT, new MaskIdentityPair(){Mask=(ushort)0b1111_1111_1111_1111, Identity=(ushort)0b0000_0000_0000_0000}},
			{NOP, new MaskIdentityPair(){Mask=(ushort)0b1111_1111_1111_1111, Identity=(ushort)0b0110_0000_0000_0000}}
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