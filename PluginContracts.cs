using System.Collections.Generic;
using TSOutputWriter;
using System.Collections;
using System.Drawing;
using OutputHelperLib;
using System.IO;


namespace PluginContracts
{
    public interface Plugin
    {

        //these are the core objects that get passed from one plugin to the next
        string[] InputType { get; }
        string OutputType { get; }

        //get the details of the plugin:
        //PluginName -- a string that contains the name of the plugin (this is what will be shown in the tree list)
        //PluginType -- a string that contains the "category" that the plugin falls under (will also be added to the tree list)
        //PluginVersion -- a string that contains the plugin's version number
        //PluginAuthor -- a string that contains the plugin's author information
        //PluginDescription -- the description that will show up when the plugin is selected
        //PluginTutorial -- a string containing a URL to further information/help about the plugin
        string PluginName { get; }
        string PluginType { get; }
        string PluginVersion { get; }
        string PluginAuthor { get; }
        string PluginDescription { get; }
        string PluginTutorial { get; }
        bool TopLevel { get; }


        //get the icon (32x32 .ico file) for the plugin
        Icon GetPluginIcon { get; }

        //use this to change the plugin's settings
        void ChangeSettings();

        //anything that needs to happen prior to plugin execution happens here (e.g., loading external files, etc.) 
        void Initialize();

        //anything that needs to be wrapped up after
        Payload FinishUp(Payload Input);

        //double-check the user to make sure that they have selected usable settings
        //for example, make sure that they've selected a folder for input/output
        bool InspectSettings();

        //methods for exporting and importing each plugin's settings
        Dictionary<string, string> ExportSettings(bool suppressWarnings);
        void ImportSettings(Dictionary<string, string> Settings);

        Payload RunPlugin(Payload Input);

        //whether this plugin should inherit its header from its parent.
        //for a fixed header that will get passed to the CSV writer, use something like this:
        Dictionary<int, string> OutputHeaderData { get; set; }
        //public Dictionary<int, string> OutputHeaderData { get; set;  } = new Dictionary<int, string>(){
        //                                                                                   {0, "TaggedText"},
        //                                                                                   {1, "posemo"},
        //                                                                                   {2, "negemo"},
        //                                                                                      };
        //
        //note that you don't have to include the filename of the segment number in the header,
        //as the csvwriter handles those automatically
        bool InheritHeader { get; }


    }




    public interface InputPlugin : Plugin
    {
        StreamReader InputStream { get; set; }
        IEnumerable TextEnumeration();
        //string GetFileIdentifier(object TextInputItem);
        bool KeepStreamOpen { get; }
        string SelectedEncoding { get; set; }
        string IncomingTextLocation { get; set; }

        //we use the initialization process for an input stream to get the number of files
        int TextCount { get; set; }

    }


    public interface OutputPlugin : Plugin
    {

        bool KeepStreamOpen { get; }
        string OutputLocation { get; set; }
        string SelectedEncoding { get; set; }
        void WriteHeader();
        bool headerWritten { get; set; }
        ThreadsafeOutputWriter Writer { get; set; }        
        System.IO.FileMode fileMode { get; set; }
    }





    public interface LinearPlugin : Plugin
    {

        Payload RunPlugin(Payload Input, int ThreadsAvailable);
        string StatusToReport { get; set; }

    }





    }
