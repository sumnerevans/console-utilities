using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUtilities
{
    /// <summary>
    /// Parse command line arguments.
    /// </summary>
    public class ArgumentParser
    {
        private List<Argument> arguments { get; set; }
        private string[] commandLineArgs { get; set; }
        private string prefix { get; set; }

        /// <summary>
        /// Initialize the ArgumentsParser with possible arguments.
        /// </summary>
        public ArgumentParser(List<Argument> arguments)
        {
            this.arguments = arguments;
        }

        /// <summary>
        /// Initialize the ArgumentsParser with possible arguments and
        /// immediately load the argument values.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="args"></param>
        /// <param name="prefix"></param>
        public ArgumentParser(List<Argument> arguments, string[] args, string prefix = "/") : this(arguments)
        {
            this.LoadArgumentsFromCommandLineArgs(args, prefix: prefix);
        }

        public void LoadArgumentsFromCommandLineArgs(string[] args, string prefix = "/")
        {
            this.commandLineArgs = args;
            this.prefix = prefix;

            foreach (var arg in this.arguments)
            {
                if (arg.IsFlag)
                    arg.Value = this.getArgumentIndex(arg.Names) != null;
                else
                    arg.Value = this.getArgumentValue(arg.Names, arg.DefaultValue);
            }
        }

        private string getArgumentValue(string[] argNames, object defaultValue)
        {
            int? argIndex = getArgumentIndex(argNames);

            // Return null if the argument doesn't exist or has no associated value
            if (argIndex == null || argIndex == this.commandLineArgs.Length - 1)
                return null;

            return this.commandLineArgs[argIndex.Value + 1];
        }

        private int? getArgumentIndex(string[] argNames)
        {
            int? index = null;
            foreach (var argName in argNames)
            {
                int? indexOfArg = Array.IndexOf(this.commandLineArgs, $"{this.prefix}{argName}");
                if (indexOfArg > -1)
                {
                    index = indexOfArg;
                    break;
                }
            }

            return index;
        }

        public virtual bool ValidateArgs()
        {
            var validationProblems = new List<string>();
            foreach (var arg in this.arguments)
            {
                string validationProblem = arg.Validate();
                if (!string.IsNullOrEmpty(validationProblem))
                    validationProblems.Add(validationProblem);
            }

            if (validationProblems.Count > 0)
            {
                // TODO: PRINT ERRORS
                return false;
            }

            return true;
        }
    }

    public class Argument
    {
        public string[] Names { get; set; }
        public bool Required { get; set; }
        public object DefaultValue { get; set; }
        public bool IsFlag { get; set; }
        public Type ObjectType { get; set; }

        private object _value = null;
        public object Value
        {
            get
            {
                return this._value;
            }
            internal set
            {
                this._value = Convert.ChangeType(value, this.ObjectType.GetType());
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return this.Required ? this.Value != null : true;
            }
        }

        public virtual string Validate()
        {
            return !this.IsValid ? $"Argument {this.Names} not found in command line arguments" : null;
        }
    }
}