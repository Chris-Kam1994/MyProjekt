using CommandLine;

namespace MyProjekt.CommandLineParser;

public class StartupOptions
{
    [Option('m', "masterfile", Required = true,
        HelpText = "fullname of file which shall be executed after successful execution")]
    public string MasterFile { get; set; } = "";


    [Option('d', "projectdirectory", Required = true,
        HelpText = "location of the directory in which the directory is located")]
    public string ProjectDirectory { get; set; } = "";


}