using listHelper;
using Print;


namespace MyBuilder
{

    class MyBuilder
    {
        public static string masterFile;

        static void Main(string[] args)
        {   
            char separator= osSeparator();
            
             
           Console.WriteLine("Please indicate the path of a project");
           string mainFolderPath = Console.ReadLine();

           Console.WriteLine("Pleace indicate the name of the master(main,index) file");
           masterFile =Console.ReadLine();

           createFolder(mainFolderPath,"BuilderFolder",separator);
           projectFolderSearch(mainFolderPath,mainFolderPath,separator);
           Con.print(masterFile,ConsoleColor.DarkYellow);
                
           runMasterFile();
           Con.print("########### Build Finish ###########",ConsoleColor.Green);    
                
            

        }

        //Traverses the directory, folders and files, and passes all TypeScript(ts) files to the tsToJs function,and copies the folder and file structure.

        static void projectFolderSearch(string projectFolder,string mainFolderPath,char sep)
        {
            foreach(string tsNameTs in Directory.GetFiles(projectFolder))
            {
                string[] mainpfadSplit = mainFolderPath.Split(sep);
                string[] tsNameTsList  = tsNameTs.Split(sep);
                string[] TsNameEndsList = tsNameTsList[tsNameTsList.Length-1].Split(".");
                string jsName = sep + TsNameEndsList[0]  +".js";
                string jsNameFinal = ListHelper.listSelector(tsNameTsList,0,tsNameTsList.Length-1,sep.ToString()) + jsName;
                string movePath = mainFolderPath + sep + "BuilderFolder"+sep+ListHelper.listSelector(tsNameTsList,mainpfadSplit.Length,tsNameTsList.Length-1,sep.ToString());


                if(tsNameTs.ToLower().EndsWith(".ts"))
                { 
                    tSToJs(tsNameTs,projectFolder);
                    bool goStop = true;
                    while(goStop)
                    {
                        foreach(string tsNameJs in Directory.GetFiles(projectFolder))
                        {
                            
                            if(tsNameJs.ToLower() == jsNameFinal.ToLower())
                            {
                                Con.print(jsNameFinal,ConsoleColor.Green);
                                Shell.Command.Heandler(osCommand("move") +" "+ jsNameFinal +" "+movePath);
                                goStop = false;
                                break;
                                
                            }
                            else
                            {
                                continue;
                            }
                        }

                    }

                    
                    
                }

                else if(tsNameTs.ToLower()!=jsNameFinal.ToLower()||!tsNameTs.ToLower().EndsWith(".ts"))
                {

                  string[] movePathSplit = tsNameTs.Split(sep);
                  string movePathElse = mainFolderPath+ sep + "BuilderFolder" + sep + ListHelper.listSelector(movePathSplit,mainpfadSplit.Length,movePathSplit.Length,sep.ToString());
                  Console.WriteLine(osCommand("copy") +" "+ tsNameTs +" "+ movePathElse);
                  Shell.Command.Heandler(osCommand("copy")+" "+ tsNameTs +" "+ movePathElse);

                }


                if (tsNameTsList[tsNameTsList.Length-1].ToLower()==masterFile.ToLower())
                {

                    masterFile =  mainFolderPath + sep + "BuilderFolder" + ListHelper.listSelector(tsNameTsList, mainFolderPath.Length, tsNameTsList.Length, sep.ToString()) +sep+ tsNameTsList[tsNameTsList.Length-1];
                    Con.print(masterFile,ConsoleColor.Blue);
                    
                }
                
                
                    
            }

            foreach(string tsName in Directory.GetDirectories(projectFolder)) 
            {
                string[] mainpfadSplit = mainFolderPath.Split(sep);
                string[] folderName = tsName.Split(sep);
                
                

                if (!Directory.Exists(mainFolderPath+sep+"BuilderFolder"+sep)) 
                {

                    Con.print(mainFolderPath,ConsoleColor.Blue);
                    createFolder(mainFolderPath   ,sep+"BuilderFolder",sep);
                    projectFolderSearch(tsName,mainFolderPath,sep);

                }
                else
                {
                    if (tsName == mainFolderPath+sep+"BuilderFolder")
                    { 
                        continue;
                    }
                    else 
                    {
                        string finalString =mainFolderPath+sep+"BuilderFolder"+sep;
                        string finalmainpfadSplit =ListHelper.listSelector(folderName,mainpfadSplit.Length,folderName.Length,sep.ToString());
                        Con.print(finalString +sep+ finalmainpfadSplit,ConsoleColor.Green);
                        createFolder(finalString,finalmainpfadSplit ,sep);
                        projectFolderSearch(tsName,mainFolderPath,sep);

                    }
                }
                
            }
        }

        //Passes to the function Heandler, in the class Command, in the namamespace Shell, the command to convert Typscript files with tsc

        static void tSToJs(string tsName,string folderPath) 
        {
            string command = "tsc " + tsName;
            Console.WriteLine(command);
            Shell.Command.Heandler(command);
            Console.WriteLine("Finish");
            
        }

        //takes a path, an orner name and a separator and creates a folder from it 
        static void createFolder(string folderPath,string name,char sep)
        {
            
                Directory.CreateDirectory(folderPath+sep+name);
                Console.WriteLine(name);
            
        }

        //returns different path separators depending on the operating system
        static char osSeparator()
        {
            char separator='.';
            if(OperatingSystem.IsLinux())
            {
                 separator = '/';
                
            }
            else if(OperatingSystem.IsWindows())
            {
                 separator = '\\';
            }
            return separator;
        }

        //returns different commands depending on the operating system
        static string osCommand(string command)
        {
            if(OperatingSystem.IsLinux())
            {
                switch(command.ToLower())
                {
                    case "copy":
                        return "cp";
                    case "move":
                        return "mv";
                
                    default:
                        return "Fail";
                }   
            }
            else
            {
                switch(command.ToLower())
                {
                    case "copy":
                        return "copy";
                    case "move":
                        return "move";
                    default:
                        return "Fail";
                }
            }

        }

        //opens the master file
        static void runMasterFile()
        {   
            
        }

        


    }

}
