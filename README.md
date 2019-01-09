# eSLR
*A C# CPU emulation package for the Sharp LR35902, used in the original GameBoy.*

## Introduction
Welcome! This repository is the home for eSLR, a C# SLR35902 emulator, a CPU extremely similar in opcode setup and architecture to the Zilog Z80. Due to this, the emulator can also be used to emulate a basic Z80 by putting it in "Zilog Mode", however this is currently experimental and untested, and not recommended. For another more stable Zilog emulator, try [z80nuget](https://github.com/sklivvz/z80).

## Usage
To use the CPU class library, set up your code like so:
  using emuSLR;
  using System;
  
  ...
  
  public void Main() {
    var myCPU = new eSLR();
    myCPU.state = State.ProcessorStates.NORMAL;
    
    //Hook up your memory controller here.
    myCPU.SaveByte = mySaveByteFunction;
    ...
    
    //Set the PC to somewhere in memory and go!
    myCPU.reg.PC = 0x0FF;
    myCPU.exec();
  }
  
