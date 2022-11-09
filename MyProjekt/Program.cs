using listHelper;
using Print;
using Shell;

namespace MyProjekt;

internal static class MyBuilder
{
    public static string masterFile;

    private static void Main(string[] args)
    {
        var separator = Path.PathSeparator;


        Console.WriteLine("Please indicate the path of a project");
        var mainFolderPath = Console.ReadLine();

        Console.WriteLine("Pleace indicate the name of the master(main,index) file");
        masterFile = Console.ReadLine();

        CreateFolder(mainFolderPath, "BuilderFolder", separator);
        ProjectFolderSearch(mainFolderPath, mainFolderPath, separator);
        Con.print(masterFile, ConsoleColor.Blue);

        RunMasterFile();
        Con.print("########### Build Finish ###########", ConsoleColor.Green);
    }

    //Traverses the directory, folders and files, and passes all TypeScript(ts) files to the tsToJs function,and copies the folder and file structure.

    private static void ProjectFolderSearch(string projectFolder, string mainFolderPath, char sep)
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
                            Con.print(jsNameFinal, ConsoleColor.Green);
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
                Console.WriteLine(OsCommand("copy") + " " + tsNameTs + " " + movePathElse);
                Command.Heandler(OsCommand("copy") + " " + tsNameTs + " " + movePathElse);
            }


            if (!string.Equals(tsNameTsList[^1], masterFile, StringComparison.CurrentCultureIgnoreCase)) continue;

            masterFile = mainFolderPath + sep + "BuilderFolder" +
                         ListHelper.listSelector(tsNameTsList, mainFolderPath.Length, tsNameTsList.Length,
                             sep.ToString()) + sep + tsNameTsList[^1];
            Con.print(masterFile, ConsoleColor.Blue);
        }

        foreach (var tsName in Directory.GetDirectories(projectFolder))
        {
            var mainpfadSplit = mainFolderPath.Split(sep);
            var folderName = tsName.Split(sep);


            if (!Directory.Exists(mainFolderPath + sep + "BuilderFolder" + sep))
            {
                Con.print(mainFolderPath, ConsoleColor.Blue);
                CreateFolder(mainFolderPath, sep + "BuilderFolder", sep);
                ProjectFolderSearch(tsName, mainFolderPath, sep);
            }
            else
            {
                if (tsName == mainFolderPath + sep + "BuilderFolder") continue;

                var finalString = mainFolderPath + sep + "BuilderFolder" + sep;
                var finalmainpfadSplit =
                    ListHelper.listSelector(folderName, mainpfadSplit.Length, folderName.Length, sep.ToString());
                Con.print(finalString + sep + finalmainpfadSplit, ConsoleColor.Green);
                CreateFolder(finalString, finalmainpfadSplit, sep);
                ProjectFolderSearch(tsName, mainFolderPath, sep);
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
        Console.WriteLine(command);
        Command.Heandler(command);
        Console.WriteLine("Finish");
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
        Console.WriteLine(name);
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
    private static void RunMasterFile()
    {
        if (OperatingSystem.IsLinux())
            Command.Heandler(" open " + masterFile);
        else
            Command.Heandler(masterFile);
    }
}