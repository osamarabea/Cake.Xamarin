using System.Collections.Generic;

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Core.Utilities;

namespace Cake.Xamarin
{
    /// <summary>
    /// MDTool settings.
    /// </summary>
    public class MDToolSettings : Cake.Core.Tooling.ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cake.Xamarin.MDToolSettings"/> class.
        /// </summary>
        public MDToolSettings () : base ()
        {
            Configuration = "Debug|iPhoneSimulator";
            Target = "Build";
        }

        /// <summary>
        /// Adds the -v flag to the mdtool command.
        /// </summary>
        /// <value><c>true</c> if increase verbosity; otherwise, <c>false</c>.</value>
        public bool IncreaseVerbosity { get; set; }

        /// <summary>
        /// Gets or sets the configuration mode to build for.
        /// </summary>
        /// <value>The configuration.</value>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the target to build.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }
    }

    internal class MDToolRunner : Cake.Core.Tooling.Tool<MDToolSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public MDToolRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator tools) 
            : base (fileSystem, cakeEnvironment, processRunner, tools)
        {
            _cakeEnvironment = cakeEnvironment;
        }

        protected override string GetToolName ()
        {
            return "vstool";
        }

        protected override IEnumerable<string> GetToolExecutableNames ()
        {
            yield return "vstool";
            yield return "vstool.exe";
            yield return "vstool.cmd";
            yield return "vstool.sh";
        }

        protected override IEnumerable<FilePath> GetAlternativeToolPaths (MDToolSettings settings)
        {
            return new [] { new FilePath (XamarinAliases.DEFAULT_MDTOOL_PATH) };
        }

        public void Build (FilePath projectFile, MDToolSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            if (settings.IncreaseVerbosity)
                builder.Append ("-v");
            
            builder.Append ("build");
            builder.Append ("-t:" + settings.Target ?? "Build");
            builder.Append ("-c:\"" + settings.Configuration + "\"");
            builder.AppendQuoted (projectFile.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder);
        }

        public void Archive (FilePath solutionFile, string projectName, MDToolSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            if (settings.IncreaseVerbosity)
                builder.Append ("-v");

            builder.Append ("archive");

            if (!string.IsNullOrEmpty (projectName))
                builder.Append ("-p:" + projectName);
            
            builder.Append ("-c:\"" + settings.Configuration + "\"");
            builder.AppendQuoted (solutionFile.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder);
        }
    }
}
