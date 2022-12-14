using CommandLine;
using listHelper;
using MyProjekt.CommandLineParser;
using Print;
using Shell;
using Console = Print.Console;

namespace MyProjekt;

internal static class MyBuilder
{
    private static void Main(string[] args)
    {
        Parser.Default
            .ParseArguments<StartupOptions>(args)
            .WithParsed(Foo);
    }

    //Traverses the directory, folders and files, and passes all TypeScript(ts) files to the tsToJs function,and copies the folder and file structure.


    private static void Foo(StartupOptions options)
    {
        var separator = Path.PathSeparator;

        CreateFolder(options.ProjectDirectory, "BuilderFolder", separator);
        ProjectFolderSearch(options.ProjectDirectory, options.ProjectDirectory, separator, options.MasterFile);
        Console.WriteLine(options.MasterFile, ConsoleColor.Blue);
        RunMasterFile(options.MasterFile);
        Console.WriteLine("########### Build Finish ###########", ConsoleColor.Green);
    }


    private static void ProjectFolderSearch(string projectFolder, string mainFolderPath, char sep, string masterFile)
    {
        foreach (var tsNameTs in Directory.GetFiles(projectFolder))
        {
            var directoryNames = mainFolderPath.Split(sep);
            var tsNameTsList = tsNameTs.Split(sep);
            var tsNameEndsList = tsNameTsList[^1].Split(".");
            var jsName = sep + tsNameEndsList[0] + ".js";
            var jsNameFinal = ListHelper.listSelector(tsNameTsList, 0, tsNameTsList.Length - 1, sep.ToString()) +
                              jsName;
            var movePath = mainFolderPath + sep + "BuilderFolder" + sep + ListHelper.listSelector(tsNameTsList,
                directoryNames.Length, tsNameTsList.Length - 1, sep.ToString());


            if (tsNameTs.ToLower().EndsWith(".ts"))
            {
                TsToJs(tsNameTs);
                var goStop = true;
                while (goStop)
                    foreach (var tsNameJs in Directory.GetFiles(projectFolder))
                        if (string.Equals(tsNameJs, jsNameFinal, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine("js-file: {0}", ConsoleColor.Green, jsNameFinal);
                            Command.Heandler(OsCommand("move") + " " + jsNameFinal + " " + movePath);
                            goStop = false;
                            break;
                        }
            }

            else if (!string.Equals(tsNameTs, jsNameFinal, StringComparison.CurrentCultureIgnoreCase) ||
                     !tsNameTs.ToLower().EndsWith(".ts"))
            {
                var movePathSplit = tsNameTs.Split(sep);
                var movePathElse = mainFolderPath + sep + "BuilderFolder" + sep + ListHelper.listSelector(movePathSplit,
                    directoryNames.Length, movePathSplit.Length, sep.ToString());
                System.Console.WriteLine(OsCommand("copy") + " " + tsNameTs + " " + movePathElse);
                Command.Heandler(OsCommand("copy") + " " + tsNameTs + " " + movePathElse);
            }


            if (!string.Equals(tsNameTsList[^1], masterFile, StringComparison.CurrentCultureIgnoreCase)) continue;

            masterFile = mainFolderPath + sep + "BuilderFolder" +
                         ListHelper.listSelector(tsNameTsList, mainFolderPath.Length, tsNameTsList.Length,
                             sep.ToString()) + sep + tsNameTsList[^1];
            Console.WriteLine(masterFile, ConsoleColor.Blue);
        }

        foreach (var tsName in Directory.GetDirectories(projectFolder))
        {
            var mainpfadSplit = mainFolderPath.Split(sep);
            var folderName = tsName.Split(sep);


            if (!Directory.Exists(mainFolderPath + sep + "BuilderFolder" + sep))
            {
                Console.WriteLine(mainFolderPath, ConsoleColor.Blue);
                CreateFolder(mainFolderPath, sep + "BuilderFolder", sep);
                ProjectFolderSearch(tsName, mainFolderPath, sep, masterFile);
            }
            else
            {
                if (tsName == mainFolderPath + sep + "BuilderFolder") continue;

                var finalString = mainFolderPath + sep + "BuilderFolder" + sep;
                var finalmainpfadSplit =
                    ListHelper.listSelector(folderName, mainpfadSplit.Length, folderName.Length, sep.ToString());
                Console.WriteLine(finalString + sep + finalmainpfadSplit, ConsoleColor.Green);
                CreateFolder(finalString, finalmainpfadSplit, sep);
                ProjectFolderSearch(tsName, mainFolderPath, sep, masterFile);
            }
        }
    }


    /// <summary>
    ///     Passes to the function Heandler, in the class Command, in the namamespace Shell, the command to convert Typscript
    ///     files with tsc
    /// </summary>
    /// <param name="tsName"></param>
    private static void TsToJs(string tsName)
    {
        var command = "tsc " + tsName;
        System.Console.WriteLine(command);
        Command.Heandler(command);
        System.Console.WriteLine("Finish");
    }

    /// <summary>
    ///     takes a path, an directory name and a separator and creates a folder from it
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="name"></param>
    /// <param name="sep"></param>
    private static void CreateFolder(string folderPath, string name, char sep)
    {
        Directory.CreateDirectory(folderPath + sep + name);
        System.Console.WriteLine(name);
    }


    //

    /// <summary>
    ///     returns different commands depending on the operating system
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    private static string OsCommand(string command)
    {
        if (OperatingSystem.IsLinux())
            switch (command.ToLower())
            {
                case "copy":
                    return "cp";
                case "move":
                    return "mv";

                default:
                    return "Fail";
            }

        switch (command.ToLower())
        {
            case "copy":
                return "copy";
            case "move":
                return "move";
            default:
                return "Fail";
        }
    }

    //opens the master file
    private static void RunMasterFile(string masterFile)
    {
        if (OperatingSystem.IsLinux())
            Command.Heandler(" open " + masterFile);
        else
            Command.Heandler(masterFile);
    }
}