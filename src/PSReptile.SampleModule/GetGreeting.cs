using System.Management.Automation;

namespace PSReptile.SampleModule
{
    /// <summary>
    ///     A simple Cmdlet that outputs a greeting to the pipeline.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Greeting")]
    [CmdletHelp(
        Synopsis = "A simple Cmdlet that outputs a greeting to the pipeline",
        Description = @"
            This Cmdlet works with greetings.
            It gets them.
            I can't see how to make it any clearer than that.
        "
    )]
    public class GetGreeting
        : Cmdlet
    {
        /// <summary>
        ///     The name of the person to greet.
        /// </summary>
        [ValidateNotNullOrEmpty]
        [Parameter(Mandatory = true, HelpMessage = "The name of the person to greet")]
        public string Name { get; set; }

        /// <summary>
        ///     Perform Cmdlet processing.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteObject($"Hello, {Name}!");
        }
    }
}
