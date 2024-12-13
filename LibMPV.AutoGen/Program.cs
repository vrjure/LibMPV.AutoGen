using CppSharp;
using CppSharp.AST;
using CppSharp.Passes;
using LibMPV.AutoGen;
using LibMPV.AutoGen.Generation;

AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    Console.Error.WriteLine(e.ExceptionObject.ToString());
    Console.ReadLine();
}
//https://sourceforge.net/projects/mpv-player-windows/files/

var mpv_outputDir = Path.GetFullPath("../../../../../LibMPVSharp");
var mpv_h_folder = Path.Combine(Environment.CurrentDirectory, "mpv", "include");

var library = new MPVLibrary(mpv_h_folder, mpv_outputDir);
var driverOption = new DriverOptions();
var driver = new Driver(driverOption);

library.Setup(driver);
driver.Setup();

if (!driver.ParseLibraries())
{
    Console.WriteLine("failed");
    return;
}

driver.ParseCode();

if (!driver.ParseCode())
{
    Diagnostics.Error("CppSharp has encountered an error while parsing code.");
    return;
}

CleanUnitPass cleanUnitPass = new CleanUnitPass();
cleanUnitPass.Context = driver.Context;
cleanUnitPass.VisitASTContext(driver.Context.ASTContext);
foreach (Module item in driverOption.Modules.Where((Module m) => m != driverOption.SystemModule && !m.Units.GetGenerated().Any()))
{
    Diagnostics.Message($"Removing module {item} because no translation units are generated...");
    driverOption.Modules.Remove(item);
}

driver.SetupPasses(library);
driver.SetupTypeMaps();
driver.SetupDeclMaps();
library.Preprocess(driver, driver.Context.ASTContext);
driver.ProcessCode();
library.Postprocess(driver, driver.Context.ASTContext);


//ConsoleDriver.Run(new MPVLibrary(mpv_h_folder, mpv_outputDir));

Console.WriteLine("done!");