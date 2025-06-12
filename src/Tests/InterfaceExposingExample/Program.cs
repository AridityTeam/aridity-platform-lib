using InterfaceExposingShared;

if (GlobalDefs.MyInterfaceInstance != null)
    GlobalDefs.MyInterfaceInstance.SomeFunction();
else
    Console.Error.WriteLine("Interface instance is null!");